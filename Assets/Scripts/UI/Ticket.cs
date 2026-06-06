using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ticket : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TextMeshProUGUI ghostName;
    [SerializeField] private TextMeshProUGUI recipeName;
    [SerializeField] private Image foodImage;
    [SerializeField] private Button button;

    public void SetTicket(string ghostName, string recipeName, Sprite foodImage, int seatNum)
    {
        this.ghostName.text = name;
        this.recipeName.text = recipeName;
        this.foodImage.sprite = foodImage;
        this.button.onClick.AddListener(() => GameManager.Instance.orderManager.MakeOrder(seatNum));
    }
}
