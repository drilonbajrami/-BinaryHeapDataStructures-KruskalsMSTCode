using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
    private Node parent;
    private Node child;

	public Node Parent { get { return parent; } set { parent = value; } }
	public Node Child { get { return child; } set { child = value; } }

	private void Update()
	{
		LineRenderer line = GetComponent<LineRenderer>();
		var points = new Vector3[2];
		if (parent != null)
			points[0] = parent.transform.position;
		if (child != null)
			points[1] = child.transform.position;
		line.SetPositions(points);
	}
}
