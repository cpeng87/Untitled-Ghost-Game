using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChefSkipUpdate : MinigameCompletion
{
    [SerializeField] private GameObject skipMinigamePopup;
    [SerializeField] public TMP_Text currencyField;
    [SerializeField] public TMP_Text costField;
    [SerializeField] private int chefCost = 3;
    [SerializeField] private GameObject currencyError;

    private void Start()
    {
        skipMinigamePopup.SetActive(false);
        currencyError.SetActive(false);
    }

    public void ToggleSkipPopup()
    {
        if (!skipMinigamePopup.activeSelf)
        {
            UpdateCostField();
            UpdateCurrencyField();
            skipMinigamePopup.SetActive(true);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            skipMinigamePopup.SetActive(false);
        }
    }

    public void PayChef()
    {
        int currency = GameManager.Instance.GetCurrency();

        if (currency >= chefCost)
        {
            GameManager.Instance.SubtractCurrency(3);
            skipMinigamePopup.SetActive(false);
            minigameResult.MinigameResult(true);
        }
        else
        {
            skipMinigamePopup.SetActive(false);
            currencyError.SetActive(true);
        }
    }

    public void UpdateCostField()
    {
        costField.text = "Minigame will be skipped. This action will cost " + chefCost + " currency.";
        Debug.Log("current cost: " + chefCost);
    }

    public void UpdateCurrencyField() 
    {
        int currency = GameManager.Instance.GetCurrency();
        currencyField.text = "Coins: " + currency;
    }
}
