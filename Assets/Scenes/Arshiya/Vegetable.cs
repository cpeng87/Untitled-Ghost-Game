using UnityEngine;
using UnityEngine.EventSystems;

public class Vegetable : Clickable
{
    // [SerializeField] private bool isDraggable;
    private Vector3 targetPosition = new Vector3(-1.5f, -1.7f, -3f);

    [SerializeField] private GameObject[] parts;
    [SerializeField] private int chops;
    [SerializeField] private bool canChop;
    [SerializeField] private float inc;
    [SerializeField] private float distChop;
    private SoupManager soupManager;

    private float originalY;
    private float originalZ;
    private bool isOnBoard;
    public void Start()
    {
        // isDraggable = true;
        chops = 0;
        canChop = true;
        // originalY = transform.position.y;
        // originalZ = transform.position.z;
        soupManager = FindObjectOfType<SoupManager>();
        isOnBoard = false;
    }

    protected override void OnClicked()
    {
        MoveToBoard();
    }

    private void MoveToBoard()
    {
        if (soupManager.GetNumPartOnBoard() == 0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90, transform.eulerAngles.z);
            // targetPosition = new Vector3(-1.3409998416900635f, -1.6769999265670777f, -2.5799999237060549f);
            this.gameObject.transform.position = targetPosition;
            soupManager.AddNumPartOnBoard(3);
            isOnBoard = true;
        }
    }

    // private void FixedUpdate()
    // {
    //     // if (!isDraggable)
    //     // {
    //     //     transform.position = targetPosition;
    //     //     transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    //     // }
    //     // if (transform.position.y <= originalY)
    //     // {
    //     //     transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
    //     // }
    //     // if (transform.position.z != originalZ)
    //     // {
    //     //     transform.position = new Vector3(transform.position.x, transform.position.y, originalZ);
    //     // }
    // }

    // public void OnTriggerEnter(Collider other) {
    //     // if (isChopped) {
    //     //     return;
    //     // }
    //     //lock the object in place
    //     if (other.gameObject.name == "choppingTarget" && !isChopped) {
    //         isDraggable = false;
    //         targetPosition = new Vector3(-1.3409998416900635f, -1.6769999265670777f, -2.5799999237060549f);
    //     }
    // }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "knife" && canChop && isOnBoard)
        {
            canChop = false;
            if (chops == 1)
            {
                parts[0].transform.position = targetPosition + new Vector3(-distChop, 0f, 0f);
            }
            else
            {
                parts[2].transform.position = targetPosition + new Vector3(distChop, 0f, 0f);
            }
            parts[chops].GetComponent<VegetablePart>().enabled = true;
            parts[chops].GetComponent<BoxCollider>().enabled = true;
            AudioManager.Instance.PlaySound("Cutting");
            // soupManager.AddToProgress(inc);
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "knife" && !canChop && isOnBoard) {
            chops++;
            if (chops == 2)
            {
                GetComponent<BoxCollider>().enabled = false;
                parts[chops].GetComponent<BoxCollider>().enabled = true;
                parts[chops].GetComponent<VegetablePart>().enabled = true;
            }
            else
            {
                canChop = true;
            }
        }
    }
}
