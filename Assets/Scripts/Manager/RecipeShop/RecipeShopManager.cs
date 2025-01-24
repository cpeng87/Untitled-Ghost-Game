using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manager.RecipeShop
{
    public class RecipeShopManager : MonoBehaviour
    {
        
        public delegate void OnRecipeClick(Recipe recipeData);
        public static OnRecipeClick onRecipeClick;
        public static RecipeShopManager Instance { get; private set; }
    
        public GameObject recipePrefab;
    
        //Recipes to populate the shop with
        [SerializeField] private List<Recipe> recipes;
    
        //UI references
        [SerializeField] private GameObject recipeShop;
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
            //Bind appropriate function to delegate
            onRecipeClick = HandleRecipeClick;
            
            //Set shop data including recipes and currency
            PopulateShop();
        
            //Initially hide and show relevant gameobjects
            recipeShopOpenButton.gameObject.SetActive(true);
            recipeShopReturnButton.gameObject.SetActive(false);
            recipeShop.gameObject.SetActive(false);
        
            //Binds buttons to their respective functions
            recipeShopOpenButton.onClick.AddListener(ShowRecipeShop);
            recipeShopReturnButton.onClick.AddListener(HideRecipeShop);
        }

        private void PopulateShop()
        {
            // Update currency text
            SetCurrencyData();
 
            //Iterate through all recipes
            foreach (Recipe recipe in recipes)
            {
                GameObject recipeGameObject = Instantiate(recipePrefab, recipeShop.transform);
                //Get RecipeClickHandler
                RecipeClickHandler recipeClickHandler = recipeGameObject.GetComponent<RecipeClickHandler>();
                //Store recipe on script
                recipeClickHandler.SetRecipe(recipe);
                
            }
            
            //Instantiate and populate data in recipe prefab
        
            //While populating, get RecipeEventHandler.cs and add HandleRecipeClick listener 
        
            //Add to shop content
        }

        //Checks if the recipe needs to be bought or sold based on its status
        private void HandleRecipeClick(Recipe recipeData)
        {
        
        }

        private void SetCurrencyData()
        {
            currencyText.text = "Currency: $" + GameManager.Instance.GetCurrency().ToString();
        }
    
        private void ShowRecipeShop()
        {
            recipeShop.SetActive(true);
        
            //Show and hide relevant buttons
            recipeShopOpenButton.gameObject.SetActive(false);
            recipeShopReturnButton.gameObject.SetActive(true);
        }

        private void HideRecipeShop()
        {
            recipeShop.SetActive(false);
        
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
