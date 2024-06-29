using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("[Rotation]")]
    [SerializeField]
    private Transform _cinemachineCameraTarget;

    [SerializeField]
    private float _sensitivity;

    [SerializeField]
    private float _topClamp;

    [SerializeField]
    private float _bottomClamp;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private readonly float _threshold = 0.01f;

    private void Start()
    {
        _cinemachineTargetPitch = _cinemachineCameraTarget.rotation.eulerAngles.x;
        _cinemachineTargetYaw = _cinemachineCameraTarget.rotation.eulerAngles.y;
    }

    public void Rotate()
    {
        var look = Managers.Input.Look;
        if (look.sqrMagnitude >= _threshold)
        {
            _cinemachineTargetYaw += look.x * _sensitivity;
            _cinemachineTargetPitch += look.y * _sensitivity;
        }

        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineCameraTarget.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0f);
    }

    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle > 180f)
        {
            return lfAngle - 360f;
        }

        if (lfAngle < -180f)
        {
            return lfAngle + 360f;
        }

        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
