// using UnityEditor.Build.Reporting;
// using UnityEngine;

// public class LatteDecorating : MonoBehaviour
// {
//     [SerializeField] private Camera cam;
//     [SerializeField] private int xs = 1024;
//     [SerializeField] private int ys = 1024;
//     [SerializeField] private int brushSize = 40;
//     [SerializeField] private Color brushColor;
//     [SerializeField] Material mat;
//     private Texture2D texture;
//     private Color[] colorMap;
//     [SerializeField] private Transform topLeftCorner;
//     [SerializeField] private Transform bottomRightCorner;
//     [SerializeField] private Transform point;
//     private bool useInterpolation = true;
//     private int x;
//     private int y;
//     private bool pressedLastFrame = false;
//     private int lastX = 0;
//     private int lastY = 0;
//     private float xMult;
//     private float yMult;

//     [SerializeField] private Texture2D textureGoal;

//     private void Start()
//     {
//         colorMap = new Color[xs * ys];
//         texture = new Texture2D(ys, xs, TextureFormat.RGBA32, false, true);
//         texture.filterMode = FilterMode.Point;
//         mat.SetTexture("_MainTex", texture);
//         xMult = xs / (bottomRightCorner.localPosition.x - topLeftCorner.localPosition.x);
//         yMult = ys / (bottomRightCorner.localPosition.y - topLeftCorner.localPosition.y);
//     }
//     private void Update()
//     {
//         if (Input.GetMouseButton(0))
//             CalculatePixel();
//         else 
//             pressedLastFrame = false;
//     }
//     private void CalculatePixel()
//     {
//         Ray ray = cam.ScreenPointToRay(Input.mousePosition);
//         RaycastHit hit;
//         if (Physics.Raycast(ray, out hit, 10f))
//         {
//             point.position = hit.point;
//             x = (int)((point.localPosition.x - topLeftCorner.localPosition.x) * xMult);
//             x = xs - x;
//             y = (int)((point.localPosition.y - topLeftCorner.localPosition.y) * yMult);
//             //x = (int)((point.localPosition.x - bottomRightCorner.localPosition.x) * xMult);
//             //y = (int)((topLeftCorner.localPosition.y - point.localPosition.y) * yMult);
//             ChangePixelsAroundPoint(); 
//         }
//         else
//             pressedLastFrame = false; 
//     }

//     private void ChangePixelsAroundPoint() 
//     {
//         if (useInterpolation && pressedLastFrame && (lastX != x || lastY != y)) 
//         {
//             int dist = (int)Mathf.Sqrt((x - lastX) * (x - lastX) + (y - lastY) * (y - lastY)); 
//             for (int i = 1; i <= dist; i++) 
//                 DrawBrush((i * x + (dist - i) * lastX) / dist, (i * y + (dist - i) * lastY) / dist); 
//         }
//         else 
//             DrawBrush(x, y); 
//         pressedLastFrame = true; 
//         lastX = x;
//         lastY = y;
//         SetTexture();
//     }
//     private void DrawBrush(int xPix, int yPix) 
//     {
//             int i = xPix - brushSize + 1, j = yPix - brushSize + 1, maxi = xPix + brushSize - 1, maxj = yPix + brushSize - 1;
//             if (i < 0) 
//                 i = 0;
//             if (j < 0)
//                 j = 0;
//             if (maxi >= xs) 
//                 maxi = xs - 1;
//             if (maxj >= ys)
//                 maxj = ys - 1;
//             for (int x = i; x <= maxi; x++)
//             {
//                 for (int y = j; y <= maxj; y++)
//                 {
//                     if ((x - xPix) * (x - xPix) + (y - yPix) * (y - yPix) <= brushSize * brushSize)
//                     {
//                         colorMap[(xs - x) * ys + y] = brushColor;
//                     }
//                 }
//             }
//     }
//     private void SetTexture() 
//     {
//         texture.SetPixels(colorMap);
//         texture.Apply();
//     }

//     private bool IsPixelActive(Color color)
//     {
//         // Consider a pixel active if not transparent and not black
//         return color.a > 0.01f && (color.r > 0.01f || color.g > 0.01f || color.b > 0.01f);
//     }

//     public float CalculateAccuracy()
//     {
//         Color[] colorData = texture.GetPixels();
//         Color[] goal = textureGoal.GetPixels();

//         int pixelsInside = 0;
//         int pixelsOutside = 0;

//         for (int i = 0; i < colorData.Length; i++)
//         {
//             Color generatedColors = colorData[i];
//             Color referenceColors = goal[i];

//             if (IsPixelActive(generatedColors))
//             {
//                 if (IsPixelActive(referenceColors))
//                 {
//                     pixelsInside++;
//                 }
//                 else
//                 {
//                     pixelsOutside++;
//                 }
//             }
//         }

//         int totalPixels = pixelsInside + pixelsOutside;
//         float percentInside = pixelsInside / totalPixels;
//         float percentOutside = pixelsOutside / totalPixels;

//         return (float) (percentInside - (0.5 * percentOutside));
//     }
// }
