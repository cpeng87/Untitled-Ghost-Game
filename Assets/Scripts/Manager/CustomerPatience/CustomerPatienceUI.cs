using System;
using UnityEngine;
using UnityEngine.UI;

public class CustomerPatienceUI : MonoBehaviour
{
    [SerializeField] private Image m_progressBar;

    private GameObject m_ghost;

    public void StoreGhost(GameObject ghost)
    {
        this.m_ghost = ghost;
    }
    
    public void SetProgress(float progressRatio)
    {
        m_progressBar.fillAmount = progressRatio;
    }

    private void Update()
    {
        //Set UI position to be relative to the ghost gameobject
        Vector3 ghostWorldPosition = m_ghost.transform.position;
        
        //Convert world position to 3D space
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(ghostWorldPosition);
        
        //TODO: Add an offset to the position
        this.transform.position = screenPosition;
    }
}
