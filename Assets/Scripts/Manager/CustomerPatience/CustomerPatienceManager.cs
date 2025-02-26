using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager.CustomerPatience
{
    public class GhostPatienceData
    {
        public float totalPatienceTime;
        public float timeRemaining;
        public GameObject ghostGameobject;
        public GameObject ghostUIGameobject;
        public CustomerPatienceUI customerPatienceUIScript;
    }
    public class CustomerPatienceManager : MonoBehaviour
    {
        public static CustomerPatienceManager Instance { get; private set; }

        public GameObject customerPatienceUIPanel;
        public GameObject patienceUIPrefab;
        public float defaultPatienceTime = 6.0f;
        
        private List<GhostPatienceData> m_ghostsPatienceDataList = new List<GhostPatienceData>();
        //Maps a ghost game object's instance ID to an index in the ghostPatienceList
        private Dictionary<int, int> m_ghostIDPatienceIndexMap = new Dictionary<int, int>();
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            if (customerPatienceUIPanel == null)
            {
                Debug.LogWarning("Warning | CustomerPatienceManager: CustomerPatienceUIPanel is not assigned");
            }
            
            if (patienceUIPrefab == null)
            {
                Debug.LogWarning("Warning | CustomerPatienceManager: patienceUIPrefab is not assigned");
            }
        }
        public void StartGhostPatienceTimer(GameObject ghost)
        {
            // Debug.Log("GhostPatience started for ghost: " + ghost.name);

            if (!patienceUIPrefab)
            {
                // Debug.LogWarning("Warning | CustomerPatienceManager: patienceUIPrefab is not assigned when starting ghost patience timer");
                return;
            }
            
            if (!customerPatienceUIPanel)
            {
                // Debug.LogWarning("Warning | CustomerPatienceManager: CustomerPatienceUIPanel is not assigned when starting ghost patience timer");
                return;
            }
            
            if(m_ghostIDPatienceIndexMap.ContainsKey(ghost.GetInstanceID()))
            {
                Debug.LogWarning("Warning | CustomerPatienceManager: This ghost's patience timer has already been started ");
                return;
            }
            
            //Attach the ghost's patience UI to the patience UI panel
            GameObject ghostPatienceUIPrefab = Instantiate(patienceUIPrefab, customerPatienceUIPanel.transform);
            
            //Get patience UI script and store a reference of the ghost gameobject
            CustomerPatienceUI patienceUIScript = ghostPatienceUIPrefab.GetComponent<CustomerPatienceUI>();
            patienceUIScript.StoreGhost(ghost);
            
            GhostPatienceData ghostPatienceData = new GhostPatienceData
            {
                totalPatienceTime = defaultPatienceTime,
                timeRemaining = defaultPatienceTime,
                ghostGameobject = ghost,
                ghostUIGameobject = ghostPatienceUIPrefab,
                customerPatienceUIScript = patienceUIScript
            };

            m_ghostsPatienceDataList.Add(ghostPatienceData);
            //Map instance ID to ghostPatienceList index
            m_ghostIDPatienceIndexMap.Add(ghost.GetInstanceID(), m_ghostsPatienceDataList.Count - 1);
        }
        
        public void StopGhostPatienceTimer(int instanceId, bool bOrderComplete = false)
        {
            if(!m_ghostIDPatienceIndexMap.ContainsKey(instanceId))
            {
                Debug.LogWarning("Warning | CustomerPatienceManager: This ghost doesn't have any ongoing patience timer ");
                return;
            }
            
            // Debug.Log("GhostPatience stopped for ghost: " + instanceId);
            
            //Get ghost patience index from instance id
            int patienceIndex = m_ghostIDPatienceIndexMap[instanceId] ;
            
            //Play stopping patience animation based on whether the order was completed or not 
            m_ghostsPatienceDataList[patienceIndex].customerPatienceUIScript.StopPatienceAnimation(bOrderComplete);
            
            //Remove the ghost patience instance
            m_ghostsPatienceDataList.RemoveAt(patienceIndex);
            //Remove the ghost from the map
            m_ghostIDPatienceIndexMap.Remove(instanceId);
            
            //Update all other patience index map entries to reflect their new indices
            m_ghostIDPatienceIndexMap.Clear();
            for (int index = 0; index < m_ghostsPatienceDataList.Count; ++index)
            {
                GhostPatienceData ghostPatienceData = m_ghostsPatienceDataList[index];
                
                m_ghostIDPatienceIndexMap.Add(ghostPatienceData.ghostGameobject.GetInstanceID(), index);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (m_ghostsPatienceDataList.Count == 0)
                return;
            
            //Iterate through all ghosts 
            for (int index = 0; index < m_ghostsPatienceDataList.Count; ++index)
            {
                GhostPatienceData ghost  = m_ghostsPatienceDataList[index];
                ghost.timeRemaining -= Time.deltaTime;
                
                //Set progress for UI
                ghost.customerPatienceUIScript.SetProgress(ghost.timeRemaining / ghost.totalPatienceTime);
                
                //Stop timer once time runs out
                if (ghost.timeRemaining <= 0.0f)
                    PatienceRanOut(ghost.ghostGameobject.GetInstanceID());
                
            }
        }

        private void PatienceRanOut(int instanceId)
        {
            StopGhostPatienceTimer(instanceId);
            
            //TODO: Show extra UI if patience runs out
        }
    }
}