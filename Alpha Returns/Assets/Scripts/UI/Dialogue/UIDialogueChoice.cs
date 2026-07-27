using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueChoice : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _button;

    public void Setup(string text, int index, System.Action<int> onSelect)
    {
        _text.text = text;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => onSelect?.Invoke(index));
    }
}
