using System;

public static class EventManager
{
    public static event Action OnOrderCompleted;

    public static void OrderCompleted()
    {
        OnOrderCompleted?.Invoke();
    }
}
