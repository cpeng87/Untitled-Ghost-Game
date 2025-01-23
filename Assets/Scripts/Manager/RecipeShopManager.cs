using System;
using UnityEngine;
using UnityEngine.UI;

public class RecipeShopManager : MonoBehaviour
{
    [SerializeField] private GameObject recipeShop;

    [SerializeField] private Button recipeShopOpenButton;
    [SerializeField] private Button recipeShopReturnButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initially hide and show relevant gameobjects
        recipeShopOpenButton.gameObject.SetActive(true);
        recipeShopReturnButton.gameObject.SetActive(false);
        recipeShop.gameObject.SetActive(false);
        
        //Binds buttons to their respective functions
        recipeShopOpenButton.onClick.AddListener(ShowRecipeShop);
        recipeShopReturnButton.onClick.AddListener(HideRecipeShop);
    }
    

    public void ShowRecipeShop()
    {
        recipeShop.SetActive(true);
        
        //Show and hide relevant buttons
        recipeShopOpenButton.gameObject.SetActive(false);
        recipeShopReturnButton.gameObject.SetActive(true);
    }

    public void HideRecipeShop()
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
