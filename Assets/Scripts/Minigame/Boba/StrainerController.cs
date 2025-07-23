using UnityEngine;

public class StrainerController : MonoBehaviour
{
    Camera cam;
    Vector3 camLeftBound;
    Vector3 camRightBound;
    [SerializeField] float speed = 5f;
    [SerializeField] float objectHalfWidth = 1f;
    [SerializeField] private Rigidbody rigidbody;

    void Start()
    {
        cam = Camera.main;
        camLeftBound = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        camRightBound = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));
    }

    void FixedUpdate()
    {
        float input = Input.GetAxis("Horizontal");
        if (input != 0)
        {
            Vector3 move = new Vector3(input * speed * Time.fixedDeltaTime, 0f, 0f);
            Vector3 targetPos = rigidbody.position + move;

            // Clamp the x-position
            float minX = camLeftBound.x + objectHalfWidth;
            float maxX = camRightBound.x - objectHalfWidth;
            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);

            rigidbody.MovePosition(targetPos);
        }
    }
}