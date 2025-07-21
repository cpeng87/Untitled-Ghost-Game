using UnityEngine;

public class Speed : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float linearVelocity;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        rb.linearVelocity = new Vector2(-1 * linearVelocity, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= -5) {
            rb.linearVelocity = new Vector2(linearVelocity, 0);
        } else if (transform.position.x >= 5) {
            rb.linearVelocity = new Vector2(-1 * linearVelocity, 0);
        }
    }
}
