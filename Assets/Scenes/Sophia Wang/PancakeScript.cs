using UnityEngine;

public class PancakeScript : MonoBehaviour
{
    private Vector3 mousePosition;
    private bool moved = false;
    private bool clicked = false;
    [SerializeField] GameObject pancakePrefab;
    private Vector3 startPosition;
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
            Instantiate(pancakePrefab, startPosition, Quaternion.identity);
        }
    }
}
