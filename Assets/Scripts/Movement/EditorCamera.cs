using UnityEngine;

public class EditorCamera : MonoBehaviour
{
    public static Camera MainCamera
    {
        get
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }
            return _mainCamera;
        }
    }

    [Range(0.01f, 250.0f)]
    public float LookSpeed = 2.0f;
    [Range(0.01f, 250.0f)]
    public float MoveSpeed = 0.35f;

    private float _rotationX;
    private float _rotationY;

    private static Camera _mainCamera;

    protected void Update()
    {
        if (!Input.GetMouseButton(1))
        {
            return;
        }

        _rotationX += Input.GetAxis("Mouse X") * LookSpeed;
        _rotationY += Input.GetAxis("Mouse Y") * LookSpeed;
        _rotationY = Mathf.Clamp(_rotationY, -90.0f, 90.0f);

        transform.localRotation = Quaternion.AngleAxis(_rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(_rotationY, Vector3.left);

        transform.position += transform.forward * MoveSpeed * Input.GetAxis("Vertical");
        transform.position += transform.right * MoveSpeed * Input.GetAxis("Horizontal");
    }
}
