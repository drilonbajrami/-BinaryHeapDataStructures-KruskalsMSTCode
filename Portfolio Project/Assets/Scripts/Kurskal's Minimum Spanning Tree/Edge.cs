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
        if(b != null)
            points[1] = b.transform.position;
        line.SetPositions(points);

        _weightText.text = _weight.ToString();
        CenterText(points[0], points[1], 0.25f);
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
    }

	public int CompareTo(Edge other)
	{
        return Weight.CompareTo(other.Weight);
	}
}