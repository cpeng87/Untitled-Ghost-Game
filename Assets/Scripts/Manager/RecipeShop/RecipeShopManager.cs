using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Manager.RecipeShop
{
    public class RecipeShopManager : MonoBehaviour
    {
        
        public static RecipeShopManager Instance { get; private set; }
    
        public GameObject recipePrefab;

        [SerializeField] private bool shouldDebug = false; 
        //Recipes to populate the shop with
        [SerializeField] private List<Recipe> recipes;
        
        //UI references
        [Header("UI References")]
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
            // Update visual currency display
            UpdateCurrencyData();
            
            Queue<Recipe> soldRecipesQueue = new Queue<Recipe>();
 
            //Iterate through all recipes
            foreach (Recipe recipe in recipes)
            {
                //Store sold recipes in a queue to display towards the end
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
        public void HandleRecipeClick(Recipe recipeData, RecipeClickHandler recipeClickHandler)
        {
            //Attempt to buy the recipe
            if (!recipeData.isBought)
            {
                if (GameManager.Instance.GetCurrency() >= recipeData.buyPrice)
                {
                    if(shouldDebug) Debug.Log($"You have enough to buy the recipe : ({recipeData.name}). Bought for {recipeData.buyPrice}!");
                    
                    //Buy the recipe
                    recipeData.isBought = true;
                    //Move the recipe towards the end of the shop (because sold)
                    recipeClickHandler.gameObject.transform.SetSiblingIndex(-1);
                    recipeClickHandler.SetRecipeSold();
                    
                    //Update currency
                    GameManager.Instance.AddCurrency(-recipeData.buyPrice);
                }
                else
                {
                    if(shouldDebug) Debug.Log($"You don't have enough to buy this recipe!({recipeData.name})");
                }
            } //TODO: Work on selling logic
            else //Already bought, cannot be sold
            {
                if(shouldDebug) Debug.Log($"You cannot sell this recipe currently!({recipeData.name})");
            }
            
            //After buying or selling, update currency value
            UpdateCurrencyData();
            
        }

        private void UpdateCurrencyData()
        {
            if(shouldDebug) Debug.Log($"Updating currency data to: {GameManager.Instance.GetCurrency()}");

            currencyText.text = "Currency: $" + GameManager.Instance.GetCurrency().ToSafeString();
        }
    
        private void ShowRecipeShop()
        {
            // Update visual currency display
            UpdateCurrencyData();
            
            //Show the recipe shop UI
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
