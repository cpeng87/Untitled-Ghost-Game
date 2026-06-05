using UnityEngine;

public class Stamps : MonoBehaviour
{
    [SerializeField] public float totalTime;
    [SerializeField] public float startScale;
    private float elapsedTime;
    
    public void OnEnable()
    {   
        elapsedTime = 0f;
        this.gameObject.transform.localScale = new Vector3(startScale, startScale, 1f);
    }

    public void Update()
    {
        elapsedTime += Time.unscaledDeltaTime;
        float val = Mathf.Lerp(startScale, 1, elapsedTime / totalTime);
        this.gameObject.transform.localScale = new Vector3(val, val, 1f);
    }
}
