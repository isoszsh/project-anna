using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class EnhancedText : MonoBehaviour
{
    public TMP_Text textComponent;
    public float colorChangeSpeed = 1.0f;

    public List<int> wobbleIndices = new List<int>();

    public List<int> colorChangeIndices = new List<int>();

    public List<int> jitterIndices = new List<int>();
     

    public void FindColorChangeIndeces(string text, List<int> colorChangeIndices)
    {
        MatchCollection colorMatches = Regex.Matches(text, "\'([^\']*)\'");
        foreach (Match match in colorMatches)
        {
            int startIndex = match.Index + 1;
            int endIndex = startIndex + match.Length - 3;
            for (int i = startIndex; i <= endIndex; i++)
            {
                colorChangeIndices.Add(i);
            }
        }
    }

    public void FindWobbleIndeces(string text, List<int> wobbleIndices)
    {
        MatchCollection wobbleMatches = Regex.Matches(text, "\"([^\"]*)\"");
        foreach (Match match in wobbleMatches)
        {
            int startIndex = match.Index +1 ;
            int endIndex = startIndex + match.Length -3;
            for (int i = startIndex; i <= endIndex; i++)
            {
                wobbleIndices.Add(i);
            }
        }
    }

    public void FindJitterIndeces(string text, List<int> jitterIndices)
    {
        MatchCollection jitterMatches = Regex.Matches(text, @"&([^&]*)&");
        foreach (Match match in jitterMatches)
        {
            int startIndex = match.Index + 1; // Skip the opening &
            int endIndex = startIndex + match.Length - 2; // Skip the closing &
            for (int i = startIndex; i < endIndex; i++)
            {
                jitterIndices.Add(i);
            }
        }
    }

    public void Start()
    {
        textComponent.ForceMeshUpdate();
        string text = textComponent.text;
        FindColorChangeIndeces(text, colorChangeIndices);
        FindWobbleIndeces(text, wobbleIndices);
        FindJitterIndeces(text, jitterIndices);
    }

    void Update()
    {
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;

        // Get the text and find the indices of the substrings within "" and ''
        string text = textComponent.text;

        Color32 newColor = new Color32(
            (byte)(Mathf.Sin(Time.time * colorChangeSpeed) * 127 + 128),
            (byte)(Mathf.Sin(Time.time * colorChangeSpeed + 2) * 127 + 128),
            (byte)(Mathf.Sin(Time.time * colorChangeSpeed + 4) * 127 + 128),
            255
        );

        System.Random random = new System.Random();

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }

            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;

            var verts = textInfo.meshInfo[materialIndex].vertices;
            Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;

            if (wobbleIndices.Contains(i))
            {
                for (int j = 0; j < 4; j++)
                {
                    var orig = verts[vertexIndex + j];
                    verts[vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * 10f, 0);
                }
            }

            if (colorChangeIndices.Contains(i))
            {
                for (int j = 0; j < 4; j++)
                {
                    vertexColors[vertexIndex + j] = newColor;
                }
            }

            if (jitterIndices.Contains(i))
            {
                Vector3 randomDirection = new Vector3(
                    (float)(random.NextDouble() * 2.0 - 1.0),
                    (float)(random.NextDouble() * 2.0 - 1.0),
                    0
                ).normalized;

                for (int j = 0; j< 4; j++)
                {
                    var orig = verts[vertexIndex + j];
                    verts[vertexIndex + j] = orig + randomDirection * Mathf.Sin(Time.time * 25f) * 2f;
                }
            }

            if (text[i] == '"' || text[i] == '\'' || text[i] == '&')
            {
                for (int j = 0; j < 4; j++)
                {
                    vertexColors[vertexIndex + j].a = 0; 
                }
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            meshInfo.mesh.colors32 = meshInfo.colors32;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
