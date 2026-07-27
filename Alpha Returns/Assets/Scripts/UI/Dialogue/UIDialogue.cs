using UnityEngine;
using Ink.Runtime;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class UIDialogue : MonoBehaviour
{
    [Header("UI Layout")]
    [SerializeField] private UIDialogueComponent _leftLayout;
    [SerializeField] private UIDialogueComponent _rightLayout;
    [SerializeField] private UIDialogueComponent _centerLayout;

    [Header("Choice Settings")]
    [SerializeField] private GameObject _choicePrefab;

    private UIDialogueComponent _activeLayout;
    private DialogueLayout _currentLayoutType;

    private string _fullLine;

    private bool _isTyping = false;
    private bool _hasChoices = false;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        SingletonHub.Instance.Get<DialogueManager>().OnRequestChoices += DisplayChoices;
        SingletonHub.Instance.Get<DialogueManager>().OnRequestLine += ProcessLine;
    }

    private void OnDisable()
    {
        SingletonHub.Instance.Get<DialogueManager>().OnRequestChoices -= DisplayChoices;
        SingletonHub.Instance.Get<DialogueManager>().OnRequestLine -= ProcessLine;
    }

    #region Input Handlers

    public void HandleInput()
    {
        if (_hasChoices) return;

        if (_isTyping)
        {
            StopCoroutine(_coroutine);
            FinishLine();
        }
        else
        {
            SingletonHub.Instance.Get<DialogueManager>().ContinueStory();
        }
    }

    #endregion

    #region Text Rendering

    private void ProcessLine(DialogueData data, float speed, System.Action onComplete)
    {
        ClearChoices();

        SwitchLayout(data.Layout, () =>
        {
            _fullLine = data.Text;
            _activeLayout.SetupVisuals(data);
            _activeLayout.SetNextIndicatorActive(!_hasChoices);

            SingletonHub.Instance.Get<DialogueManager>().OnCompleteLine = onComplete;

            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(TypeLine(speed));
        });
    }

    private void SwitchLayout(DialogueLayout targetLayoutType, System.Action onComplete)
    {
        UIDialogueComponent targetLayout = GetLayoutComponent(targetLayoutType);

        if (_activeLayout == targetLayout)
        {
            onComplete?.Invoke();
            return;
        }

        if (_activeLayout != null)
        {
            EndCurrentLayout(() =>
            {
                ClearChoices();
                _activeLayout = targetLayout;
                _currentLayoutType = targetLayoutType;
                ApplyCurrentLayout();
                onComplete?.Invoke();
            });
        }
        else
        {
            _activeLayout = targetLayout;
            _currentLayoutType = targetLayoutType;
            ApplyCurrentLayout();
            onComplete?.Invoke();
        }
    }

    public IEnumerator TypeLine(float speed)
    {
        _isTyping = true;
        _activeLayout.DialogueText.text = "";

        foreach (char letter in _fullLine.ToCharArray())
        {
            _activeLayout.DialogueText.text += letter;
            yield return new WaitForSeconds(speed);
        }

        FinishLine();
    }

    private void FinishLine()
    {
        _isTyping = false;
        _activeLayout.DialogueText.text = _fullLine;
        _coroutine = null;

        SingletonHub.Instance.Get<DialogueManager>().OnCompleteLine?.Invoke();
    }

    #endregion

    #region Layout UI

    private void ApplyCurrentLayout()
    {
        switch (_currentLayoutType)
        {
            case DialogueLayout.Left:
                _leftLayout.ApplyLeftLayout();
                break;
            case DialogueLayout.Right:
                _rightLayout.ApplyRightLayout();
                break;
            case DialogueLayout.Center:
                _centerLayout.ApplyCenterLayout();
                break;
        }
    }

    private void EndCurrentLayout(System.Action onComplete = null)
    {
        switch (_currentLayoutType)
        {
            case DialogueLayout.Left:
                _leftLayout.EndLeftLayout(onComplete);
                break;
            case DialogueLayout.Right:
                _rightLayout.EndRightLayout(onComplete);
                break;
            case DialogueLayout.Center:
                _centerLayout.EndCenterLayout(onComplete);
                break;
            default:
                onComplete?.Invoke();
                break;
        }
    }

    #endregion

    #region Choice Management

    public void DisplayChoices(List<Choice> choices, System.Action<int> onChoiceSelected)
    {
        _hasChoices = true;

        if (_activeLayout != null)
        {
            _activeLayout.SetNextIndicatorActive(false);
        }

        ClearChoices();

        for (int i = 0; i < choices.Count; i++)
        {
            var choice = choices[i];
            GameObject rootObj = Instantiate(_choicePrefab, _activeLayout.ChoiceContainer);
            Transform visual = rootObj.transform.GetChild(0);

            if (rootObj.TryGetComponent<UIDialogueChoice>(out var choiceUI))
            {
                choiceUI.Setup(choice.text, choice.index, onChoiceSelected);
            }

            visual.localScale = Vector3.zero;
            Vector3 pos = visual.localPosition;
            visual.localPosition = new Vector3(pos.x, -50f, pos.z);

            float delay = i * 0.1f;

            LeanTween.moveLocalY(visual.gameObject, 0f, 0.4f)
                .setDelay(delay)
                .setEase(LeanTweenType.easeOutBack);

            LeanTween.scale(visual.gameObject, Vector3.one, 0.4f)
                .setDelay(delay)
                .setEase(LeanTweenType.easeOutBack);
        }
    }

    public void ClearChoices()
    {
        _hasChoices = false;

        if (_activeLayout == null ||
                _activeLayout.ChoiceContainer == null ||
                _activeLayout.ChoiceContainer.childCount == 0)
        {
            return;
        }

        int childCount = _activeLayout.ChoiceContainer.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = _activeLayout.ChoiceContainer.GetChild(i);
            Transform visual = child.GetChild(0);
            float delay = i * 0.1f;

            LeanTween.cancel(visual.gameObject);

            LeanTween.moveLocalY(visual.gameObject, 250f, 0.4f)
                .setDelay(delay)
                .setEase(LeanTweenType.easeInBack);

            LeanTween.scale(visual.gameObject, Vector3.zero, 0.4f)
                .setDelay(delay)
                .setEase(LeanTweenType.easeInBack)
                .setOnComplete(() =>
                {
                    Destroy(child.gameObject);
                    Debug.Log("[DialogueUI] Choice berhasil di-destroy (Immediate).");
                });
        }
    }

    #endregion

    #region Helpers

    private UIDialogueComponent GetLayoutComponent(DialogueLayout layout)
    {
        return layout switch
        {
            DialogueLayout.Left => _leftLayout,
            DialogueLayout.Right => _rightLayout,
            DialogueLayout.Center => _centerLayout,
            _ => _leftLayout
        };
    }

    //public void ShowPanel(bool isVisible) => _dialoguePanel.SetActive(isVisible);

    #endregion
}