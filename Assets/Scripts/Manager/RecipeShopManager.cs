using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeShopManager : MonoBehaviour
{
    
    public GameObject recipePrefab;
    
    //Recipes to populate the shop with
    [SerializeField] private List<Recipe> recipes;
    
    //UI references
    [SerializeField] private GameObject recipeShop;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private Button recipeShopOpenButton;
    [SerializeField] private Button recipeShopReturnButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        //Instaniate and populate data in recipe prefab
        //Add to shop content
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
