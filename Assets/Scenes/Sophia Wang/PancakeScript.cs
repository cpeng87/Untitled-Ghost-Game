using UnityEngine;

public class PancakeScript : MonoBehaviour
{
    private Vector3 mousePosition;
    private bool moved = false;
    private bool clicked = false;
    [SerializeField] GameObject pancakePrefab;
    private Vector3 startPosition;
    private bool hasBounced = false;
    private void Start()
    {
        startPosition = transform.position;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 getMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - getMousePos();
    }
    private void OnMouseDrag()
    {
        if (moved == false)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
            clicked = true;
        }
    }
    private void OnMouseUp()
    {
        if (clicked)
        {
            moved = true;
            GameObject pancake = Instantiate(pancakePrefab, startPosition, Quaternion.identity);
            pancake.name = "Pancake";
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
