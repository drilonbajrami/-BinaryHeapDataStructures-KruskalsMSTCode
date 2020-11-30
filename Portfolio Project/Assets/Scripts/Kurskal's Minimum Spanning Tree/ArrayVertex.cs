using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArrayVertex : MonoBehaviour
{
	private int index;
	private int parent;
	private int rank;

	public int Index { get { return index; } set { index = value; } }
	public int Parent { get { return parent; } set { parent = value; } }
	public int Rank { get { return rank; } set { rank = value; } }

	private TMP_Text indexText;
	private TMP_Text parentText;
	private TMP_Text rankText;

	void Start()
	{
		indexText = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
		parentText = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
		rankText = gameObject.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
	}

	void Update()
	{
		if (indexText != null)
			indexText.text = index.ToString();

		if (parentText != null)
			parentText.text = parent.ToString();

		if (rankText != null)
			rankText.text = rank.ToString();
	}

	public IEnumerator Blink(int i, float animationSpeed)
	{
		Color32 a = gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color;
		for (int p = 0; p < 6; p++)
		{
			if (p % 2 == 0)
			{
				gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
			}
			else
			{
				gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color = a;
			}
			yield return new WaitForSeconds(animationSpeed / 5);
		}
	}

	public void IsParent()
	{
		gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 40, 40, 255);
	}

	public void GetParentColor(Vertex vertex)
	{
		gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>().color = vertex.GetColor();
	}

	public void HighlightIndex(bool a)
	{
		if (a)
			gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 120, 120, 255);
		else
			gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
	}

	public void HighlightParent(bool a)
	{
		if (a)
			gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color32(255, 200, 125, 255);
		else
			gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
	}

	public void HighlightRank(bool a)
	{
		if (a)
			gameObject.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color32(255, 80, 255, 255);
		else
			gameObject.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
	}

	public void SameParent()
	{
		gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
	}

	public void DifferentParent()
	{
		gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color32(100, 255, 0, 255);
	}
}
