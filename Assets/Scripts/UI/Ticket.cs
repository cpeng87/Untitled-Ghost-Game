using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ticket : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI recipeName;
    [SerializeField] private Image foodImage;
    [SerializeField] private Button button;

    public void SetTicket(string name, string recipeName, Sprite foodImage, int seatNum)
    {
        this.name.text = name;
        this.recipeName.text = recipeName;
        this.foodImage.sprite = foodImage;
        this.button.onClick.AddListener(() => GameManager.Instance.orderManager.MakeOrder(seatNum));
    }
}
