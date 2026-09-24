using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CreditsRoll : MonoBehaviour
{
    [SerializeField] private GameObject creditsNamePanel;
    [SerializeField] private float speed;
    [SerializeField] private List<GameObject> spinnyGhosts = new List<GameObject>();
    private float time;

    [SerializeField] private GameObject lawyer;
    [SerializeField] private GameObject dog;
    bool lawyerHasFacedPlayer = false;

    [SerializeField] private GameObject weeb;
    [SerializeField] private GameObject officeWorker;
    bool weebHasFacedPlayer = false;

    [SerializeField] private GameObject musician;
    [SerializeField] private GameObject jock;
    [SerializeField] private GameObject soldier;
    [SerializeField] private GameObject doctor;
    bool hasPeeked = false;


    //event flags
    private bool spinnyDone;
    private bool lawyerDogDone;
    private bool weebOfficeDone;
    private bool peekDone;

    // Update is called once per frame

    public void Start()
    {
        AudioManager.Instance.PlaySong("Title");
    }

    void Update()
    {
        time += Time.deltaTime;
        creditsNamePanel.transform.position += new Vector3(0, speed * Time.deltaTime, 0);

        if (creditsNamePanel.transform.position.y >= 67f)
        {
            //transition to title screen
            MoveToTitle();
        }

        if (!lawyerDogDone)
        {
            LawyerAndDog();
        }
        else if (!weebOfficeDone)
        {
            WeebAndOffice();
        }
        else if (!peekDone)
        {
            Peeker();
        }
        else if (!spinnyDone)
        {
            SpinnyGhostDown();
        }
    }

    //top at around 4790 y -> transition to title screen or do a reaps dance

    private void MoveToTitle()
    {   
        //taken from the pause manager but like removed like most the code
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Reset();
        }
        SceneManager.LoadScene("TitleScene");
    }

    private void SpinnyGhostDown()
    {
        foreach (GameObject ghost in spinnyGhosts)
        {
            ghost.transform.position -= new Vector3(0f, speed * 0.8f * Time.deltaTime, 0f);
            ghost.transform.Rotate(0f, speed * 100f * Time.deltaTime, 0f);
            if (ghost.transform.position.y < -21)
            {
                spinnyDone = true;
            }
        }
    }

    //holy hardcode
    private void LawyerAndDog()
    {
        if (lawyer.transform.position.x > 5.73f && lawyerHasFacedPlayer == false) 
        {
            lawyer.transform.position -= new Vector3(speed * 2f * Time.deltaTime, 0, 0);
        }
        else if (dog.transform.position.x < 2.77f)
        {
            dog.transform.position += new Vector3(4f * speed * Time.deltaTime, 0, 0);
        }
        else if (Quaternion.Angle(lawyer.transform.rotation, Quaternion.Euler(0, 90, 0)) > 1f && lawyerHasFacedPlayer == false)
        {
            lawyer.transform.rotation = Quaternion.Lerp(lawyer.transform.rotation, Quaternion.Euler(0, 90, 0), speed * 2f * Time.deltaTime);
            dog.transform.rotation = Quaternion.Lerp(dog.transform.rotation, Quaternion.Euler(0, 90, 0), speed * 2f * Time.deltaTime);
        }
        else if ((Quaternion.Angle(lawyer.transform.rotation, Quaternion.Euler(0, 0, 0)) > 1f))
        {
            lawyerHasFacedPlayer = true;
            lawyer.transform.rotation = Quaternion.Lerp(lawyer.transform.rotation, Quaternion.Euler(0, 0, 0), speed * 2f * Time.deltaTime);
            dog.transform.rotation = Quaternion.Lerp(dog.transform.rotation, Quaternion.Euler(0, 0, 0), speed * 2f * Time.deltaTime);
        }
        else
        {
            if (dog.transform.position.x >= 11.6f)
            {
                lawyerDogDone = true;
            }
            else
            {
                lawyer.transform.position += new Vector3(2f * speed * Time.deltaTime, 0, 0);
                dog.transform.position += new Vector3(2f * speed * Time.deltaTime, 0, 0);
            }
        }
    }

    private void WeebAndOffice()
    {
        if (officeWorker.transform.position.x <= -4.44f)
        {
            weeb.transform.position += new Vector3(speed * 2f * Time.deltaTime, 0, 0);
            officeWorker.transform.position += new Vector3(speed * 2f * Time.deltaTime, 0, 0);
        }
        else if (Quaternion.Angle(weeb.transform.rotation, Quaternion.Euler(0, 90, 0)) > 0.1f && weebHasFacedPlayer == false)
        {
            weeb.transform.rotation = Quaternion.Lerp(weeb.transform.rotation, Quaternion.Euler(0, 90, 0), speed * 2f * Time.deltaTime);
            officeWorker.transform.rotation = Quaternion.Lerp(officeWorker.transform.rotation, Quaternion.Euler(0, 90, 0), speed * 2f * Time.deltaTime);
        }
        else if (Quaternion.Angle(weeb.transform.rotation, Quaternion.Euler(0, 0, 0)) > 1f)
        {
            weebHasFacedPlayer = true;
            weeb.transform.rotation = Quaternion.Lerp(weeb.transform.rotation, Quaternion.Euler(0, 0, 0), speed * 2f * Time.deltaTime);
            officeWorker.transform.rotation = Quaternion.Lerp(officeWorker.transform.rotation, Quaternion.Euler(0, 0, 0), speed * 2f * Time.deltaTime);
        }
        else
        {
            weeb.transform.position += new Vector3(speed * 2f * Time.deltaTime, 0, 0);
            officeWorker.transform.position += new Vector3(speed * 2f * Time.deltaTime, 0, 0);
        }

        if (weeb.transform.position.x >= 11.6f)
        {
            weebOfficeDone = true;
        }
    }

    private void Peeker()
    {
        if (!hasPeeked)
        {
            if (musician.transform.position.y < -4.16f)
            {
                musician.transform.position += new Vector3(0f, speed * Time.deltaTime * 2f, 0f);
            }
            else if (doctor.transform.position.y > 6.24f)
            {
                doctor.transform.position -= new Vector3(0f, speed * Time.deltaTime * 2f, 0f);
            }
            else if (jock.transform.position.x < -9.8f)
            {
                jock.transform.position += new Vector3(speed * Time.deltaTime * 2f, 0f, 0f);
            }
            else if (soldier.transform.position.x > 9.6f)
            {
                soldier.transform.position -= new Vector3(speed * Time.deltaTime * 2f, 0f, 0f);
            }
            else
            {
                hasPeeked = true;
            }
        }
        else
        {
            if (musician.transform.position.y > -6.67f)
            {
                musician.transform.position -= new Vector3(0f, speed * Time.deltaTime * 2f, 0f);
            }
            else if (doctor.transform.position.y < 7.81f)
            {
                doctor.transform.position += new Vector3(0f, speed * Time.deltaTime * 2f, 0f);
            }
            else if (jock.transform.position.x > -12f)
            {
                jock.transform.position -= new Vector3(speed * Time.deltaTime * 2f, 0f, 0f);
            }
            else if (soldier.transform.position.x < 11.67f)
            {
                soldier.transform.position += new Vector3(speed * Time.deltaTime * 2f, 0f, 0f);
            }
            else
            {
                peekDone = true;
            }
        }
    }
}
