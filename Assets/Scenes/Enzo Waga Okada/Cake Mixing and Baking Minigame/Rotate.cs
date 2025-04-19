using UnityEngine;

public class Rotate : MonoBehaviour
{
    public KeyCode leftArrow;
    public KeyCode rightArrow;
    public bool started = false;
    [SerializeField] private float rotateSpeed = 50f;
    void Start() {

    }

    void Update() {
        if (started == true) {
            if (transform.localScale.x > 5.8f && transform.localScale.y > 5.8f)
                transform.localScale += new Vector3(-0.4f, -0.4f, 0) * Time.deltaTime;
        }
        if (transform.localScale.x < 7f && transform.localScale.y < 7f) {
            if (Input.GetKey(leftArrow)) {
                started = true;
                transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
                transform.localScale += new Vector3(1.2f, 1.2f, 0) * Time.deltaTime;
            }
            if (Input.GetKey(rightArrow)) {
                started = true;
                transform.Rotate(Vector3.forward * -1 * rotateSpeed * Time.deltaTime);
                transform.localScale += new Vector3(1.2f, 1.2f, 0) * Time.deltaTime;
            }
        }
    }

}
