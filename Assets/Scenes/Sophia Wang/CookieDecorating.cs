using UnityEngine;

public class CookieDecorating : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private int xs = 1024;
    [SerializeField] private int ys = 1024;
    private int radius = 512;
    [SerializeField] private int brushSize = 40;
    [SerializeField] private Color brushColor;
    [SerializeField] Material mat;
    private Texture2D texture;
    private Color[] colorMap;
    [SerializeField] private Transform topLeftCorner;
    [SerializeField] private Transform bottomRightCorner;
    [SerializeField] private Transform point;
    private bool useInterpolation = true;
    private int x;
    private int y;
    private bool pressedLastFrame = false;
    private int lastX = 0;
    private int lastY = 0;
    private float xMult;
    private float yMult;

    private void Start()
    {
        colorMap = new Color[xs * ys];
        texture = new Texture2D(ys, xs, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        mat.SetTexture("_MainTex", texture);
        xMult = xs / (bottomRightCorner.localPosition.x - topLeftCorner.localPosition.x);
        yMult = ys / (bottomRightCorner.localPosition.y - topLeftCorner.localPosition.y);
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
            CalculatePixel();
        else 
            pressedLastFrame = false;
    }
    private void CalculatePixel()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f))
        {
            point.position = hit.point;
            x = (int)((point.localPosition.x - topLeftCorner.localPosition.x) * xMult); 
            y = (int)((point.localPosition.y - topLeftCorner.localPosition.y) * yMult);
            ChangePixelsAroundPoint(); 
        }
        else
            pressedLastFrame = false; 
    }

    private void ChangePixelsAroundPoint() 
    {
        if (useInterpolation && pressedLastFrame && (lastX != x || lastY != y)) 
        {
            int dist = (int)Mathf.Sqrt((x - lastX) * (x - lastX) + (y - lastY) * (y - lastY)); 
            for (int i = 1; i <= dist; i++) 
                DrawBrush((i * x + (dist - i) * lastX) / dist, (i * y + (dist - i) * lastY) / dist); 
        }
        else 
            DrawBrush(x, y); 
        pressedLastFrame = true; 
        lastX = x;
        lastY = y;
        SetTexture();
    }
    private void DrawBrush(int xPix, int yPix) 
    {
        //if ((x - xPix) * (x - xPix) + (y - yPix) * (y - yPix) <= radius * radius)
        //{
            int i = xPix - brushSize + 1, j = yPix - brushSize + 1, maxi = xPix + brushSize - 1, maxj = yPix + brushSize - 1;
            if (i < 0) 
                i = 0;
            if (j < 0)
                j = 0;
            if (maxi >= xs) 
                maxi = xs - 1;
            if (maxj >= ys)
                maxj = ys - 1;
            for (int x = i; x <= maxi; x++)
            {
                for (int y = j; y <= maxj; y++)
                {
                    if ((x - xPix) * (x - xPix) + (y - yPix) * (y - yPix) <= brushSize * brushSize) 
                        colorMap[x * ys + y] = brushColor;
                }
            }
        //}  
    }
    private void SetTexture() 
    {
        texture.SetPixels(colorMap);
        texture.Apply();
    }
}
