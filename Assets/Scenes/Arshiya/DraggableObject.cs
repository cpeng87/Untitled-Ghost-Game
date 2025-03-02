using UnityEngine;
using UnityEngine.EventSystems;

//this script handles the "drag and drop" logic for the knife and vegetables
public class DraggableObject : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        mainCamera = Camera.main;
    }

    public virtual void OnPointerDown(PointerEventData eventData) {
        isDragging = true;
        Vector3 worldPos = transform.position;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

        offset = worldPos - mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, screenPos.z));
    }
    
    public virtual void OnDrag(PointerEventData eventData) {
        if (isDragging) {
            Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y, mainCamera.WorldToScreenPoint(transform.position).z);
            transform.position = mainCamera.ScreenToWorldPoint(screenPos) + offset;
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData){
        isDragging = false;
    }
}
