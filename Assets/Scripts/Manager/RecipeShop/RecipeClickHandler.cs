using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manager.RecipeShop
{
    public class RecipeClickHandler : MonoBehaviour
    { 
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI costText;
        
        private Recipe currentRecipe;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        public void SetRecipe(Recipe recipe)
        {
            currentRecipe = recipe;
            
            //Set recipe visual information
            nameText.text = recipe.name;
            costText.text = "$ "+recipe.buyPrice.ToString();
            //TODO: Set image info
        }

        public void RecipeClicked()
        {
            Debug.Log("Recipe clicked: " + currentRecipe.name);
            RecipeShopManager.Instance.HandleRecipeClick(currentRecipe, gameObject);
        }
    
    }
}
