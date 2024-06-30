using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _lockOnRotationSpeed;

    private GameObject _mainCamera;
    private CharacterMovement _movement;
    private ThirdPersonCamera _camera;
    private LockOn _lockOn;

    private void Awake()
    {
        _mainCamera = Camera.main.gameObject;
        _movement = GetComponent<CharacterMovement>();
        _camera = GetComponent<ThirdPersonCamera>();
        _lockOn = GetComponent<LockOn>();
    }

    private void Start()
    {
        Managers.Input.GetAction("Jump").performed += context => _movement.Jump();
        Managers.Input.GetAction("LockOn").performed += context =>
        {
            if (_lockOn.IsLockOn)
            {
                _lockOn.Target = null;
            }
            else
            {
                _lockOn.FindTarget(_mainCamera.transform);
            }
        };
    }

    private void Update()
    {
        _movement.Gravity();
        _movement.CheckGrounded();
        _movement.MoveAndRotate(Managers.Input.Move, _mainCamera.transform.eulerAngles.y);
    }

    private void LateUpdate()
    {
        if (_lockOn.IsLockOn)
        {
            _camera.LookRotate((_lockOn.Target.position + transform.position) * 0.5f, _lockOnRotationSpeed);
            _lockOn.TrackingTarget(_mainCamera.transform);
        }
        else
        {
            var look = Managers.Input.Look;
            _camera.Rotate(look.y, look.x);
        }
    }
}
