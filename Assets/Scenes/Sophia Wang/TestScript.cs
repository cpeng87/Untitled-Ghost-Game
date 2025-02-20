using UnityEngine;

class Draw : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private int totalXPixels = 1024;
    [SerializeField] private int totalYPixels = 512;
    [SerializeField] private int brushSize = 4;
    [SerializeField] private Color brushColor;
    [SerializeField] private bool useInterpolation = true;
    [SerializeField] private Transform topLeftCorner;
    [SerializeField] private Transform bottomRightCorner;
    [SerializeField] private Transform point;
    [SerializeField] private Material material;
    [SerializeField] private Texture2D generatedTexture;
    Color[] colorMap;
    [SerializeField] private int xPixel = 0;
    [SerializeField] private int yPixel = 0;
    [SerializeField] private bool pressedLastFrame = false;
    [SerializeField] private int lastX = 0;
    [SerializeField] private int lastY = 0;
    [SerializeField] private float xMult;
    [SerializeField] private float yMult;

    private void Start()
    {
        colorMap = new Color[totalXPixels * totalYPixels];
        generatedTexture = new Texture2D(totalYPixels, totalXPixels, TextureFormat.RGBA32, false);
        generatedTexture.filterMode = FilterMode.Point;
        material.SetTexture("_BaseMap", generatedTexture); 

        ResetColor();

        xMult = totalXPixels / (bottomRightCorner.localPosition.x - topLeftCorner.localPosition.x);
        yMult = totalYPixels / (bottomRightCorner.localPosition.y - topLeftCorner.localPosition.y);
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
            xPixel = (int)((point.localPosition.x - topLeftCorner.localPosition.x) * xMult);
            yPixel = (int)((point.localPosition.y - topLeftCorner.localPosition.y) * yMult);
            ChangePixelsAroundPoint(); 
        }
        else
            pressedLastFrame = false; 
    }

    private void ChangePixelsAroundPoint() 
    {
        if (useInterpolation && pressedLastFrame && (lastX != xPixel || lastY != yPixel))
        {
            int dist = (int)Mathf.Sqrt((xPixel - lastX) * (xPixel - lastX) + (yPixel - lastY) * (yPixel - lastY));
            for (int i = 1; i <= dist; i++) 
                DrawBrush((i * xPixel + (dist - i) * lastX) / dist, (i * yPixel + (dist - i) * lastY) / dist);
        }
        else 
            DrawBrush(xPixel, yPixel); 
        pressedLastFrame = true; 
        lastX = xPixel;
        lastY = yPixel;
        SetTexture();
    }

    private void DrawBrush(int xPix, int yPix) 
    {
        int i = xPix - brushSize + 1, j = yPix - brushSize + 1, maxi = xPix + brushSize - 1, maxj = yPix + brushSize - 1; 
        if (i < 0) 
            i = 0;
        if (j < 0)
            j = 0;
        if (maxi >= totalXPixels)
            maxi = totalXPixels - 1;
        if (maxj >= totalYPixels)
            maxj = totalYPixels - 1;
        for (int x = i; x <= maxi; x++)
        {
            for (int y = j; y <= maxj; y++)
            {
                if ((x - xPix) * (x - xPix) + (y - yPix) * (y - yPix) <= brushSize * brushSize)
                    colorMap[x * totalYPixels + y] = brushColor;
            }
        }
    }

    private void SetTexture()
    {
        generatedTexture.SetPixels(colorMap);
        generatedTexture.Apply();
    }

    private void ResetColor() 
    {
        for (int i = 0; i < colorMap.Length; i++)
            colorMap[i] = new Color(200, 160, 130); //Color.white;
        SetTexture();
    }

}