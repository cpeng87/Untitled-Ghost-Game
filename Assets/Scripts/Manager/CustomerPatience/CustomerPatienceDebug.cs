using Manager.CustomerPatience;
using UnityEngine;

public class CustomerPatienceDebug : MonoBehaviour
{
    private int patienceMode = 0;

    public void TogglePatienceMode(GameObject ghost)
    {

        if (patienceMode == 0)
        {
            CustomerPatienceManager.Instance.StartGhostPatienceTimer(ghost);
        } else if (patienceMode == 1) //Stop patience (order incomplete)
        {
            CustomerPatienceManager.Instance.StopGhostPatienceTimer(ghost.GetInstanceID());
        }
        else if (patienceMode == 2)
        {
            CustomerPatienceManager.Instance.StartGhostPatienceTimer(ghost);
        }
        else  // Order complete
        {
            CustomerPatienceManager.Instance.StopGhostPatienceTimer(ghost.GetInstanceID(), true);
        }

        patienceMode = (patienceMode + 1) % 4;

    }
    
    
}
