using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _runSpeed;

    [SerializeField]
    private float _sprintSpeed;

    [SerializeField]
    private float _landSpeed;

    [SerializeField]
    private float _lockOnRotationSpeed;

    private Camera _mainCamera;
    private Animator _animator;
    private CharacterMovement _movement;
    private ThirdPersonCamera _camera;
    private LockOn _lockOn;
    private UI_LockOn _lockOnUI;

    // animation IDs
    private readonly int _animIDSpeed = Animator.StringToHash("Speed");
    private readonly int _animIDPosX = Animator.StringToHash("PosX");
    private readonly int _animIDPosY = Animator.StringToHash("PosY");
    private readonly int _animIDGrounded = Animator.StringToHash("Grounded");
    private readonly int _animIDJump = Animator.StringToHash("Jump");
    private readonly int _animIDFall = Animator.StringToHash("Fall");

    private void Awake()
    {
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
        _movement = GetComponent<CharacterMovement>();
        _camera = GetComponent<ThirdPersonCamera>();
        _lockOn = GetComponent<LockOn>();
    }

    private void Start()
    {
        _lockOnUI = Managers.Resource.Instantiate<UI_LockOn>("UI_LockOn.prefab");
        _movement.MoveSpeed = _runSpeed;
        InitInputActions();
    }

    private void Update()
    {
        _movement.Gravity();
        _animator.SetBool(_animIDFall, _movement.IsFalling);

        _movement.CheckGrounded();
        _animator.SetBool(_animIDGrounded, _movement.IsGrounded);

        HandleMovement();
        UpdateAnimatorParameters();
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

    private void HandleMovement()
    {
        var move = Managers.Input.Move;
        var inputDirection = new Vector3(move.x, 0f, move.y).normalized;
        float cameraYaw = _mainCamera.transform.eulerAngles.y;

        _movement.MoveSpeed = _movement.IsLanding ? _landSpeed : Managers.Input.Sprint ? _sprintSpeed : _runSpeed;
        _movement.Move(inputDirection, cameraYaw);

        if (_lockOn.IsLockOn && IsOnlyRun())
        {
            var rotationDirection = inputDirection == Vector3.zero
                ? Vector3.zero
                : (_lockOn.Target.position - transform.position).normalized;
            _movement.Rotate(rotationDirection);
        }
        else
        {
            _movement.Rotate(inputDirection, cameraYaw);
        }
    }

    private void UpdateAnimatorParameters()
    {
        bool isLockOnOnlyRun = _lockOn.IsLockOn && IsOnlyRun();
        _animator.SetFloat(_animIDSpeed, _movement.SpeedBlend);
        _animator.SetFloat(_animIDPosX, isLockOnOnlyRun ? _movement.PosXBlend : 0f);
        _animator.SetFloat(_animIDPosY, isLockOnOnlyRun ? _movement.PosYBlend : 1f);
    }

    private bool IsOnlyRun()
    {
        return !(Managers.Input.Sprint || _movement.IsJumping || _movement.IsFalling || _movement.IsLanding);
    }

    private void InitInputActions()
    {
        Managers.Input.GetAction("Jump").performed += context =>
        {
            _movement.Jump();
            _animator.SetTrigger(_animIDJump);
        };
        Managers.Input.GetAction("Sprint").started += context => _movement.MoveSpeed = _sprintSpeed;
        Managers.Input.GetAction("Sprint").canceled += context => _movement.MoveSpeed = _runSpeed;
        Managers.Input.GetAction("LockOn").performed += context =>
        {
            if (_lockOn.IsLockOn)
            {
                _lockOn.Target = null;
            }
            else
            {
                _lockOn.FindTarget(_mainCamera.transform, target =>
                {
                    // 타겟이 절두체 안에 있는지 확인
                    var planes = GeometryUtility.CalculateFrustumPlanes(_mainCamera);
                    var bounds = target.GetComponent<Collider>().bounds;
                    return GeometryUtility.TestPlanesAABB(planes, bounds);
                });
            }

            _lockOnUI.Target = _lockOn.Target;
        };
    }
}
