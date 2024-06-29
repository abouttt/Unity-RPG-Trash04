using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject _mainCamera;
    private CharacterMovement _movement;
    private ThirdPersonCamera _camera;

    private void Awake()
    {
        _mainCamera = Camera.main.gameObject;
        _movement = GetComponent<CharacterMovement>();
        _camera = GetComponent<ThirdPersonCamera>();
    }

    private void Start()
    {
        Managers.Input.GetAction("Jump").performed += context => _movement.Jump();
    }

    private void Update()
    {
        _movement.Gravity();
        _movement.CheckGrounded();
        _movement.MoveAndRotate(Managers.Input.Move, _mainCamera.transform.eulerAngles.y);
    }

    private void LateUpdate()
    {
        _camera.Rotate();
    }
}
