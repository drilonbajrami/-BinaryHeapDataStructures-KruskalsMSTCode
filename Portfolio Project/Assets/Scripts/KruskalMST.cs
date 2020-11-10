using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KruskalMST : MonoBehaviour
{
    Node[] nodes;

    public Node nodePrefab;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
			Node node;
			node = Instantiate(nodePrefab, pos, Quaternion.identity);
			node.Value = UnityEngine.Random.Range(0, 20);
			node.transform.position = pos;
			node.transform.parent = gameObject.transform;
		}

		if (Input.GetMouseButtonDown(1))
		{
			Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
			RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

			if (hit.collider != null)
			{
				DestroyImmediate(hit.collider.gameObject);
			}
		}
	}
}
