using UnityEngine;

public class GhostObj : Clickable
{
    [SerializeField] private Ghost scriptable;
    public bool hasTakedOrder;
    protected override void OnClicked()
    {
        if (hasTakedOrder)
        {
            return;
        }
        hasTakedOrder = GameManager.Instance.orderManager.TakeOrder(scriptable.ghostName, scriptable.order, scriptable.recipesOrdered);
    }

    public Ghost GetScriptable()
    {
        return scriptable;
    }
}
