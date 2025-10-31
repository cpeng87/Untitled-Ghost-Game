using UnityEngine;
using System.Collections.Generic;
using Manager.CustomerPatience;
using TMPro;

public class Order
{
    public string ghostName;
    public string minigame;
    public string recipeName;
    public int price;
    public int seatNum;
    public Sprite foodImage;

    public Order(string ghostName, string minigame, string recipeName, int price, int seatNum, Sprite foodImage)
    {
        this.ghostName = ghostName;
        this.minigame = minigame;
        this.recipeName = recipeName;
        this.price = price;
        this.seatNum = seatNum;
        this.foodImage = foodImage;
    }
}
public class OrderManager : MonoBehaviour
{
    public Order[] activeOrders = new Order[3];  //list of active orders for ghosts
    private int currActiveOrder = -1;  //keeps track of the current order being worked on after make order is pressed. normally set to -1 to indicate no current order being worked on

    //Randomized a recipe that is avaliable to order. Adds that order and triggers dialogue for the order
    //Returns whether it is a success or fail to tell the object in Ghostobj to switch states
    // public bool TakeOrder(string name, List<string> orderDialogue, List<Recipe> recipes, int seatNum)
    public bool TakeOrder(string name, List<Recipe> recipes, int seatNum)
    {

        List<Recipe> possibleRecipe = new List<Recipe>();
        foreach(Recipe recipe in recipes)
        {
            if (GameManager.Instance.unlockedRecipes.Contains(recipe))
            {
                possibleRecipe.Add(recipe);
            }
        }
        if (possibleRecipe.Count == 0)
        {
            Debug.Log("No possible recipes, something went terribly wrong.");
            return false;
        }
        int selectedIndex = (int) (Random.value * possibleRecipe.Count);

        activeOrders[seatNum] = new Order(name, recipes[selectedIndex].minigame, recipes[selectedIndex].recipeName, recipes[selectedIndex].sellPrice, seatNum, recipes[selectedIndex].foodImage);
        currActiveOrder = seatNum;
        DialoguePlayer.Instance.StartOrderDialogue(name, recipes[selectedIndex].recipeName, seatNum);
        return true;
    }

    public void MakeOrder(int orderIdx)
    {
        AudioManager.Instance.PlaySound("ButtonDown");
        if (GetNumActiveOrder() == 0)
        {
            Debug.Log("No active orders.");
            return;
        }

        if (orderIdx < 0 || orderIdx >= GetNumActiveOrder())
        {
            Debug.Log("Invalid index for order.");
        }

        TicketManager.Instance.HideOrders();
        AudioManager.Instance.SaveSong();

        GameManager.Instance.SwitchToMinigame(activeOrders[orderIdx].minigame);
        currActiveOrder = orderIdx;
    }

    //completes an order
    //if success, say success dialogue and get money, if fail say fail dialogue.
    public void CompleteOrder(bool result, bool chefSkip, bool specialCookie)
    {
        if (currActiveOrder == -1)
        {
            Debug.Log("There is no current active order, cannot switch seen! If testing disregard...");
        }
        AudioManager.Instance.ContinueSong();
        Ghost currGhost = GameManager.Instance.ghostManager.GetGhostScriptableFromName(activeOrders[currActiveOrder].ghostName);
        DialoguePlayer.Instance.CompleteOrderDialogue(currGhost.ghostName, activeOrders[currActiveOrder].seatNum, result, specialCookie);
        
        if (!chefSkip && result)
        {
            GameManager.Instance.AddCurrency(1);
        }
        else
        {
            GameManager.Instance.UpdateCurrency();
        }
    }

    public void RemoveCompletedOrder()
    {
        Ghost currGhost = GameManager.Instance.ghostManager.GetGhostScriptableFromName(activeOrders[currActiveOrder].ghostName);
        GameManager.Instance.ghostManager.RemoveActiveGhost(currGhost);
        GhostSpawningManager.Instance.DeleteSpawnedGhost(activeOrders[currActiveOrder].seatNum);
        activeOrders[currActiveOrder] = null;
    }

    //helper method to replace tags in strings
    //Ex. "I love {item}" and you want to replace with "tea", then it returns "I love tea" if tag is "{item}" and newString is "tea"
    public List<string> TagReplacer(List<string> dialogue, string tag, string newString)
    {
        List<string> rtn = new List<string>();
        for (int i = 0; i < dialogue.Count; i++)
        {
            rtn.Add(dialogue[i].Replace(tag, newString));
        }
        return rtn;
    }

    //returns seat number based on name of ghost, returns -1 if not found
    public int GetSeatNum(string ghostName)
    {
        foreach (Order order in activeOrders)
        {
            if (order.ghostName == ghostName)
            {
                return order.seatNum;
            }
        }
        Debug.Log("Could not find seat num for ghost with name: " + ghostName + ". An error has occured.");
        return -1;
    }

    //returns true if ghost has an active order, false otherwise
    public bool HasActiveOrder(string ghostName)
    {
        foreach (Order order in activeOrders)
        {
            if (order != null)
            {
                if (order.ghostName == ghostName)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public string GetCurrActiveOrderName()
    {
        return activeOrders[currActiveOrder].ghostName;
    }

    public int GetCurrActiveOrderSeatNum()
    {
        return activeOrders[currActiveOrder].seatNum;
    }

    public int GetCurrActiveOrderPrice()
    {
        return activeOrders[currActiveOrder].price;
    }

    public int GetNumActiveOrder()
    {
        int counter = 0;
        foreach (Order order in activeOrders)
        {
            if (order != null)
            {
                counter += 1;
            }
        }
        return counter;
    }
}
