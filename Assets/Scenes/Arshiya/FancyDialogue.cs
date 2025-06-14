using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text;
public class FancyDialogue : MonoBehaviour
{
    //wiggly variables
    [SerializeField] private float frequency = 2f; 
    [SerializeField] private float amplitude = 5f; 
    [SerializeField] private  float speed = 2f;  

    //shaky variables
    [SerializeField] private  float jitterAmount = 2f;  
    private float jitterUpdateTime = 0.05f; // Time between jitter updates
    private float lastJitterTime = 0f;
    private Dictionary<int, Vector3> jitterOffsets = new Dictionary<int, Vector3>(); 

    //bold variables
    [SerializeField] private float boldOffset = 0.5f;
    [SerializeField] private float boldSpacingOffset = 0.5f;
    //italic variables
    [SerializeField] private float italicOffset = 0.5f;

    [SerializeField] private TMP_Text textMesh;
    private Vector3[] vertices;
    private TMP_TextInfo textInfo;

    private struct TagRanges {
        public int start;
        public int end;
    }

    private List<TagRanges> wiggleRanges = new List<TagRanges>();
    private List<TagRanges> shakyRanges = new List<TagRanges>();
    private List<TagRanges> boldRanges = new List<TagRanges>();
    private List<TagRanges> italicRanges = new List<TagRanges>();
    
    //private string originalText;
    private string finalText;
    void Start()
    {
        textInfo = textMesh.textInfo;
        // Debug.Log(textInfo);
        // originalText = textMesh.text;
        finalText = "";
    }

    public void Evaluate() {
        textInfo = textMesh.textInfo;
        // originalText = textMesh.text;
        finalText = "";
    }

    public void Reset() {
        wiggleRanges = new List<TagRanges>();
        shakyRanges = new List<TagRanges>();
        boldRanges = new List<TagRanges>();
        italicRanges = new List<TagRanges>();
    }

    void Update()
    {
        // textInfo = textMesh.textInfo;
        // //originalText = textMesh.text;
        // finalText = "";
        // textMesh.ClearMesh(false);
        parseTags(textMesh.text);
        textMesh.text = finalText;
        textMesh.ForceMeshUpdate();
        textInfo = textMesh.textInfo;
        Italic();
        Bold();
        Shake();
        Wiggle();
    }

    private void parseTags(string s) {
        StringBuilder parsedText = new StringBuilder();

        for (int i = 0; i < s.Length; i++) {
            // Preserve tabs and new lines
            if (s[i] == '\t' || s[i] == '\n') {
                parsedText.Append(s[i]);
                continue;
            }
            // Handle <wiggle> tag
            if (s.Substring(i).StartsWith("<wiggle>", System.StringComparison.OrdinalIgnoreCase)) {
                int start = parsedText.Length;
                i += "<wiggle>".Length;
                int endTagIndex = s.IndexOf("</wiggle>", i, System.StringComparison.OrdinalIgnoreCase);
                if (endTagIndex < 0) { parsedText.Append("<wiggle>"); continue; }

                while (i < endTagIndex) parsedText.Append(s[i++]);

                wiggleRanges.Add(new TagRanges { start = start, end = parsedText.Length - 1 });
                i = endTagIndex + "</wiggle>".Length - 1;
                continue;
            }

            // Handle <shaky> tag
            if (s.Substring(i).StartsWith("<shaky>", System.StringComparison.OrdinalIgnoreCase)) {
                int start = parsedText.Length;
                i += "<shaky>".Length;
                int endTagIndex = s.IndexOf("</shaky>", i, System.StringComparison.OrdinalIgnoreCase);
                if (endTagIndex < 0) { parsedText.Append("<shaky>"); continue; }

                while (i < endTagIndex) parsedText.Append(s[i++]);

                shakyRanges.Add(new TagRanges { start = start, end = parsedText.Length - 1 });
                i = endTagIndex + "</shaky>".Length - 1;
                continue;
            }

            // Handle <bold> tag
            if (s.Substring(i).StartsWith("<bold>", System.StringComparison.OrdinalIgnoreCase)) {
                int start = parsedText.Length;
                i += "<bold>".Length;
                int endTagIndex = s.IndexOf("</bold>", i, System.StringComparison.OrdinalIgnoreCase);
                if (endTagIndex < 0) { parsedText.Append("<bold>"); continue; }

                while (i < endTagIndex) parsedText.Append(s[i++]);

                boldRanges.Add(new TagRanges { start = start, end = parsedText.Length - 1 });
                i = endTagIndex + "</bold>".Length - 1;
                continue;
            }

            // Handle <italic> tag
            if (s.Substring(i).StartsWith("<italic>", System.StringComparison.OrdinalIgnoreCase)) {
                int start = parsedText.Length;
                i += "<italic>".Length;
                int endTagIndex = s.IndexOf("</italic>", i, System.StringComparison.OrdinalIgnoreCase);
                if (endTagIndex < 0) { parsedText.Append("<italic>"); continue; }

                while (i < endTagIndex) parsedText.Append(s[i++]);

                italicRanges.Add(new TagRanges { start = start, end = parsedText.Length - 1 });
                i = endTagIndex + "</italic>".Length - 1;
                // Debug.Log("Range is " + start + " to " + endTagIndex);
                continue;
            }

            // Append all other characters normally
            parsedText.Append(s[i]);
        }

        finalText = parsedText.ToString();
    }
    private void Shake() {
        for (int i = 0; i < textInfo.characterCount; i++) {
            TMP_CharacterInfo curChar = textInfo.characterInfo[i];
            if (!curChar.isVisible) {
                continue;
            }
            //check if our current character is in the shaky range
            foreach (var range in shakyRanges)
            {
                if (i >= range.start && i <= range.end)
                {
                    if (Time.time - lastJitterTime > jitterUpdateTime)
                    {
                        lastJitterTime = Time.time;
                        jitterOffsets.Clear(); // Reset previous jitter offsets

                        for (int j = 0; j < textInfo.characterCount; j++)
                        {
                            jitterOffsets[j] = new Vector3(Random.Range(-jitterAmount, jitterAmount), 
                                                        Random.Range(-jitterAmount, jitterAmount), 0);
                        }
                    }

                   
                    // Get the material and vertex indices for this character.
                    int materialIndex = curChar.materialReferenceIndex;
                    int vertexIndex = curChar.vertexIndex;
                    Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                     // Apply stored jitter values instead of calling Random.Range() every frame
                    Vector3 jitter = jitterOffsets.ContainsKey(i) ? jitterOffsets[i] : Vector3.zero;

                    vertices[vertexIndex + 0] += jitter;
                    vertices[vertexIndex + 1] += jitter;
                    vertices[vertexIndex + 2] += jitter;
                    vertices[vertexIndex + 3] += jitter;
                    break;
                }
            }
            
        }
        // Push the updated vertex data to the mesh.
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
    private void Wiggle() {
        for (int i = 0; i < textInfo.characterCount; i++) {
            TMP_CharacterInfo curChar = textInfo.characterInfo[i];
            if (!curChar.isVisible) {
                continue;
            }
            //check if our current character is in the wiggle range
            foreach (var range in wiggleRanges)
            {
                if (i >= range.start && i <= range.end)
                {
                    // Get the material and vertex indices for this character.
                    int materialIndex = curChar.materialReferenceIndex;
                    int vertexIndex = curChar.vertexIndex;
                    Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                    float offset = Mathf.Sin(Time.time * speed + i * frequency) * amplitude;
                    vertices[vertexIndex + 0].y += offset;
                    vertices[vertexIndex + 1].y += offset;
                    vertices[vertexIndex + 2].y += offset;
                    vertices[vertexIndex + 3].y += offset;
                    break;
                }
            }
        }
        // Push the updated vertex data to the mesh.
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
    private void Bold() {
        for (int i = 0; i < textInfo.characterCount; i++) {
            TMP_CharacterInfo curChar = textInfo.characterInfo[i];
            if (!curChar.isVisible) {
                continue;
            }

            foreach (var range in boldRanges) {
                if (i >= range.start && i <= range.end) {
                    int materialIndex = curChar.materialReferenceIndex;
                    int vertexIndex = curChar.vertexIndex;
                    Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                    // Expand character outward
                    vertices[vertexIndex + 0].x -= boldOffset;  // Bottom-left
                    vertices[vertexIndex + 1].x -= boldOffset;  // Top-left
                    vertices[vertexIndex + 2].x += boldOffset;  // Top-right
                    vertices[vertexIndex + 3].x += boldOffset;  // Bottom-right

                    // Ensure the last character in the range gets bold effect
                    if (i == range.end) {
                        vertices[vertexIndex + 0].x -= boldOffset;
                        vertices[vertexIndex + 1].x -= boldOffset;
                        vertices[vertexIndex + 2].x += boldOffset;
                        vertices[vertexIndex + 3].x += boldOffset;
                    }

                    // **Shift next character slightly forward to maintain spacing**
                    if (i + 1 <= range.end && i + 1 < textInfo.characterCount) {
                        TMP_CharacterInfo nextChar = textInfo.characterInfo[i + 1];
                        if (nextChar.isVisible) {
                            int nextVertexIndex = nextChar.vertexIndex;
                            Vector3[] nextVertices = textInfo.meshInfo[nextChar.materialReferenceIndex].vertices;

                            nextVertices[nextVertexIndex + 0].x += boldSpacingOffset;
                            nextVertices[nextVertexIndex + 1].x += boldSpacingOffset;
                            nextVertices[nextVertexIndex + 2].x += boldSpacingOffset;
                            nextVertices[nextVertexIndex + 3].x += boldSpacingOffset;
                        }
                    }
                }
            }
        }

        // Push the updated vertex data to the mesh.
        for (int i = 0; i < textInfo.meshInfo.Length; i++) {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }

    private void Italic() {
        //Debug.Log("Itallic Ranges are" + italicRanges);
        for (int i = 0; i < textInfo.characterCount; i++) {
            TMP_CharacterInfo curChar = textInfo.characterInfo[i];
            if (!curChar.isVisible) {
                continue;
            }
            //check if our current character is in the wiggle range
            foreach (var range in italicRanges)
            {
                if (i >= range.start && i <= range.end)
                {
                    int materialIndex = curChar.materialReferenceIndex;
                    int vertexIndex = curChar.vertexIndex;
                    Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
                    
                    vertices[vertexIndex + 0] += new Vector3(-2 * italicOffset, 0, 0);
                    vertices[vertexIndex + 1] += new Vector3(2 * italicOffset, 0, 0);
                    vertices[vertexIndex + 2] += new Vector3(2* italicOffset, 0, 0); 
                    vertices[vertexIndex + 3] += new Vector3(- 2 * italicOffset, 0, 0);  
                    break;
                }
            }
        }
        // Push the updated vertex data to the mesh.
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
