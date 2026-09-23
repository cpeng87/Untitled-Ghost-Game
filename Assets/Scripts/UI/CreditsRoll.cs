using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CreditsRoll : MonoBehaviour
{
    [SerializeField] private GameObject creditsNamePanel;
    [SerializeField] private float speed;
    [SerializeField] private List<GameObject> spinnyGhosts = new List<GameObject>();
    private float time;
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

        SpinnyGhostDown();
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
            ghost.transform.position -= new Vector3(0f, speed * Time.deltaTime, 0f);
            ghost.transform.Rotate(0f, speed * 100f * Time.deltaTime, 0f);
        }
    }
}
