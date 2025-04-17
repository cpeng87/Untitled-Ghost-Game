using UnityEngine;

public class ArcEvent
{
    public static event System.Action OnArcChanged;

    public static void TriggerArcChanged()
    {
        if (OnArcChanged != null)
        {
            OnArcChanged.Invoke();
        }
    }
}
