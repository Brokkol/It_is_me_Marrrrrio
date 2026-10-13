using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // За кем следим (наш Player)
    public float smoothSpeed = 5f; // Плавность перемещения
    public Vector3 offset = new Vector3(0, 1, -10); // Смещение относительно игрока

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            desiredPosition.x = Mathf.Max(desiredPosition.x, transform.position.x);
            
            desiredPosition.y = transform.position.y;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}