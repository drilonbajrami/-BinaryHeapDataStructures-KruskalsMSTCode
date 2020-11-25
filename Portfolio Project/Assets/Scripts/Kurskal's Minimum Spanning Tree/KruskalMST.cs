using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KruskalMST : MonoBehaviour
{
	[Range(1, 10)]
	public int size;

	private Vertex[] vertices;
	private Edge[] edges;
	private List<Vector2> usedPoints;

	public Vertex vertexPrefab;
	public Edge edgePrefab;

	private void Start()
	{
		CreateGraph();
	}

	private void CreateGraph()
	{
		vertices = new Vertex[size];
		edges = new Edge[size * (size - 1) / 2];
		usedPoints = new List<Vector2>();

		SpawnVertices(size);
		SpawnEdges(size);
		CheckIfAllVerticesAreConnected();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			KruskalsMST();
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			ClearGraph();
			CreateGraph();
		}
	}

	private void KruskalsMST()
	{
		Debug.Log("Running Kruskal's Algorithm");
		Array.Sort(edges);

		int i = 0;
		int e = 0;

		while (e < size - 1)
		{
			if (edges[i] != null)
			{
				int x = Find(edges[i].A.Index);
				int y = Find(edges[i].B.Index);

				if (x != y)
				{
					edges[i].ChangeColor(Color.red);
					edges[i].GetComponent<LineRenderer>().widthMultiplier = 1.5f;
					edges[i].GetComponent<LineRenderer>().sortingOrder = -1;
					Union(x, y);
					e++;
				}

				i++;
			}
			else
			{
				i++;
			}
		}

		//for (int e = 0; e < edges.Length; e++)
		//{
		//	if (edges[e] != null)
		//	{
		//		int x = Find(edges[e].A.Index);
		//		int y = Find(edges[e].B.Index);

		//		if (x != y)
		//		{
		//			edges[e].ChangeColor(Color.red);
		//			Union(x, y);
		//		}
		//	}
		//}
	}

	public int Find(int i)
	{
		if (vertices[i].Parent != i)
			vertices[i].Parent = Find(vertices[i].Parent);

		return vertices[i].Parent;
	}

	public void Union(int x, int y)
	{
		if (vertices[x].Rank > vertices[y].Rank)
			vertices[y].Parent = x;
		else if (vertices[x].Rank < vertices[y].Rank)
			vertices[x].Parent = y;
		else
		{
			vertices[y].Parent = x;
			vertices[x].Rank++;
		}
	}

	private void SpawnVertices(int size)
	{
		SetPositions();
		for (int i = 0; i < size; i++)
		{
			vertices[i] = Instantiate(vertexPrefab);
			vertices[i].transform.position = usedPoints[i];
			vertices[i].transform.parent = gameObject.transform;
			vertices[i].Index = i;
			vertices[i].Rank = 0;
			vertices[i].Parent = i;
		}
	}

	private void SpawnEdges(int size)
	{
		int o = 0;
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				if (i == j)
					continue;
				else
				{
					if ((vertices[i].transform.position - vertices[j].transform.position).sqrMagnitude < 18)
					{
						if (vertices[i].connectedVertices.Contains(vertices[j]))
							continue;
						else
						{
							edges[o] = Instantiate(edgePrefab);
							edges[o].A = vertices[i];
							edges[o].B = vertices[j];
							edges[o].transform.parent = gameObject.transform;
							edges[o].Weight = UnityEngine.Random.Range(1, 20);
							vertices[i].connectedVertices.Add(vertices[j]);
							vertices[j].connectedVertices.Add(vertices[i]);
							vertices[i].Connected = true;
							vertices[j].Connected = true;
							o++;
						}
					}
				}
			}
		}
	}

	private void ClearGraph()
	{
		for (int i = 0; i < edges.Length; i++)
			if (edges[i] != null)
				DestroyImmediate(edges[i].gameObject);

		for (int i = 0; i < vertices.Length; i++)
			if (vertices[i] != null)
				DestroyImmediate(vertices[i].gameObject);

		usedPoints.Clear();
		Array.Clear(edges, 0, edges.Length);
		Array.Clear(vertices, 0, vertices.Length);
	}

	private void CheckIfAllVerticesAreConnected()
	{
		for (int i = 0; i < size; i++)
		{
			if (vertices[i].Connected == false)
			{
				ClearGraph();
				CreateGraph();
			}
		}
	}

	private void SetPositions()
	{
		for (int i = 0; i < size;)
		{
			Vector2 point = new Vector2(UnityEngine.Random.Range(-5.0f, 5.0f), UnityEngine.Random.Range(-3.0f, 3.0f));

			if (usedPoints.Count == 0)
			{
				usedPoints.Add(point);
				i++;
				continue;
			}

			for (int j = 0; j < usedPoints.Count; j++)
			{
				if ((point - usedPoints[j]).sqrMagnitude > 3)
				{
					if (j == usedPoints.Count - 1)
					{
						usedPoints.Add(point);
						i++;
					}
					continue;
				}
				break;
			}
		}
	}

	private void MaybeIWillUseThisLaterIDK()
	{
		//if (Input.GetMouseButtonDown(0))
		//{
		//	Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		//	hit = Physics2D.Raycast(pos, Vector2.zero);

		//	if (hit.collider != null && startNode == null)
		//	{
		//		startNode = hit.collider.gameObject.GetComponent<Node>();
		//	}
		//	else
		//	{
		//		Node node;
		//		node = Instantiate(nodePrefab, pos, Quaternion.identity);
		//		node.Value = UnityEngine.Random.Range(0, 20);
		//		node.transform.position = pos;
		//		node.transform.parent = gameObject.transform;
		//	}
		//}

		//if (Input.GetMouseButtonDown(1))
		//{
		//	Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		//	hit = Physics2D.Raycast(pos, Vector2.zero);

		//	if (hit.collider != null)
		//	{
		//		if (startNode != null)
		//		{
		//			endNode = hit.collider.gameObject.GetComponent<Node>();
		//			Edge e;
		//			e = Instantiate(edgePrefab, Vector2.zero, Quaternion.identity);
		//			e.A = startNode;
		//			e.B = endNode;
		//			e.transform.parent = gameObject.transform;
		//			startNode = null;
		//			endNode = null;
		//		}
		//		else
		//		{
		//			startNode = null;
		//			DestroyImmediate(hit.collider.gameObject);
		//		}
		//	}
		//}
	}
}
