using UnityEngine;
using TMPro;

public class Node : MonoBehaviour
{
    private int _value;
	private TMP_Text _valueText;

    public int Value { get { return _value; } set { _value = value; } }

	private void Start()
	{
		_valueText = gameObject.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
	}

	private void Update()
	{
		if(_valueText != null)
			_valueText.text = _value.ToString();
	}

	public void ChangeColor(Color color)
	{
		gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;
	}
}
