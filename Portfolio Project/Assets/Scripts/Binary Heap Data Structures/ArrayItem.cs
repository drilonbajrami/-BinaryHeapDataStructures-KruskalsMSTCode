using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArrayItem : MonoBehaviour
{
    private int _index;
    private int _value;

    public int Index { get { return _index; } set { _index = value; } }
    public int Value { get { return _value; } set { _value = value; } }

    private TMP_Text indexText;
    private TMP_Text valueText;

    void Start()
    {
        indexText = gameObject.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        valueText = gameObject.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (indexText != null)
            indexText.text = _index.ToString();

        if (valueText != null)
            valueText.text = _value.ToString();
    }

    public void ChangeColor(Color color)
    {
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
    }
}
