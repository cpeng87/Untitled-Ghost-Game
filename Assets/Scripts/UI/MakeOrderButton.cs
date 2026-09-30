using UnityEngine;
using UnityEngine.UI;

public class MakeOrderButton : MonoBehaviour
{
    [SerializeField] private Button btn;

    public void Start()
    {
        btn.interactable = false;
    }
}
