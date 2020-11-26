using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Vertex : MonoBehaviour, IComparable<Vertex>
{
	private int _index;
	private int _parent;
	private int _rank;

	public int Index { get { return _index; } set { _index = value; } }
	public int Parent { get { return _parent; } set { _parent = value; } }
	public int Rank { get { return _rank; } set { _rank = value; } }

	private bool _connected;
	public bool Connected { get { return _connected; } set { _connected = value; } }

	public List<Vertex> connectedVertices = new List<Vertex>();
	private TMP_Text _indexText;

	private void Start()
	{
		_connected = false;
		_indexText = gameObject.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
	}

	private void Update()
	{
		if (_indexText != null)
			_indexText.text = _index.ToString();
	}

	public void ChangeColor(Color color)
	{
		gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
	}

	public void ChangeColor(Color32 color)
	{
		gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
	}

	public void GetParentColor(Vertex vertex)
	{
		gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = vertex.GetColor();
	}

	public Color32 GetColor()
	{
		return gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
	}

	public int CompareTo(Vertex other)
	{
		return Parent.CompareTo(other.Parent);
	}
}
