using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public Transform model;
    public float rotateSpeed = 32f;
    public float rotateLerp = 8;
    public float moveSpeed = 1f;
    public float moveLerp = 10f;
    public float zoomSpeed = 10f;
    public float zoomLerp = 4f;

    private Vector3 position, targetPosition;
    private Quaternion rotation, targetRotation;
    private float distance, targetDistance;
    private const float default_distance = 5f;
    private const float min_angle_y = -89f;
    private const float max_angle_y = 89f;


    // Use this for initialization
    void Start()
    {

        targetRotation = Quaternion.Euler(-215f, -419f, -180f);
        targetPosition = new Vector3(-12.42f,8.25f, -1.21f);
        targetDistance = default_distance;
    }

    // Update is called once per frame
    void Update()
    {
        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(dx) > 5f || Mathf.Abs(dy) > 5f)
        {
            return;
        }

        float d_target_distance = targetDistance;
        if (d_target_distance < 2f)
        {
            d_target_distance = 2f;
        }

        // LMC: translation
        if (Input.GetMouseButton(2))
        {
            dx *= moveSpeed * d_target_distance / default_distance;
            dy *= moveSpeed * d_target_distance / default_distance;
            targetPosition -= transform.up * dy + transform.right * dx;
        }

        // RMC: rotation
        if (Input.GetMouseButton(1))
        {
            dx *= rotateSpeed;
            dy *= rotateSpeed;
            if (Mathf.Abs(dx) > 0 || Mathf.Abs(dy) > 0)
            {
                // Get euler angles of camera
                Vector3 angles = transform.rotation.eulerAngles;

                angles.x = Mathf.Repeat(angles.x + 180f, 360f) - 180f;
                angles.y += dx;
                angles.x -= dy;
                angles.x = ClampAngle(angles.x, min_angle_y, max_angle_y);
                // calculate Rotation
                targetRotation.eulerAngles = new Vector3(angles.x, angles.y, 0);
                Vector3 temp_position =
                        Vector3.Lerp(targetPosition, model.position, Time.deltaTime * moveLerp);
                targetPosition = Vector3.Lerp(targetPosition, temp_position, Time.deltaTime * moveLerp);
            }
        }

        // scroll
        targetDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    private void FixedUpdate()
    {
        rotation = Quaternion.Slerp(rotation, targetRotation, Time.deltaTime * rotateLerp);
        position = Vector3.Lerp(position, targetPosition, Time.deltaTime * moveLerp);
        distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * zoomLerp);

        transform.rotation = rotation;
        transform.position = position - rotation * new Vector3(0, 0, distance);
    }
}