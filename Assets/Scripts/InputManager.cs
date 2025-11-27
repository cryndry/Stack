using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : LazySingleton<InputManager>
{
    private InputSystemActions inputSystemActions;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        inputSystemActions = new InputSystemActions();
    }

    private void OnEnable()
    {
        inputSystemActions.UI.Click.Enable();
        inputSystemActions.UI.Click.performed += OnLeftClickCallback;
    }

    private void OnDisable()
    {
        inputSystemActions.UI.Click.performed -= OnLeftClickCallback;
        inputSystemActions.UI.Click.Disable();
    }

    private void OnLeftClickCallback(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        RaycastHit2D hit2D = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
        if (hit2D.collider != null)
        {
            EventManager.Instance.InvokeTapOnUI(hit2D.collider);
        }
        else
        {
            EventManager.Instance.InvokeTapTouchLayer();
        }
    }
}
