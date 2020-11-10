using System;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    private int _value;
	[SerializeField]
    private Text textValue;

    public int Value { get { return _value; } set { _value = value; } }

	private void Start()
	{
		textValue = gameObject.transform.GetChild(2).GetChild(0).GetComponent<Text>();
	}

	private void Update()
	{
		if(textValue != null)
			textValue.text = _value.ToString();
	}
}
