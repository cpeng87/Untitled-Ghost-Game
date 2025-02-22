using System.Collections.Generic;
using UnityEngine;

namespace Manager.CustomerPatience
{
    public class GhostPatience
    {
        public float totalPatienceTime;
        public float timeRemaining;
        public GameObject ghostGameobject;
        public GameObject ghostUIGameobject;
        public CustomerPatienceUI customerPatienceUIScript;
    }
    public class CustomerPatienceManager : MonoBehaviour
    {

        public GameObject customerPatienceUIPanel;
        public GameObject patiencePrefab;
        private List<GhostPatience> ghostsPatienceList;

        public void StartGhostPatience(float totalPatienceTime, GameObject ghost)
        {
            
            //Attach the ghost's patience UI to the patience UI panel
            GameObject ghostPatienceUIPrefab = Instantiate(patiencePrefab, customerPatienceUIPanel.transform);
            
            GhostPatience ghostPatience = new GhostPatience
            {
                totalPatienceTime = totalPatienceTime,
                timeRemaining = totalPatienceTime,
                ghostGameobject = ghost,
                ghostUIGameobject = ghostPatienceUIPrefab,
                customerPatienceUIScript = ghostPatienceUIPrefab.GetComponent<CustomerPatienceUI>()
            };
            
            ghostsPatienceList.Add(ghostPatience);
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            
            //Iterate through all ghosts 
            for (int index = 0; index < ghostsPatienceList.Count; ++index)
            {
                GhostPatience ghost  = ghostsPatienceList[index];
                ghost.timeRemaining -= Time.deltaTime;
                
                //Set progress for UI
                ghost.customerPatienceUIScript.SetProgress(ghost.timeRemaining / ghost.totalPatienceTime);
                
                //Stop timer once time runs out
                if (ghost.timeRemaining <= 0.0f)
                    PatienceRanOut(index);
                
            }
        }

        private void PatienceRanOut(int index)
        {
            //For now just destroy the UI gameobject
            Destroy(ghostsPatienceList[index].ghostUIGameobject);
            
            //TODO: Convey to the UI to hide progress bar

            //Remove the ghost patience instance
            ghostsPatienceList.RemoveAt(index);
        }
    }
}