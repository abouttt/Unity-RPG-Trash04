using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, GameControls.IPlayerActions
{
    public static LockOn LockOn { get; private set; }
    public static Interactor Interactor { get; private set; }

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

    private Camera _mainCamera;
    private Animator _animator;
    private CharacterMovement _movement;
    private ThirdPersonCamera _thirdPersonCamera;

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
        _thirdPersonCamera = GetComponent<ThirdPersonCamera>();
        LockOn = GetComponent<LockOn>();
        Interactor = GetComponentInChildren<Interactor>();
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
        bool isLockOnOnlyRun = LockOn.IsLockOn && IsOnlyRun();
        _animator.SetFloat(_animIDSpeed, _movement.SpeedBlend);
        _animator.SetFloat(_animIDPosX, isLockOnOnlyRun ? _movement.PosXBlend : 0f);
        _animator.SetFloat(_animIDPosY, isLockOnOnlyRun ? _movement.PosYBlend : 1f);
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

    public void OnCursorToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Managers.UI.IsShowedSelfishPopup)
            {
                return;
            }

            Managers.Input.CursorLocked = !Managers.Input.CursorLocked;
        }
    }
    #endregion
}
