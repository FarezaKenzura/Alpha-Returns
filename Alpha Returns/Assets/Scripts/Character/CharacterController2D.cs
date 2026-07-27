using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private CharacterMovement2D _movement;
    [SerializeField] private CharacterIndicator _indicator;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (!SingletonHub.Instance.Get<InputManager>().IsGameplayActive()) return;

        if (Pointer.current.press.wasPressedThisFrame)
        {
            if (SingletonHub.Instance.Get<InputManager>().IsPointerOverUI()) return;

            Vector2 screenPos = Pointer.current.position.ReadValue();
            Vector3 worldPos = _mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));

            Vector3 snappedPos = _movement.OnSnappedPos.Invoke(worldPos);

            _indicator.OnActiveIndicator?.Invoke(snappedPos);
            _movement.OnMoveCommand?.Invoke(snappedPos);
        }
    }
}
