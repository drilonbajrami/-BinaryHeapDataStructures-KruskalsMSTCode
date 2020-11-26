using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KruskalMST : MonoBehaviour
{
	private Color32[] colors = {
		new Color32(217, 131, 255, 255),
		new Color32(90, 214, 255, 255),
		new Color32(200, 255, 90, 255),
		new Color32(255, 231, 59, 255),
		new Color32(116, 255, 90, 255), 
	};

	private float animationSpeed = 1.0f;
	private int size = 10;

	private bool working = false;
	private bool graphCreated = false;

	int o; // Number of edges created

	private Vertex[] vertices;
	private Edge[] edges;
	private List<Vector2> usedPoints;

	public Vertex vertexPrefab;
	public Edge edgePrefab;

	private void Update()
	{
		UpdateVerticesColors();
	}

	// Animation speed slider
	public void OnValueChanged(float newValue)
	{
		animationSpeed = newValue;
	}

	public void RunKruskals()
	{
		StartCoroutine(KruskalsMST());
	}

	public void GenerateGraph()
	{
		if (graphCreated == false)
		{
			CreateGraph();
		}
		else
		{
			ClearGraph();
			CreateGraph();
		}
	}

	//================================================== Kruskal's MST Operations ==================================================//
	private IEnumerator KruskalsMST()
	{
		if (working == false)
		{
			working = true;
			Array.Sort(edges, 0, o);

			int i = 0;
			int e = 0;
			int c = 0;

			while (e < size - 1)
			{
				if (edges[i] != null)
				{
					edges[i].ChangeColor(Color.red);
					yield return new WaitForSeconds(animationSpeed);

					int x = Find(edges[i].A.Index);
					int y = Find(edges[i].B.Index);

					if (x != y)
					{
						edges[i].ChangeColor(new Color32(134, 255, 134, 255));
						edges[i].GetComponent<LineRenderer>().widthMultiplier = 1.1f;
						edges[i].GetComponent<LineRenderer>().sortingOrder = -1;

						if (edges[i].A.GetColor().Equals(new Color32(255, 255, 255, 255)) && edges[i].B.GetColor().Equals(new Color32(255, 255, 255, 255)))
						{
							edges[i].A.ChangeColor(colors[c]);
							edges[i].B.ChangeColor(colors[c]);
							c++;
						}
						Union(x, y);
						yield return new WaitForSeconds(animationSpeed);
						e++;
					}
					else
						edges[i].ChangeColor(new Color32(200, 200, 200, 255));

					i++;
				}
				else
					i++;
			}

			for (int a = 0; a < o; a++)
			{
				if (edges[a] != null && edges[a].GetColor().Equals(new Color32(200, 200, 200, 255)))
					DestroyImmediate(edges[a].gameObject);
			}
			working = false;
		}
	}

	private int Find(int i)
	{
		if (vertices[i].Parent != i)
		{
			vertices[i].Parent = Find(vertices[i].Parent);
		}
		return vertices[i].Parent;
	}

	private void Union(int x, int y)
	{
		if (vertices[x].Rank > vertices[y].Rank)
		{
			vertices[y].Parent = x;
		}
		else if (vertices[x].Rank < vertices[y].Rank)
		{
			vertices[x].Parent = y;
		}
		else
		{
			vertices[y].Parent = x;
			vertices[x].Rank++;
		}
	}

	//================================================== Tree Graph Operations ==================================================//
	public void CreateGraph()
	{
		vertices = new Vertex[size];
		edges = new Edge[size * (size - 1) / 2];
		usedPoints = new List<Vector2>();

		SpawnVertices(size);
		SpawnEdges(size);
		CheckIfAllVerticesAreConnected();
		graphCreated = true;
	}

	public void ClearGraph()
	{
		StopAllCoroutines();
		working = false;
		graphCreated = false;
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

	private void UpdateVerticesColors()
	{
		if (graphCreated)
		{
			for (int i = 0; i < size; i++)
				if (vertices[i] != null)
					vertices[i].GetParentColor(vertices[vertices[i].Parent]);
		}
	}

	private void SpawnEdges(int size)
	{
		o = 0;
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				if (i == j)
					continue;
				else
				{
					if ((vertices[i].transform.position - vertices[j].transform.position).sqrMagnitude < 15)
					{
						if (vertices[i].connectedVertices.Contains(vertices[j]))
							continue;
						else
						{
							edges[o] = Instantiate(edgePrefab);
							edges[o].A = vertices[i];
							edges[o].B = vertices[j];

							bool intersects = false;

							for (int a = 0; a < o; a++)
							{
								if (edges[o].Intersects(edges[a]))
								{
									intersects = true;
									break;
								}
							}

							if (intersects)
							{
								DestroyImmediate(edges[o].gameObject);
								edges[o] = null;
							}
							else
							{
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

		if (o < 15)
		{
			ClearGraph();
			CreateGraph();
		}
	}
}