using UnityEngine;
using TMPro;

public class FancyDialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogue; 
    [SerializeField] private int wiggleStart;
    [SerializeField] private int wiggleEnd;
    [SerializeField] private int shakyStart;
    [SerializeField] private int shakyEnd;

    public float frequency = 2f; // Frequency of the wave
    public float amplitude = 5f; // Amplitude of the wave
    public float speed = 2f;     // Speed of the wave

    // Update is called once per frame
    void Update()
    {
        wiggleStart = dialogue.text.IndexOf("<wiggle>") + "<wiggle>".Length;
        wiggleEnd = dialogue.text.IndexOf("</wiggle>") - "</wiggle>".Length;
        shakyStart = dialogue.text.IndexOf("<shaky>") + "<shaky>".Length;
        shakyEnd = dialogue.text.IndexOf("</shaky>") - "</shaky>".Length;
    
        if (wiggleStart != -1) {
            createWiggle();
        }
        // if (shakyStart != -1) {
        //     createShaky();
        // }
    }

    private void createWiggle() {
        dialogue.ForceMeshUpdate();
        for (int i = wiggleStart; i < wiggleEnd; i++) {
            var characterInfo = dialogue.textInfo.characterInfo[i];
            if (!characterInfo.isVisible) {continue;}
            var vertices = dialogue.textInfo.meshInfo[characterInfo.materialReferenceIndex].vertices;
            for (int j = 0; j < 4; j++) {
                float offset = Mathf.Sin(Time.time * speed + i * frequency) * amplitude;
                vertices[characterInfo.vertexIndex + 0].y += offset;
                vertices[characterInfo.vertexIndex + 1].y += offset;
                vertices[characterInfo.vertexIndex + 2].y += offset;
                vertices[characterInfo.vertexIndex + 3].y += offset;
            }
        }
        // Update the mesh with modified vertices
        for (int i = 0; i < dialogue.textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = dialogue.textInfo.meshInfo[i];
            meshInfo.mesh.vertices = dialogue.textInfo.meshInfo[i].vertices;
            dialogue.UpdateGeometry(meshInfo.mesh, i);
        }
    }

}
