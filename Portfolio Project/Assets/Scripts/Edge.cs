using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    private Node a;
    private Node b;
    private int weight;

    public Node A { get { return a; } set { a = value; } }
    public Node B { get { return b; } set { b = value; } }
    public int Weight { get { return weight; } set { weight = value; } }

    void Update()
    {
        LineRenderer line = GetComponent<LineRenderer>();
        var points = new Vector3[2];
        if (a != null)
            points[0] = a.transform.position;
        if (b != null)
            points[1] = b.transform.position;
        line.SetPositions(points);
    }
}
