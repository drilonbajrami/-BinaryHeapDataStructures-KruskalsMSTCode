using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArrayItem : MonoBehaviour
{
    private int index;
    private int value;

    public int Index { get { return index; } set { index = value; } }
    public int Value { get { return value; } set { this.value = value; } }

    private TMP_Text indexText;
    private TMP_Text valueText;

    void Start()
    {
        indexText = gameObject.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        valueText = gameObject.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (indexText != null)
            indexText.text = index.ToString();

        if (valueText != null)
            valueText.text = value.ToString();
    }

    public void ChangeColor(Color color)
    {
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
    }
}
