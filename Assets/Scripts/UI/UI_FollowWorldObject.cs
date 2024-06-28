using UnityEngine;

public class UI_FollowWorldObject : MonoBehaviour
{
    public Transform Target
    {
        get => _target;
        set
        {
            _target = value;
            gameObject.SetActive(_target != null);
        }
    }

    [field: SerializeField]
    public Vector3 Offset { get; set; }

    private Transform _target;
    private Camera _mainCamera;
    private RectTransform _rt;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _rt = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (_target == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _rt.position = _mainCamera.WorldToScreenPoint(_target.position + Offset);
        }
    }

    public void SetTargetAndOffset(Transform target, Vector3 offset)
    {
        Target = target;
        Offset = offset;
    }
}
