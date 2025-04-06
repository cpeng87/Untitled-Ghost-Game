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
        [SerializeField] private GameObject soldText;
        [SerializeField] private Image foodImage;
        
        private Recipe currentRecipe;

        public void SetRecipe(Recipe recipe)
        {
            currentRecipe = recipe;
            
            //Set recipe visual information
            nameText.text = recipe.name;
            costText.text = "$ "+recipe.buyPrice.ToString();
            soldText.gameObject.SetActive(RecipeShopManager.Instance.IsRecipeBought(recipe));
            // soldText.gameObject.SetActive(recipe.isBought);

            foodImage.sprite = recipe.foodImage;
        }

        public void RecipeClicked()
        {
            // Debug.Log("Recipe clicked: " + currentRecipe.name);
            RecipeShopManager.Instance.HandleRecipeClick(currentRecipe, this);
        }

        public void SetRecipeSold()
        {
           soldText.gameObject.SetActive(true);
        }
    
    }
}
