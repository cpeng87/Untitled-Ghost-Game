using UnityEngine;
using UnityEngine.UI;

public enum CakeState
{
    LeftMix = 0,
    RightMix = 1,
    UpMix = 2,
    DownMix = 3,
    Oven = 4
}

public class Rotate : MonoBehaviour
{
    public KeyCode leftArrow;
    public KeyCode rightArrow;
    public KeyCode upArrow;
    public KeyCode downArrow;
    public int numFlourState = 2;
    [SerializeField] private CakeState cakeState = CakeState.LeftMix;
    [SerializeField] private float progress;
    [SerializeField] private float neededProgress;
    private float timer;
    [SerializeField] private float swapDirectionTime;

    [SerializeField] private float rotateSpeed = 50f;
    [SerializeField] private float progressIncrement;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject goalCircle;
    [SerializeField] private GameObject movingCircle;

    private void Start()
    {
        goalCircle.SetActive(false);
        movingCircle.SetActive(false);
    }

    void Update()
    {
        if (cakeState == CakeState.LeftMix || cakeState == CakeState.RightMix || cakeState == CakeState.UpMix || cakeState == CakeState.DownMix)
        {
            timer += Time.deltaTime;
            if (timer > swapDirectionTime)
            {
                float val = Random.value;
                if (val < 0.25f)
                {
                    cakeState = CakeState.LeftMix;
                    arrow.transform.eulerAngles = new Vector3(0f, 0f, 180f);
                }
                else if (val < 0.5f)
                {
                    cakeState = CakeState.UpMix;
                    arrow.transform.eulerAngles = new Vector3(0f, 0f, 90f);
                }
                else if (val < 0.75f)
                {
                    cakeState = CakeState.DownMix;
                    arrow.transform.eulerAngles = new Vector3(0f, 0f, 270f);
                }
                else
                {
                    cakeState = CakeState.RightMix;
                    arrow.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                }
                timer = 0;
            }
            if (Input.GetKey(leftArrow))
            {
                if (cakeState == CakeState.LeftMix)
                {
                    progress += progressIncrement;
                    transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
                }
            }
            else if (Input.GetKey(rightArrow))
            {
                if (cakeState == CakeState.RightMix)
                {
                    progress += progressIncrement;
                    transform.Rotate(Vector3.forward * -1 * rotateSpeed * Time.deltaTime);
                }
            }
            else if (Input.GetKey(upArrow))
            {
                if (cakeState == CakeState.UpMix)
                {
                    progress += progressIncrement;
                    transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
                }
            }
            else if (Input.GetKey(downArrow))
            {
                if (cakeState == CakeState.DownMix)
                {
                    progress += progressIncrement;
                    transform.Rotate(Vector3.forward * -1 * rotateSpeed * Time.deltaTime);
                }
            }
            if (progress > neededProgress)
            {
                cakeState = CakeState.Oven;
                StartCakeOven();
            }

        }
    }

    private void StartCakeOven()
    {
        goalCircle.SetActive(true);
        movingCircle.SetActive(true); 
    }

}
