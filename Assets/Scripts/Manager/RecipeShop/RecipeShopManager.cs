using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manager.RecipeShop
{
    public class RecipeShopManager : MonoBehaviour
    {
        
        public static RecipeShopManager Instance { get; private set; }
    
        public GameObject recipePrefab;
    
        //Recipes to populate the shop with
        [SerializeField] private List<Recipe> recipes;
        
        //UI references
        [SerializeField] private GameObject recipeShopUI;
        [SerializeField] private GameObject recipesContainer;
        [SerializeField] private TextMeshProUGUI currencyText;
        [SerializeField] private Button recipeShopOpenButton;
        [SerializeField] private Button recipeShopReturnButton;
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
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        { 
            //Set shop data including recipes and currency
            PopulateShop();
        
            //Initially hide and show relevant gameobjects
            recipeShopOpenButton.gameObject.SetActive(true);
            recipeShopReturnButton.gameObject.SetActive(false);
            recipeShopUI.gameObject.SetActive(false);
        
            //Binds buttons to their respective functions
            recipeShopOpenButton.onClick.AddListener(ShowRecipeShop);
            recipeShopReturnButton.onClick.AddListener(HideRecipeShop);
        }

        private void PopulateShop()
        {
            // Update currency text
            SetCurrencyData();
            
            Queue<Recipe> soldRecipesQueue = new Queue<Recipe>();
 
            //Iterate through all recipes
            foreach (Recipe recipe in recipes)
            {
                //Store sold recipes in a queue to display in the end
                if (recipe.isBought)
                {
                    soldRecipesQueue.Enqueue(recipe);
                    continue;
                }
                //Instantiate recipe UI and populate data in recipe prefab
                GameObject recipeGameObject = Instantiate(recipePrefab, recipesContainer.transform);
                RecipeClickHandler recipeClickHandler = recipeGameObject.GetComponent<RecipeClickHandler>();
                recipeClickHandler.SetRecipe(recipe);
                
            }

            //TODO: Ensure sold items are displayed last (either use temporary stack or list )
            //Display sold items towards the end
            while (soldRecipesQueue.Count > 0)
            {
                Recipe recipe = soldRecipesQueue.Dequeue();
                
                //Instantiate recipe UI and populate data in recipe prefab
                GameObject recipeGameObject = Instantiate(recipePrefab, recipesContainer.transform);
                RecipeClickHandler recipeClickHandler = recipeGameObject.GetComponent<RecipeClickHandler>();
                recipeClickHandler.SetRecipe(recipe);
            }
            
        }

        //Checks if the recipe needs to be bought or sold based on its status
        public void HandleRecipeClick(Recipe recipeData, GameObject recipeShopObject)
        {
            //TODO: Handle recipe being bought or sold 
            
            //Attempt to buy the recipe
            if (!recipeData.isBought)
            {
                if (GameManager.Instance.GetCurrency() >= recipeData.buyPrice)
                {
                    Debug.Log($"You have enough to buy this recipe!({recipeData.name})");
                    //Buy the recipe
                    recipeData.isBought = true;
                    //Move the recipe towards the end (because sold)
                    recipeShopObject.transform.SetSiblingIndex(-1);
                }
                else
                {
                    Debug.Log($"You don't have enough to buy this recipe!({recipeData.name})");
                }
            }
            //TODO: Change behavior to allow selling of recipes
            else //Already bought, cannot be sold
            {
                Debug.Log($"You cannot sell this recipe currently!({recipeData.name})");
            }
            
        }

        private void SetCurrencyData()
        {
            currencyText.text = "Currency: $" + GameManager.Instance.GetCurrency().ToString();
        }
    
        private void ShowRecipeShop()
        {
            recipeShopUI.SetActive(true);
        
            //Show and hide relevant buttons
            recipeShopOpenButton.gameObject.SetActive(false);
            recipeShopReturnButton.gameObject.SetActive(true);
        }

        private void HideRecipeShop()
        {
            recipeShopUI.SetActive(false);
        
            //Show and hide relevant buttons
            recipeShopReturnButton.gameObject.SetActive(false);
            recipeShopOpenButton.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            //Unbind buttons before destruction
            recipeShopOpenButton.onClick.RemoveListener(ShowRecipeShop);
            recipeShopReturnButton.onClick.RemoveListener(HideRecipeShop);
        }
    }
}
