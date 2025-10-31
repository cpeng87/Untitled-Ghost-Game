
using UnityEngine;

public class PancakeScript : MonoBehaviour
{
    private Vector3 mousePosition;
    private bool moved = false;
    private bool clicked = false;
    [SerializeField] GameObject pancakePrefab;
    private Vector3 startPosition;
    private bool hasBounced = false;
    private Rigidbody rb;
    private Collider col;
    private void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        if (rb != null)
        {
            rb.isKinematic = false;
        }
        if (col != null)
        {
            col.enabled = true;
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 getMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - getMousePos();

        if (rb != null)
        {
            rb.isKinematic = true;
        }
        if (col != null)
        {
            col.enabled = false;
        }
    }
    private void OnMouseDrag()
    {
        if (!moved)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
            clicked = true;
        }
    }
    private void OnMouseUp()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        if (col != null)
        {
            col.enabled = true;
        }



        if (clicked && !moved)
        {
            moved = true;
            GameObject pancake = Instantiate(pancakePrefab, startPosition, Quaternion.identity);
            pancake.name = "Pancake";

            Rigidbody newRb = pancake.GetComponent<Rigidbody>();
            Collider newCol = pancake.GetComponent<Collider>();
            if (newRb != null)
            {
                newRb.isKinematic = false;
            }
            if (newCol != null)
            {
                newCol.enabled = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasBounced)
        {
            return;
        }
        if (collision.gameObject.name == "Pancake" || collision.gameObject.name == "Plate")
        {
            AudioManager.Instance.PlaySound("Pancake");
            hasBounced = true;
        }
    }
}
