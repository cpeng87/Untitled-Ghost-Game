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

    public Order(string ghostName, string minigame, string recipeName, int price, int seatNum)
    {
        this.ghostName = ghostName;
        this.minigame = minigame;
        this.recipeName = recipeName;
        this.price = price;
        this.seatNum = seatNum;
    }
}
public class OrderManager : MonoBehaviour
{
    public List<Order> activeOrders = new List<Order>();  //list of active orders for ghosts
    private int currActiveOrder = -1;  //keeps track of the current order being worked on after make order is pressed. normally set to -1 to indicate no current order being worked on

    //Randomized a recipe that is avaliable to order. Adds that order and triggers dialogue for the order
    //Returns whether it is a success or fail to tell the object in Ghostobj to switch states
    public bool TakeOrder(string name, List<string> orderDialogue, List<Recipe> recipes, int seatNum)
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

        activeOrders.Add(new Order(name, recipes[selectedIndex].minigame, recipes[selectedIndex].recipeName, recipes[selectedIndex].sellPrice, seatNum));
        currActiveOrder = activeOrders.Count - 1;
        DialoguePlayer.Instance.StartOrderDialogue(name, recipes[selectedIndex].recipeName, seatNum);
        // dialoguePlayer = FindObjectsByType<DialoguePlayer>(FindObjectsSortMode.None)[0];
        // dialoguePlayer.StartOrderDialogue(name, recipes[selectedIndex].recipeName, seatNum);
        // orderDialogue = TagReplacer(orderDialogue, "{item}", recipes[selectedIndex].recipeName);
        // DialogueManager.Instance.StartDialogue(name, orderDialogue, seatNum);
        // Debug.Log("Size of orders: " + activeOrders.Count);
        return true;
    }

    //calls game manager to switch to the minigame! Currently does the first order.
    public void MakeOrder(int orderIdx)
    {
        if (activeOrders.Count == 0)
        {
            Debug.Log("No active orders.");
            return;
        }

        if (orderIdx < 0 || orderIdx >= activeOrders.Count)
        {
            Debug.Log("Invalid index for order.");
        }

        TicketManager.Instance.HideOrders();

        GameManager.Instance.SwitchToMinigame(activeOrders[orderIdx].minigame);
        currActiveOrder = orderIdx;
    }

    //completes an order
    //if success, say success dialogue and get money, if fail say fail dialogue.
    public void CompleteOrder(bool result)
    {
        if (currActiveOrder == -1)
        {
            Debug.Log("There is no current active order, cannot switch seen! If testing disregard...");
        }
        Ghost currGhost = GameManager.Instance.ghostManager.GetGhostScriptableFromName(activeOrders[currActiveOrder].ghostName);

        // dialoguePlayer = FindObjectsByType<DialoguePlayer>(FindObjectsSortMode.None)[0];
        // dialoguePlayer.CompleteOrderDialogue(currGhost.ghostName, activeOrders[currActiveOrder].seatNum, result);
        // DialogueManager.Instance.CompleteOrderDialogue(currGhost.ghostName, activeOrders[currActiveOrder].seatNum, result);
        DialoguePlayer.Instance.CompleteOrderDialogue(currGhost.ghostName, activeOrders[currActiveOrder].seatNum, result);
        // if (result)
        // {
        //     // List<string> successDialogue = TagReplacer(currGhost.success, "{item}", activeOrders[currActiveOrder].recipeName);
        //     // //add story dialogue as well
        //     // int storyIndex = GameManager.Instance.ghostManager.GetStoryIndex(currGhost.ghostName);
        //     // GameManager.Instance.ghostManager.IncrementStoryIndex(currGhost.ghostName);
        //     // List<string> storyDialogue = currGhost.story[storyIndex];
        //     // List<string> combinedDialogue = successDialogue;
        //     // combinedDialogue.AddRange(storyDialogue);
        //     // DialogueManager.Instance.CompleteOrderDialogue(currGhost.ghostName, combinedDialogue, activeOrders[currActiveOrder].seatNum);
        //     // GameManager.Instance.AddCurrency(activeOrders[currActiveOrder].price);
        // }
        // else
        // {
        //     List<string> failureDialogue = TagReplacer(currGhost.failure, "{item}", activeOrders[currActiveOrder].recipeName);
        //     DialogueManager.Instance.CompleteOrderDialogue(currGhost.ghostName, failureDialogue, activeOrders[currActiveOrder].seatNum);
        // }

    }

    public void RemoveCompletedOrder()
    {
        Ghost currGhost = GameManager.Instance.ghostManager.GetGhostScriptableFromName(activeOrders[currActiveOrder].ghostName);
        GameManager.Instance.ghostManager.RemoveActiveGhost(currGhost);
        activeOrders.RemoveAt(currActiveOrder);
        Debug.Log("Completed order!");
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
            if (order.ghostName == ghostName)
            {
                return true;
            }
        }
        return false;
    }
}
