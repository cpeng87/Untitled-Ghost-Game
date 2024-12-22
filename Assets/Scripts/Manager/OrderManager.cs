using UnityEngine;
using System.Collections.Generic;

public class Order
{
    public string ghostName;
    public string minigame;
    public string recipeName;
    public int price;

    public Order(string ghostName, string minigame, string recipeName, int price)
    {
        this.ghostName = ghostName;
        this.minigame = minigame;
        this.recipeName = recipeName;
        this.price = price;
    }
}
public class OrderManager : MonoBehaviour
{
    public List<Order> activeOrders = new List<Order>();
    private int currActiveOrder = -1;  //keeps track of the current order being worked on after make order is pressed. normally set to -1 to indicate no current order being worked on

    //Randomized a recipe that is avaliable to order. Adds that order and triggers dialogue for the order
    //Returns whether it is a success or fail to tell the object in Ghostobj to switch states
    public bool TakeOrder(string name, List<string> orderDialogue, List<Recipe> recipes)
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

        activeOrders.Add(new Order(name, recipes[selectedIndex].minigame, recipes[selectedIndex].recipeName, recipes[selectedIndex].sellPrice));
        currActiveOrder = activeOrders.Count - 1;
        orderDialogue = TagReplacer(orderDialogue, "{item}", recipes[selectedIndex].recipeName);
        DialogueManager.Instance.StartDialogue(name, orderDialogue);
        return true;
    }

    //calls game manager to switch to the minigame! Currently does the first order.
    public void MakeOrder()
    {
        GameManager.Instance.SwitchToMinigame(activeOrders[0].minigame);
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
        if (result)
        {
            List<string> successDialogue = TagReplacer(currGhost.success, "{item}", activeOrders[currActiveOrder].recipeName);
            DialogueManager.Instance.StartDialogue(currGhost.ghostName, successDialogue);
            GameManager.Instance.AddCurrency(activeOrders[currActiveOrder].price);
        }
        else
        {
            List<string> failureDialogue = TagReplacer(currGhost.failure, "{item}", activeOrders[currActiveOrder].recipeName);
            DialogueManager.Instance.StartDialogue(currGhost.ghostName, failureDialogue);
        }
        activeOrders.RemoveAt(currActiveOrder);
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
}
