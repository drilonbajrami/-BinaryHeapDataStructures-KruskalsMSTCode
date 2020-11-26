using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Edge : MonoBehaviour, IComparable<Edge>
{
	private Vertex a;
	private Vertex b;
	private int _weight;

	public Vertex A { get { return a; } set { a = value; } }
	public Vertex B { get { return b; } set { b = value; } }
	public int Weight { get { return _weight; } set { _weight = value; } }

	private TMP_Text _weightText;

	private void Start()
	{
		_weightText = gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
	}

	void Update()
	{
		LineRenderer line = GetComponent<LineRenderer>();
		Vector3[] points = new Vector3[2];
		if (a != null)
			points[0] = a.transform.position;
		if (b != null)
			points[1] = b.transform.position;
		line.SetPositions(points);

		_weightText.text = _weight.ToString();
		CenterText(points[0], points[1], 0.2f);
	}

	private void CenterText(Vector2 startPoint, Vector2 endPoint, float distance)
	{
		Vector2 midPoint = new Vector2((startPoint.x + endPoint.x) / 2, (startPoint.y + endPoint.y) / 2);
		Vector2 normal = new Vector2(-(endPoint.y - startPoint.y), endPoint.x - startPoint.x);
		normal.Normalize();
		_weightText.transform.position = midPoint + distance * normal;
	}

	public void ChangeColor(Color32 color)
	{
		GetComponent<LineRenderer>().startColor = color;
		GetComponent<LineRenderer>().endColor = color;
		_weightText.color = color;
	}

	public Color32 GetColor()
	{
		return GetComponent<LineRenderer>().startColor;
	}

	public int CompareTo(Edge other)
	{
		return Weight.CompareTo(other.Weight);
	}

	public bool Intersects(Edge other)
	{
		// Part of the code below is taken from this thread https://stackoverflow.com/questions/47470449/how-to-find-coordinates-of-a-point-where-2-points-and-distance-are-given-with-ja
		float slope;
		float theta;
		float distance = 0.0001f;

		// This edge line segment resize
		Vector2 a1 = A.transform.position;
		Vector2 a2 = B.transform.position;
		slope = (a2.y - a1.y) / (a2.x - a1.x);
		theta = Mathf.Atan(slope);
		Vector2 p1 = new Vector2(a1.x + distance * Mathf.Cos(theta), a1.y + distance * Mathf.Sin(theta));
		Vector2 p2 = new Vector2(a2.x - distance * Mathf.Cos(theta), a2.y - distance * Mathf.Sin(theta));

		// Other edge line segment resize
		Vector2 b1 = other.A.transform.position;
		Vector2 b2 = other.B.transform.position;
		slope = (b2.y - b1.y) / (b2.x - b2.x);
		theta = Mathf.Atan(slope);
		Vector2 p3 = new Vector2(b1.x + distance * Mathf.Cos(theta), b1.y + distance * Mathf.Sin(theta));
		Vector2 p4 = new Vector2(b2.x - distance * Mathf.Cos(theta), b2.y - distance * Mathf.Sin(theta));

		// The part below is taken from user BAST42 on https://forum.unity.com/threads/line-intersection.17384/ Unity forum
		Vector2 a = p2 - p1;
		Vector2 b = p3 - p4;
		Vector2 c = p1 - p3;

		float alphaNumerator = b.y * c.x - b.x * c.y;
		float alphaDenominator = a.y * b.x - a.x * b.y;
		float betaNumerator = a.x * c.y - a.y * c.x;
		float betaDenominator = a.y * b.x - a.x * b.y;

		bool doIntersect = true;

		if (alphaDenominator == 0 || betaDenominator == 0)
		{
			doIntersect = false;
		}
		else
		{
			if (alphaDenominator > 0)
			{
				if (alphaNumerator < 0 || alphaNumerator > alphaDenominator)
				{
					doIntersect = false;
				}
			}
			else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator)
			{
				doIntersect = false;
			}

			if (doIntersect && betaDenominator > 0)
			{
				if (betaNumerator < 0 || betaNumerator > betaDenominator)
				{
					doIntersect = false;
				}
			}
			else if (betaNumerator > 0 || betaNumerator < betaDenominator)
			{
				doIntersect = false;
			}
		}

		return doIntersect;
	}
}