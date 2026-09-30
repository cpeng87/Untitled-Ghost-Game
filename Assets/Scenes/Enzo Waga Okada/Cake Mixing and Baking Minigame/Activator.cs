using UnityEngine;
using System.Collections;

public class Activator : MinigameCompletion {
    public KeyCode key;
    bool active = false;
    GameObject note;
    private SpriteRenderer spriteRenderer;
    private Color green;
    private Color red;
    private ParticleSystem sparkles;

    void Start() {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        green = new Color(0f, 203f/255f, 29f/255f, 1f);
        red = new Color(202f/255f, 6f/255f, 0f, 1f);
        sparkles = this.gameObject.GetComponent<ParticleSystem>();
    }

    void Update() {
        spriteRenderer.color = Color.Lerp(green, red, (Mathf.Abs(note.transform.position.x)) / 5f);
        Debug.Log(Mathf.Abs(note.transform.position.x) / 5f);
        if (active)
        {
            // spriteRenderer.color = Color.Lerp(green, red, Mathf.Abs(note.transform.position.x - 5) / 5f);
            // Debug.Log(5 - Mathf.Abs(note.transform.position.x) / 5f);
            if (Input.GetKeyDown(key)) {
                if (note.transform.position.x <= 0.4f && note.transform.position.x >= -0.4f)
                {
                    Destroy(note);
                    sparkles.Play();

                    StartCoroutine(Finish());
                }

            }
        }
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(0.6f);
        minigameResult.MinigameResult(true);
    }

    void OnTriggerEnter2D(Collider2D col) {
        active = true;
        // spriteRenderer.color = green;
        if (col.gameObject.tag == "Note") {
            note = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        // spriteRenderer.color = red;
        active = false;
    }
}
