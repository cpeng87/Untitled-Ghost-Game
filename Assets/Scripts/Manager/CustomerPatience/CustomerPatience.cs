using UnityEngine;
using UnityEngine.UI;

public class CustomerPatience : MonoBehaviour
{

    //The total time before a customer's patience runs out
    public float totalPatienceTime = 3.0f;
    
    private float m_curPatienceTimeRemaining = 0.0f;
    private bool bTimerStarted = false;
    
    //-- USER INTERFACE COMPONENTS --
    [SerializeField] private GameObject m_customerPatienceProgressUI;
    //TODO: Convert to a SLIDER
    [SerializeField] private Image m_progressBar;
    private GameObject m_customerPatienceUIPanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //TODO: Change this perhaps to UIManager storing refernce of Patience UI Panel
        m_customerPatienceUIPanel = UIManager.Instance.gameObject ? UIManager.Instance.gameObject : GameObject.Find("CustomerPatienceUI");
        
        if(m_customerPatienceUIPanel == null)
            Debug.LogError("CustomerPatienceUIPanel is null on GameObject: " + this.gameObject.name);
    }

    // Update is called once per frame
    private void Update()
    {
        if (bTimerStarted)
        {
            m_curPatienceTimeRemaining -= Time.deltaTime;
            
            //Stop timer once time runs out
            if (m_curPatienceTimeRemaining <= 0.0f)
                PatienceRanOut();
        }
    }
    
    public void StartPatienceTimer()
    {
        bTimerStarted = true;
        m_curPatienceTimeRemaining = totalPatienceTime;
    }

    private void PatienceRanOut()
    {
        StopPatienceTimer();
        
        //Broadcast that the patience has run out
    }

    private void StopPatienceTimer()
    {
        bTimerStarted = false;
        m_curPatienceTimeRemaining = 0;
        
        //Convey to the UI to hide progress bar
    }
}
