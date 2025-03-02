using UnityEngine;
using UnityEngine.EventSystems;

public class Knife : DraggableObject
{

    public override void OnPointerDown(PointerEventData eventData) {
        transform.rotation = Quaternion.Euler(90f, 90f, 90f);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData) {
        transform.rotation = Quaternion.Euler(0f, 90f, 90f);
        transform.position = new Vector3(-4.65f, -1.983f, -3.4f);
        base.OnPointerUp(eventData);
    }
}
