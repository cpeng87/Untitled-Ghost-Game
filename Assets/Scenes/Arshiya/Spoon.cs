using UnityEngine;

public class Spoon : DraggableObject
{
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float yFixed;
    [SerializeField] private float zFixed;

    private enum StirState { Idle, WentLeft, WentRight }
    private StirState stirState = StirState.Idle;

    [SerializeField] private float inc;
    private SoupManager soupManager;

    public override void Start()
    {
        soupManager = FindObjectOfType<SoupManager>();
        base.Start();
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = GetMouseWorldPosition();
            float clampedX = Mathf.Clamp(mousePos.x, minX, maxX);
            transform.position = new Vector3(clampedX, yFixed, zFixed);

            HandleStirDetection(clampedX);
        }
    }

    private void HandleStirDetection(float xPos)
    {
        // Define thresholds for when we count a hit
        float threshold = 0.05f;

        if (Mathf.Abs(xPos - minX) < threshold && stirState == StirState.WentRight)
        {
            // Full cycle: left -> right -> left
            // Debug.Log("Full stir!");
            soupManager.AddToMixProgress(inc);
            stirState = StirState.Idle;
        }
        if (Mathf.Abs(xPos - maxX) < threshold && stirState == StirState.WentLeft)
        {
            soupManager.AddToMixProgress(inc);
            stirState = StirState.WentRight;
        }
        else if (Mathf.Abs(xPos - minX) < threshold && stirState == StirState.Idle)
        {
            soupManager.AddToMixProgress(inc);
            stirState = StirState.WentLeft;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Plane plane = new Plane(Vector3.up, new Vector3(0, yFixed, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return transform.position;
    }
}
