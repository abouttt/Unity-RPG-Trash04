using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject _mainCamera;
    private CharacterMovement _movement;

    private void Awake()
    {
        _mainCamera = Camera.main.gameObject;
        _movement = GetComponent<CharacterMovement>();
    }

    private void Start()
    {
        Managers.Input.GetAction("Jump").performed += context => _movement.Jump();
    }

    private void Update()
    {
        _movement.Gravity();
        _movement.CheckGrounded();
        _movement.MoveAndRotation(Managers.Input.Move, _mainCamera.transform.eulerAngles.y);
    }
}
