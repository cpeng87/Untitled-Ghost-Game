using UnityEngine;
using UnityEngine.EventSystems;

public class Vegetable : DraggableObject
{

    [SerializeField] private bool isDraggable;
    [SerializeField] private bool isChopped;
    [SerializeField] private Vector3 targetPosition;

    [SerializeField] private GameObject[] parts;
    [SerializeField] private int chops;
    [SerializeField] private bool canChop;
    public override void Start() {
        isDraggable = true;
        isChopped = false;
        chops = 0;
        canChop = true;
        base.Start();
    }

    private void FixedUpdate()
    {
        if (!isDraggable)
        {
            transform.position = targetPosition;
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (isChopped) {
            return;
        }
        //lock the object in place
        if (other.gameObject.name == "choppingTarget") {
            isDraggable = false;
            targetPosition = new Vector3(-1.3409998416900635f, -1.6769999265670777f, -2.5799999237060549f);
        }
    }

    public void OnCollisionEnter(Collision other) {
        if (other.gameObject.name == "knife" && canChop) {
            canChop = false;
            parts[chops].transform.position = targetPosition + new Vector3(-(chops % 2 == 0 ? 1 : -1), 0f, 0f);
            parts[chops].GetComponent<DraggableObject>().enabled = true;
        }
    }

    public void OnCollisionExit(Collision other) {
        if (other.gameObject.name == "knife" && !canChop) {
            chops++;
            if (chops == 2) {
                isChopped = true;
                GetComponent<BoxCollider>().enabled = false;
                parts[chops].GetComponent<DraggableObject>().enabled = true;
            } else {
                canChop = true;
            }
        }
    }


    public override void OnPointerDown(PointerEventData eventData) {
        if (isDraggable) {
            base.OnPointerDown(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData) {
        if (isDraggable) {
            base.OnDrag(eventData);
        }
    }
}
