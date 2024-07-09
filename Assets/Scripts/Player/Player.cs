using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, GameControls.IPlayerActions
{
    public static LockOn LockOn { get; private set; }
    public static Interactor Interactor { get; private set; }
    public static ItemInventory ItemInventory { get; private set; }

    [SerializeField]
    private float _runSpeed;

    [SerializeField]
    private float _sprintSpeed;

    [SerializeField]
    private float _landSpeed;

    [SerializeField]
    private float _lockOnRotationSpeed;

    private Vector2 _move;
    private Vector2 _look;
    private bool _isPressedSprint;

    // 애니메이션 블렌드
    private float _speedBlend;
    private float _posXBlend;
    private float _posYBlend;

    private Camera _mainCamera;
    private Animator _animator;
    private CharacterMovement _movement;
    private ThirdPersonCamera _thirdPersonCamera;

    // 애니메이션 아이디
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
        _thirdPersonCamera = GetComponent<ThirdPersonCamera>();
        LockOn = GetComponent<LockOn>();
        Interactor = GetComponentInChildren<Interactor>();
        ItemInventory = GetComponent<ItemInventory>();
    }

    private void Start()
    {
        Managers.Resource.Instantiate<UI_Interaction>("UI_Interaction.prefab");
        Managers.Resource.Instantiate<UI_LockOn>("UI_LockOn.prefab");

        _movement.MoveSpeed = _runSpeed;
    }

    private void OnEnable()
    {
        Managers.Input.Player.SetCallbacks(this);
        Managers.Input.Player.Enable();
    }

    private void OnDisable()
    {
        if (Managers.Instance != null)
        {
            Managers.Input.Player.RemoveCallbacks(this);
            Managers.Input.Player.Disable();
        }
    }

    private void Update()
    {
        _movement.Gravity();
        _movement.CheckGrounded();

        HandleMovement();
        UpdateAnimatorParameters();
    }

    private void LateUpdate()
    {
        if (LockOn.IsLockOn)
        {
            _thirdPersonCamera.LookRotate((LockOn.Target.position + transform.position) * 0.5f, _lockOnRotationSpeed);
            LockOn.TrackingTarget(_mainCamera.transform);
        }
        else
        {
            _thirdPersonCamera.Rotate(_look.y, _look.x);
        }
    }

    private void HandleMovement()
    {
        var inputDirection = new Vector3(_move.x, 0f, _move.y).normalized;
        float cameraYaw = _mainCamera.transform.eulerAngles.y;

        _movement.MoveSpeed = _movement.IsLanding ? _landSpeed : _isPressedSprint ? _sprintSpeed : _runSpeed;
        _movement.Move(inputDirection, cameraYaw);

        if (LockOn.IsLockOn && IsOnlyRun())
        {
            var rotationDirection = inputDirection == Vector3.zero
                ? Vector3.zero
                : (LockOn.Target.position - transform.position).normalized;
            _movement.Rotate(rotationDirection);
        }
        else
        {
            _movement.Rotate(inputDirection, cameraYaw);
        }
    }

    private void UpdateAnimatorParameters()
    {
        var inputDirection = new Vector3(_move.x, 0f, _move.y).normalized;
        bool isLockOnOnlyRun = LockOn.IsLockOn && IsOnlyRun();
        float targetSpeed = inputDirection == Vector3.zero ? 0 : _movement.MoveSpeed;
        float speedChangeRate = _movement.SpeedChangeRate * Time.deltaTime;

        _speedBlend = Mathf.Lerp(_speedBlend, targetSpeed, speedChangeRate);
        _posXBlend = Mathf.Lerp(_posXBlend, inputDirection.x, speedChangeRate);
        _posYBlend = Mathf.Lerp(_posYBlend, inputDirection.z, speedChangeRate);
        if (_speedBlend < 0.01f)
        {
            _speedBlend = 0f;
            _posXBlend = 0f;
            _posYBlend = 0f;
        }

        _animator.SetFloat(_animIDSpeed, _speedBlend);
        _animator.SetFloat(_animIDPosX, isLockOnOnlyRun ? _posXBlend : 0f);
        _animator.SetFloat(_animIDPosY, isLockOnOnlyRun ? _posYBlend : 1f);
        _animator.SetBool(_animIDGrounded, _movement.IsGrounded);
        _animator.SetBool(_animIDJump, _movement.IsJumping);
        _animator.SetBool(_animIDFall, _movement.IsFalling);
    }

    private bool IsOnlyRun()
    {
        return !(_isPressedSprint || _movement.IsJumping || _movement.IsFalling || _movement.IsLanding);
    }

    #region Input
    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (Managers.Input.CursorLocked)
        {
            _look = context.ReadValue<Vector2>();
        }
        else
        {
            _look = Vector2.zero;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isPressedSprint = true;
        }
        else if (context.canceled)
        {
            _isPressedSprint = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _movement.Jump();
        }
    }

    public void OnLockOn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (LockOn.IsLockOn)
            {
                LockOn.Target = null;
            }
            else
            {
                LockOn.FindTarget(_mainCamera.transform, target =>
                {
                    // 타겟이 절두체 안에 있는지 확인
                    var planes = GeometryUtility.CalculateFrustumPlanes(_mainCamera);
                    var bounds = target.GetComponent<Collider>().bounds;
                    return GeometryUtility.TestPlanesAABB(planes, bounds);
                });
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Interactor.Interact = true;
        }
        else if (context.canceled)
        {
            Interactor.Interact = false;
        }
    }
    #endregion
}
