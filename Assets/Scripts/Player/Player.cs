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

        var move = Managers.Input.Move;
        var inputDirection = new Vector3(move.x, 0f, move.y).normalized;
        if (_lockOn.IsLockOn)
        {
            HandleLockOnMovement(inputDirection);
        }
        else
        {
            _movement.MoveAndRotate(inputDirection, _mainCamera.transform.eulerAngles.y);
        }
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

    private void HandleLockOnMovement(Vector3 move)
    {
        Vector3 directionToTarget = (_lockOn.Target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        _movement.Move(move, lookRotation.eulerAngles.y);

        if (move != Vector3.zero)
        {
            if (_movement.IsJumping || _movement.IsFalling)
            {
                _movement.Rotate(move, _mainCamera.transform.eulerAngles.y);
            }
            else
            {
                _movement.Rotate(directionToTarget);
            }
        }
    }
}
