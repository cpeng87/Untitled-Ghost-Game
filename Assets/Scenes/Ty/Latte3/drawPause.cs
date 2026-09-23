using UnityEngine;

public class DrawPause : MonoBehaviour
{
    public LatteDraw LatteDrawer;
    public GameObject cookbook;
    public bool showOnStart = false;

    void Start()
    {
        if (showOnStart)
            Show();
        else
            Hide();
    }

    public void Show()
    {
        if (cookbook != null) cookbook.SetActive(true);
        if (LatteDrawer != null) LatteDrawer.SetPaused(true);
    }

    public void Hide()
    {
        if (cookbook != null) cookbook.SetActive(false);
        if (LatteDrawer != null) LatteDrawer.SetPaused(false);
    }

    // Handy for wiring directly to a UI Button's OnClick event.
    public void ToggleVisible()
    {
        if (cookbook != null && cookbook.activeSelf)
            Hide();
        else
            Show();
    }
}