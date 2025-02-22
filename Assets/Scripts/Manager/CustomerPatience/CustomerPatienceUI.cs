using UnityEngine;
using UnityEngine.UI;

public class CustomerPatienceUI : MonoBehaviour
{
    [SerializeField] private Image m_progressBar;

    public void SetProgress(float progressRatio)
    {
        m_progressBar.fillAmount = progressRatio;
    }
    
}
