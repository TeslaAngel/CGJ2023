using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextRowVisualizer : MonoBehaviour
{
    public List<TMP_Text> texts;
    public float VisualizeTime;

    // Start is called before the first frame update
    void Start()
    {
        //conceal all texts
        foreach (TMP_Text text in texts)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(texts.Count > 0)
        {
            texts[0].color = new Color(texts[0].color.r, texts[0].color.g, texts[0].color.b, texts[0].color.a + (1 / VisualizeTime)*Time.deltaTime);

            if (texts[0].color.a >= 1f)
            {
                texts.Remove(texts[0]);
            }
        }
        
    }
}
