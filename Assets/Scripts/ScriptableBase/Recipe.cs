using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe", order = 0)]
public class Recipe : ScriptableObject 
{
    public string recipeName;
    public int sellPrice; //how much you get when you make and sell one of these items
    public int buyPrice; //how much you pay when you buy from recipe store
    public string minigame;
}
