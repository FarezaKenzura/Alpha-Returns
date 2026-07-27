using UnityEngine;

public class CharacterIndicator : MonoBehaviour
{
    public System.Action<Vector3> OnActiveIndicator;
    [SerializeField] private GameObject _point;
    [SerializeField] private SpriteRenderer _sprite;

    private void OnEnable()
    {
        OnActiveIndicator += Play;
    }

    private void OnDisable()
    {
        OnActiveIndicator -= Play;
    }

    public void Play(Vector3 position)
    {
        LeanTween.cancel(_point);

        _point.transform.position = position;
        _point.transform.localScale = Vector3.zero;

        LeanTween.alpha(_point, 1f, 0f);

        _point.SetActive(true);

        LeanTween.scale(_point, Vector3.one, 0.4f)
            .setEaseOutQuad();

        LeanTween.alpha(_point, 0f, 0.4f)
            .setEaseInQuad()
            .setOnComplete(() =>
            {
                _point.SetActive(false);
            });
    }
}
