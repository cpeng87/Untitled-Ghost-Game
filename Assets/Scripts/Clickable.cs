using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour
{
    protected virtual void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject() && Time.timeScale == 0)
                return; // pointer is over UI (e.g. pause menu) — skip the world raycast
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    OnClicked();
                }
            }
        }
    }

    protected virtual void OnClicked() {}
}