using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manager.RecipeShop
{
    public class RecipeShopManager : MonoBehaviour
    {

        public static RecipeShopManager Instance { get; private set; }

        [SerializeField] private Animator recipeNotifAnimator;

        [Header("Settings")]
        public GameObject recipePrefab;
        [SerializeField] private bool shouldDebug = false;
        //Recipes to populate the shop with
        [SerializeField] private List<Recipe> recipes;
        [SerializeField] private string recipePurchasePromptMessage = "Confirm purchase";
        [SerializeField] private string recipeSalePromptMessage = "Confirm sale";

        //UI references
        [Header("General UI References")]
        [SerializeField] private GameObject recipeShopUI;
        [SerializeField] private GameObject recipesContainer;
        [SerializeField] private TextMeshProUGUI currencyText;
        [SerializeField] private Button recipeShopOpenButton;
        [SerializeField] private Button recipeShopReturnButton;

        [Header("Transaction Prompt UI")]
        [SerializeField] private GameObject recipeTransactionConfirmationPromptUI;
        [SerializeField] private TextMeshProUGUI recipeTransactionMessagePromptText;

        private Tuple<Recipe, RecipeClickHandler> currentRecipe;
        public static List<Recipe> boughtRecipes = new List<Recipe>();
        private List<GameObject> currRecipeCards = new List<GameObject>();
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
            // PopulateShop();

            //Initially hide and show relevant gameobjects
            recipeShopOpenButton.gameObject.SetActive(true);
            recipeShopReturnButton.gameObject.SetActive(false);
            recipeShopUI.gameObject.SetActive(false);

            //Binds buttons to their respective functions
            recipeShopOpenButton.onClick.AddListener(ShowRecipeShop);
            recipeShopReturnButton.onClick.AddListener(HideRecipeShop);

            SetRecipeShopNotif(false);

            ArcEvent.OnArcChanged += RecipeNotifOn;
        }

        private void PopulateShop()
        {
            foreach (GameObject card in currRecipeCards)
            {
                Destroy(card);
            }
            // Update visual currency display
            UpdateCurrencyData();

            Queue<Recipe> soldRecipesQueue = new Queue<Recipe>();

            //Iterate through all recipes
            foreach (Recipe recipe in recipes)
            {
                //Store sold recipes in a queue to display towards the end
                if (boughtRecipes.Contains(recipe))
                {
                    soldRecipesQueue.Enqueue(recipe);
                    continue;
                }
                //Instantiate recipe UI and populate data in recipe prefab
                if ((int)recipe.unlockArc <= (int)GameManager.Instance.arc)
                {
                    GameObject recipeGameObject = Instantiate(recipePrefab, recipesContainer.transform);
                    currRecipeCards.Add(recipeGameObject);
                    RecipeClickHandler recipeClickHandler = recipeGameObject.GetComponent<RecipeClickHandler>();
                    recipeClickHandler.SetRecipe(recipe);
                }

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
            //Store current recipe under consideration
            currentRecipe = new Tuple<Recipe, RecipeClickHandler>(recipeData, recipeClickHandler);

            //Show prompt to buy the recipe
            if (boughtRecipes.Contains(recipeData) == false)
            {
                ShowTransactionConfirmationPrompt(true);

            } //TODO: Work on selling logic
            else //Already bought, cannot be sold
            {
                if (shouldDebug) Debug.Log($"You cannot sell this recipe currently!({recipeData.name})");
            }

        }

        //Shows the UI prompt to ask the user if they want to confirm purchasing/selling a recipe
        private void ShowTransactionConfirmationPrompt(bool isRecipeBought)
        {
            recipeTransactionMessagePromptText.text = isRecipeBought ? recipePurchasePromptMessage : recipeSalePromptMessage;

            recipeTransactionConfirmationPromptUI.SetActive(true);
        }

        private void HideTransactionConfirmationPrompt()
        {
            recipeTransactionConfirmationPromptUI.SetActive(false);
        }

        public void CompleteTransaction(bool transactionWasConfirmed)
        {
            if (currentRecipe == null)
            {
                Debug.LogError("No recipe selected when completing the transaction!");
                return;
            }

            //Cancel the transaction
            if (!transactionWasConfirmed)
            {
                CancelTransaction();
                return;
            }

            //Attempt to buy the recipe
            if (boughtRecipes.Contains(currentRecipe.Item1) == false)
            {
                if (GameManager.Instance.GetCurrency() >= currentRecipe.Item1.buyPrice)
                {
                    if (shouldDebug) Debug.Log($"You have enough to buy the recipe : ({currentRecipe.Item1.name}). Bought for {currentRecipe.Item1.buyPrice}!");

                    //Buy the recipe
                    // currentRecipe.Item1.isBought = true;
                    boughtRecipes.Add(currentRecipe.Item1);
                    //Move the recipe towards the end of the shop (because sold)
                    currentRecipe.Item2.gameObject.transform.SetSiblingIndex(-1);
                    currentRecipe.Item2.SetRecipeSold();
                    GameManager.Instance.AddUnlockedRecipe(currentRecipe.Item1);
                    //Update currency
                    GameManager.Instance.AddCurrency(-currentRecipe.Item1.buyPrice);
                }
                else
                {
                    if (shouldDebug) Debug.Log($"You don't have enough to buy this recipe!({currentRecipe.Item1.name})");
                }
            }

            //After buying or selling, update currency value
            UpdateCurrencyData();

            //Upon transaction completion, hide the transaction prompt
            CancelTransaction();
        }

        private void UpdateCurrencyData()
        {
            if (shouldDebug) Debug.Log($"Updating currency data to: {GameManager.Instance.GetCurrency()}");

            currencyText.text = "Currency: $" + GameManager.Instance.GetCurrency().ToString();
        }

        private void ShowRecipeShop()
        {
            if (GameManager.Instance.state == State.Main)
            {
                PopulateShop();
                // Update visual currency display
                UpdateCurrencyData();

                //Incase a transaction is open, cancel it 
                CancelTransaction();

                //Show the recipe shop UI
                recipeShopUI.SetActive(true);

                //Show and hide relevant buttons
                recipeShopOpenButton.gameObject.SetActive(false);
                recipeShopReturnButton.gameObject.SetActive(true);
                GameManager.Instance.state = State.Recipe;

                SetRecipeShopNotif(false);
            }
        }

        public void CancelTransaction()
        {
            //Unset recipe under consideration
            currentRecipe = null;

            //Hide transaction prompt screen in case open
            HideTransactionConfirmationPrompt();
        }

        private void HideRecipeShop()
        {
            foreach (GameObject card in currRecipeCards)
            {
                Destroy(card);
            }
            recipeShopUI.SetActive(false);

            //Show and hide relevant buttons
            recipeShopReturnButton.gameObject.SetActive(false);
            recipeShopOpenButton.gameObject.SetActive(true);

            //Cancel transaction in the case that a transaction is being considered
            CancelTransaction();
            GameManager.Instance.state = State.Main;
        }

        private void OnDestroy()
        {
            //Unbind buttons before destruction
            recipeShopOpenButton.onClick.RemoveListener(ShowRecipeShop);
            recipeShopReturnButton.onClick.RemoveListener(HideRecipeShop);
            ArcEvent.OnArcChanged -= RecipeNotifOn;
        }

        public bool IsRecipeBought(Recipe recipe)
        {
            if (boughtRecipes.Contains(recipe))
            {
                return true;
            }
            return false;
        }

        private void RecipeNotifOn()
        {
            SetRecipeShopNotif(true);
        }
        
        public void SetRecipeShopNotif(bool b)
        {
            recipeNotifAnimator.SetBool("isActive", b);
        }
    }
}
