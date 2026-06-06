using UnityEngine;
using UnityEngine.EventSystems;

public class Knife : DraggableObject
{
    private Vector3 originPos;
    public override void Start()
    {
        originPos = this.gameObject.transform.position;
        base.Start();
    }
    public override void OnPointerDown(PointerEventData eventData) {

        transform.rotation = Quaternion.Euler(90f, 90f, 90f);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        transform.rotation = Quaternion.Euler(0f, 90f, 90f);
        transform.position = originPos;
        base.OnPointerUp(eventData);

    }
}
