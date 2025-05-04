using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputBroadcaster : MonoBehaviour
{
    private InputSystem_Actions _inputSystemActions;
    public event Action<Vector2> TouchHoldChanged;
    public event Action<Vector2> TouchTapPerformed;
    public Vector2 TouchStartPosition { get; private set; }
    public Vector2 TouchCurrentPosition { get; private set; }
    public bool TouchHeld { get; private set; } = false;

    private void Awake()
    {
        _inputSystemActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputSystemActions.Enable();
        _inputSystemActions.Player.TouchHold.performed += OnTouchHoldStart;
        _inputSystemActions.Player.TouchHold.canceled += OnTouchHoldEnd;
        _inputSystemActions.Player.TouchTap.performed += OnTouchTap;
    }

    private void OnDisable()
    {
        _inputSystemActions.Player.TouchHold.performed -= OnTouchHoldStart;
        _inputSystemActions.Player.TouchHold.canceled -= OnTouchHoldEnd;
        _inputSystemActions.Player.TouchTap.performed -= OnTouchTap;
        _inputSystemActions.Disable();
    }

    private void OnTouchHoldStart(InputAction.CallbackContext context)
    {
        TouchHeld = true;
        Vector2 TouchPosition = context.ReadValue<Vector2>();
        TouchStartPosition = TouchPosition;
        TouchCurrentPosition = TouchPosition;
        //Debug.Log("Touch Start Position: " + TouchStartPosition);
    }

    private void OnTouchHoldEnd(InputAction.CallbackContext context)
    {
        TouchHeld = false;
        //Debug.Log("Touch End Position: " + TouchCurrentPosition);

        TouchStartPosition = Vector2.zero;
        TouchCurrentPosition = Vector2.zero;
    }

    private void OnTouchTap(InputAction.CallbackContext context)
    {
        TouchTapPerformed?.Invoke(context.ReadValue<Vector2>());
    }

    private void Update()
    {
        if (TouchHeld)
        {
            Vector2 touchLastPosition = TouchCurrentPosition;
            TouchCurrentPosition = _inputSystemActions.Player.TouchHold.ReadValue<Vector2>();
            Vector2 touchDelta = TouchCurrentPosition - touchLastPosition;
            TouchHoldChanged?.Invoke(touchDelta);
        }
    }
}
