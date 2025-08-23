using Unity.VisualScripting;
using UnityEngine;

public class RingScript : MonoBehaviour
{
    public GameObject ring;
    public Sprite ring_sprite;
    private float start_x;
    private float start_y;
    private float end_x;
    private float end_y;

    private float speed=4f;

    public static int score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        start_x = ring.transform.localScale.x;
        start_y = ring.transform.localScale.y;

        end_x = start_x / 2;
        end_y = start_y / 2;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Reset();

        if (spriteRenderer != null && ring_sprite != null)
        {
            spriteRenderer.sprite = ring_sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(ring.transform.localScale.x >= end_x)
        {
            ring.transform.localScale -= new Vector3((speed * 0.3f * Time.deltaTime), 0, 0);
        }

        if(ring.transform.localScale.y >= end_y)
        {
            ring.transform.localScale -= new Vector3(0, speed * 0.2f * Time.deltaTime, 0);
        }

        if(ring.transform.localScale.x <= end_x && ring.transform.localScale.y <= end_y)
        {
            Reset();
        }
        
    }

    private void Reset()
    {
        ring.transform.localScale = new Vector3(start_x * 2, start_y * 2, 1);
    }

    public int GetScore()
    {
        float diff = start_x-ring.transform.localScale.x;
        score = Mathf.RoundToInt(diff/(ring.transform.localScale.x*2-ring.transform.localScale.x)*10);

        Reset();

        return score;
    }

    public static string ScoreToString(int score)
    {
        Debug.Log(score);
        if (score >= 0 && score <= 1)
        {
            return "Great! +10";
        }
        else if (score >= -1 && score <= 2)
        {
            return "Okay. +2";
        }
        else
        {
            return "Bad... +0";
        }
    }

    public static float ScoreToSliderIncrement(int score)
    {
        if (score >= 0 && score <= 1)
        {
            return 10;
        }
        else if (score >= -1 && score <= 2)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }
}
