using System.Collections.Generic;
using UnityEngine;

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
        public GameObject patiencePrefab;
        public float defaultPatienceTime = 2.0f;
        
        private List<GhostPatienceData> m_ghostsPatienceDataList;
        //Maps a ghost game object's instance ID to an index in the ghostPatienceList
        private Dictionary<int, int> m_ghostIDPatienceIndexMap;
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
        }
        public void StartGhostPatienceTimer(GameObject ghost)
        {
            Debug.Log("GhostPatience started for ghost: " + ghost.name);
            //Attach the ghost's patience UI to the patience UI panel
            GameObject ghostPatienceUIPrefab = Instantiate(patiencePrefab, customerPatienceUIPanel.transform);
            
            GhostPatienceData ghostPatienceData = new GhostPatienceData
            {
                totalPatienceTime = defaultPatienceTime,
                timeRemaining = defaultPatienceTime,
                ghostGameobject = ghost,
                ghostUIGameobject = ghostPatienceUIPrefab,
                customerPatienceUIScript = ghostPatienceUIPrefab.GetComponent<CustomerPatienceUI>()
            };
            
            m_ghostsPatienceDataList.Add(ghostPatienceData);
            //Map instance ID to ghostPatienceList index
            m_ghostIDPatienceIndexMap.Add(ghost.GetInstanceID(), m_ghostsPatienceDataList.Count - 1);
        }
        
        public void StopGhostPatienceTimer(int instanceId, bool bOrderComplete = false)
        {
            if(!m_ghostIDPatienceIndexMap.ContainsKey(instanceId))
            {
                Debug.LogError("Error, CustomerPatienceManager: instance ID was not found in the map when stopping ghost's patience");
                return;
            }
            
            Debug.Log("GhostPatience stopped for ghost: " + instanceId);
            
            //Get ghost patience index from instance id
            int patienceIndex = m_ghostIDPatienceIndexMap[instanceId] ;
            
            //TODO: Change this to an animation (through UI) | For now just destroy the UI gameobject
            Destroy(m_ghostsPatienceDataList[patienceIndex].ghostUIGameobject);
            
            //TODO: Convey to the UI to hide progress bar
            
            //TODO: Show different animation if order was completed 
            if (bOrderComplete)
            {
                
            }
            
            //Remove the ghost patience instance
            m_ghostsPatienceDataList.RemoveAt(patienceIndex);
            //Remove the ghost from the map
            m_ghostIDPatienceIndexMap.Remove(instanceId);
        }

        // Update is called once per frame
        void Update()
        {
            
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