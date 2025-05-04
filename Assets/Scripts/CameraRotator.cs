using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    private GameController _controller;
    [SerializeField] private float _sensitivity = 100;

    private float _pitch = 0;
    private float _yaw = 0;

    private void Awake()
    {
        _controller = FindAnyObjectByType<GameController>();
    }

    private void OnEnable()
    {
        _controller.Input.TouchHoldChanged += RotateCamera;
    }

    private void OnDisable()
    {
        _controller.Input.TouchHoldChanged -= RotateCamera;
    }

    private void RotateCamera(Vector2 touchDelta)
    {
        _pitch -= touchDelta.y * _sensitivity * Time.unscaledDeltaTime;
        _yaw += touchDelta.x * _sensitivity * Time.unscaledDeltaTime;

        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }
}
