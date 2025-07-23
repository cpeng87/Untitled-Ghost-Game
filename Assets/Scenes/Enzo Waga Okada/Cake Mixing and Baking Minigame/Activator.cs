using UnityEngine;

public class Activator : MinigameCompletion {
    public KeyCode key;
    bool active = false;
    GameObject note;

    void Start() {

    }

    void Update() {
        if (Input.GetKeyDown(key) && active) {
            if (note.transform.position.x <= 0.2f && note.transform.position.x >= -0.2f)
            {
                Destroy(note);
                minigameResult.MinigameResult(true);
            }

        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        active = true;
        if (col.gameObject.tag == "Note") {
            note = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        active = false;
    }
}
