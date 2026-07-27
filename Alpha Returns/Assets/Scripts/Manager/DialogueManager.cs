using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public enum DialogueLayout
{
    Center,
    Left,
    Right
}

public class VisualData
{
    public string SpeakerID;
    public string DisplayName;
    public Sprite PortraitSprite;
    public Sprite ChatSprite;
}

public class DialogueData
{
    public string Text;
    public VisualData Visual;
    public DialogueLayout Layout;

    public DialogueData(string text)
    {
        this.Text = text;
        this.Visual = new VisualData();
    }
}

public class DialogueManager : MonoBehaviour
{
    public System.Action<DialogueData, float, System.Action> OnRequestLine;
    public System.Action<List<Choice>, System.Action<int>> OnRequestChoices;
    public System.Action OnCompleteLine;

    [Header("Params")]
    [SerializeField] private float _typingSpeed = 0.04f;

    [SerializeField] private SOStoryData _storyDB;
    [SerializeField] private SOCharacterData _characterDB;

    private Story _story;
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string CHAT_TAG = "chat";
    private const string ITEM_TAG = "item";
    private const string LAYOUT_TAG = "layout";

    private void Awake()
    {
        SingletonHub.Instance.Register(this);
    }

    private void Start()
    {
        StartChapter("C1");
    }

    public void StartChapter(string chapterID)
    {
        ChapterData chapter = _storyDB.GetChapter(chapterID);
        EnterDialogueMode(chapter.inkJSON);
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        SingletonHub.Instance.Get<InputManager>().SwitchInputMap(InputMap.UI);

        _story = new Story(inkJSON.text);
        ContinueStory();
    }

    public void ContinueStory()
    {
        if (_story.canContinue)
        {
            DialogueData data = new DialogueData(_story.Continue());
            HandleTags(_story.currentTags, data);

            OnRequestLine?.Invoke(data, _typingSpeed, () =>
            {
                if (_story.currentChoices.Count > 0)
                {
                    OnRequestChoices?.Invoke(_story.currentChoices, OnChoiceSelected);
                }
            });
        }
        else ExitDialogueMode();
    }

    private void OnChoiceSelected(int index)
    {
        _story.ChooseChoiceIndex(index);
        ContinueStory();
    }

    private void HandleTags(List<string> tags, DialogueData data)
    {
        string portraitTag = "";
        string chatTag = "";
        string itemTag = "";

        foreach (string tag in tags)
        {
            string[] split = tag.Split(':');
            if (split.Length != 2) continue;

            string key = split[0].Trim();
            string val = split[1].Trim();

            switch (key)
            {
                case SPEAKER_TAG: data.Visual.SpeakerID = val; break;
                case PORTRAIT_TAG: portraitTag = val; break;
                case CHAT_TAG: chatTag = val; break;
                case ITEM_TAG: itemTag = val; break;
                case LAYOUT_TAG:
                    System.Enum.TryParse(val, true, out data.Layout);
                    break;
            }
        }

        PopulateVisuals(data, portraitTag, chatTag);
    }

    private void PopulateVisuals(DialogueData data, string portraitTag, string chatTag)
    {
        CharacterData dbChar = _characterDB.GetCharacter(data.Visual.SpeakerID);

        if (dbChar == null)
        {
            data.Visual.DisplayName = data.Visual.SpeakerID;
            return;
        }

        data.Visual.DisplayName = dbChar.CharacterName;
        data.Visual.PortraitSprite = dbChar.GetPortrait(portraitTag);
        data.Visual.ChatSprite = dbChar.GetChat(chatTag);
    }

    private void ExitDialogueMode()
    {
        SingletonHub.Instance.Get<InputManager>().SwitchInputMap(InputMap.Player);

        _story = null;
    }
}
