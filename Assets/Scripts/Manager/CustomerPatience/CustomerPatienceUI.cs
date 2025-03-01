using System;
using UnityEngine;
using UnityEngine.UI;


//This script lies on the CustomerPatienceUIPrefab, visually updating the progress bar as well as positioning it according to the ghost gameobject
public class CustomerPatienceUI : MonoBehaviour
{
    [SerializeField] private GameObject m_containerObject;
    [SerializeField] private Slider m_progressBarSlider;
    
    public Vector3 progressBarOffset = Vector3.zero;
    private bool bProgressStarted = false;
    private GameObject m_ghost;

    private void Awake()
    {
        //Initially hide it and show once UI position has been set (In Update)
        this.m_containerObject.SetActive(false);
    }

    public void StoreGhost(GameObject ghost)
    {
        this.m_ghost = ghost;
    }
    
    public void SetProgress(float progressRatio)
    {
        m_progressBarSlider.value = progressRatio;
    }

    public void StopPatienceAnimation(bool bOrderComplete)
    {
        if (bOrderComplete)
        {
            //Show success animation (eg. cheerful animation and sound)
        }
        else 
        {
            //Show failure animation
        }
        
        //TODO: Remove this and use bOrderComplete for different animations || For now, just destroy this gameobject
        Destroy(gameObject);
    }

    private void Update()
    {
        //Set UI position to be relative to the ghost gameobject
        Vector3 ghostWorldPosition = m_ghost.transform.position;
        
        //Convert world position to 3D space
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(ghostWorldPosition);
        
        //Finally set position of UI
        this.transform.position = screenPosition + progressBarOffset;
        
        if (!bProgressStarted) //Show UI once position has been set initially  
        {
            bProgressStarted = true;
            this.m_containerObject.SetActive(true);
        }
    }
    
}
