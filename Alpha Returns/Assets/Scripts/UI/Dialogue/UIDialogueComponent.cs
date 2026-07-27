using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIDialogueComponent
{
    [Header("UI Containers")]
    public RectTransform DialogueStand;
    public RectTransform DialogueChat;
    public Transform ChoiceContainer;

    [Header("UI Elements")]
    public Image PortraitImage;
    public Image ChatImage;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI DialogueText;
    public GameObject NextIndicator;

    public void SetupVisuals(DialogueData data)
    {
        NameText.text = data.Visual.DisplayName;
        //ChatImage.sprite = data.Visual.ChatSprite;
        //PortraitImage.sprite = data.Visual.PortraitSprite;
    }

    public void SetNextIndicatorActive(bool isActive)
    {
        NextIndicator.SetActive(isActive);

        if (isActive)
        {
            if (!LeanTween.isTweening(NextIndicator))
            {
                NextIndicator.transform.localPosition = new Vector3(NextIndicator.transform.localPosition.x, -50f, 0);
                LeanTween.moveLocalY(NextIndicator, -60f, 0.6f)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setLoopPingPong();
            }
        }
        else
        {
            LeanTween.cancel(NextIndicator);
        }
    }

    #region Unique Animations per Layout

    public void ApplyLeftLayout()
    {
        CancelTweens();
        DialogueStand.pivot = new Vector2(0f, 0f);

        LeanTween.scale(DialogueStand.gameObject, Vector3.one, 0.4f)
            .setEase(LeanTweenType.easeOutBack);

        LeanTween.scale(DialogueChat.gameObject, Vector3.one, 0.3f)
            .setDelay(0.2f)
            .setEase(LeanTweenType.easeOutQuad);
    }

    public void EndLeftLayout(System.Action onComplete = null)
    {
        CancelTweens();
        DialogueStand.pivot = new Vector2(0f, 0f);

        LeanTween.scale(DialogueChat.gameObject, Vector3.zero, 0.3f)
            .setEase(LeanTweenType.easeInQuad);

        LeanTween.scale(DialogueStand.gameObject, Vector3.zero, 0.4f)
            .setDelay(0.2f)
            .setEase(LeanTweenType.easeInBack)
            .setOnComplete(() => onComplete?.Invoke());
    }

    public void ApplyRightLayout()
    {
        CancelTweens();
        DialogueStand.pivot = new Vector2(1f, 0f);

        LeanTween.scale(DialogueStand.gameObject, Vector3.one, 0.4f)
            .setEase(LeanTweenType.easeOutBack);

        LeanTween.scale(DialogueChat.gameObject, Vector3.one, 0.3f)
            .setDelay(0.2f)
            .setEase(LeanTweenType.easeOutQuad);
    }

    public void EndRightLayout(System.Action onComplete = null)
    {
        CancelTweens();
        DialogueStand.pivot = new Vector2(1f, 0f);

        LeanTween.scale(DialogueChat.gameObject, Vector3.zero, 0.3f)
            .setEase(LeanTweenType.easeInQuad);

        LeanTween.scale(DialogueStand.gameObject, Vector3.zero, 0.4f)
            .setDelay(0.2f)
            .setEase(LeanTweenType.easeInBack)
            .setOnComplete(() => onComplete?.Invoke());
    }

    public void ApplyCenterLayout()
    {
        CancelTweens();
        DialogueStand.pivot = new Vector2(0.5f, 0.5f);

        DialogueStand.localScale = Vector3.zero;
        DialogueChat.localScale = Vector3.zero;

        LeanTween.scale(DialogueStand.gameObject, Vector3.one, 0.4f)
            .setEase(LeanTweenType.easeOutBack);

        LeanTween.scale(DialogueChat.gameObject, Vector3.one, 0.3f)
            .setDelay(0.2f)
            .setEase(LeanTweenType.easeOutBack);
    }

    public void EndCenterLayout(System.Action onComplete = null)
    {
        CancelTweens();

        LeanTween.scale(DialogueChat.gameObject, Vector3.zero, 0.3f)
            .setEase(LeanTweenType.easeInBack);

        LeanTween.scale(DialogueStand.gameObject, Vector3.zero, 0.4f)
            .setDelay(0.2f)
            .setEase(LeanTweenType.easeInBack)
            .setOnComplete(() => onComplete?.Invoke());
    }

    private void CancelTweens()
    {
        if (DialogueStand != null) LeanTween.cancel(DialogueStand.gameObject);
        if (DialogueChat != null) LeanTween.cancel(DialogueChat.gameObject);
    }

    #endregion
}
