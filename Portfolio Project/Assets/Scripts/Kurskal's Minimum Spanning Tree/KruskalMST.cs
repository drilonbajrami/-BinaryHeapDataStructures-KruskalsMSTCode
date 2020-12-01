using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
	private bool showTable = true;
	private float tableAnimationSpeed;

	int o; // Number of edges created

	private Vertex[] vertices;
	private Edge[] edges;
	private List<Vector2> usedPoints;

	public Vertex vertexPrefab;
	public Edge edgePrefab;

	private ArrayVertexBox arrayBox;
	private ArrayEdgeBox arrayEdgeBox;

	private int MSTCost;
	[SerializeField]
	private TMP_Text mstCostText;

	private void Update()
	{
		UpdateVerticesColors();
		TableAnimationSpeedSwitch();
	}

	// Animation speed slider
	public void OnValueChanged(float newValue)
	{
		animationSpeed = newValue;
	}

	public void OnValueChange(bool show)
	{
		showTable = show;

		if (showTable)
			Camera.main.cullingMask = -1;
		else
			Camera.main.cullingMask = 55;
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
			CreateArrayEdgeBox();

			int i = 0;
			int e = 0;
			int c = 0;

			while (e < size - 1)
			{
				if (edges[i] != null)
				{
					int A = edges[i].A.Index;
					int B = edges[i].B.Index;

					edges[i].ChangeColor(Color.red);
					arrayEdgeBox.Items[i].E.ChangeLineColor(Color.red);

					arrayBox.Items[A].HighlightIndex(true);
					arrayBox.Items[B].HighlightIndex(true);
					yield return new WaitForSeconds(animationSpeed / 2);
					arrayBox.Items[A].HighlightParent(true);
					arrayBox.Items[B].HighlightParent(true);
					yield return new WaitForSeconds(tableAnimationSpeed);

					int x = Find(A);
					int y = Find(B);

					if (x != y)
					{
						edges[i].ChangeColor(new Color32(134, 255, 134, 255));
						arrayEdgeBox.Items[i].E.ChangeLineColor(new Color32(134, 255, 134, 255));
						edges[i].GetComponent<LineRenderer>().widthMultiplier = 1.1f;
						edges[i].GetComponent<LineRenderer>().sortingOrder = -1;

						arrayBox.Items[A].DifferentParent();
						arrayBox.Items[B].DifferentParent();
						StartCoroutine(arrayBox.Items[A].Blink(1, tableAnimationSpeed));
						yield return StartCoroutine(arrayBox.Items[B].Blink(1, tableAnimationSpeed));

						if (x == A && y == B)
						{
							StartCoroutine(arrayBox.Items[A].Blink(0, tableAnimationSpeed));
							yield return StartCoroutine(arrayBox.Items[B].Blink(0, tableAnimationSpeed));
						}
						else if (x != A && y == B)
						{
							arrayBox.Items[x].IsParent();
							StartCoroutine(arrayBox.Items[A].Blink(1, tableAnimationSpeed));
							yield return StartCoroutine(arrayBox.Items[x].Blink(0, tableAnimationSpeed));
							arrayBox.Items[A].HighlightParent(false);
							yield return StartCoroutine(arrayBox.Items[B].Blink(0, tableAnimationSpeed));
							arrayBox.Items[x].HighlightParent(true);
							arrayBox.Items[A].HighlightParent(false);
						}
						else if (x == A && y != B)
						{
							arrayBox.Items[y].IsParent();
							StartCoroutine(arrayBox.Items[B].Blink(1, tableAnimationSpeed));
							yield return StartCoroutine(arrayBox.Items[y].Blink(0, tableAnimationSpeed));
							arrayBox.Items[B].HighlightParent(false);
							yield return StartCoroutine(arrayBox.Items[A].Blink(0, tableAnimationSpeed));
							arrayBox.Items[y].HighlightParent(true);
							arrayBox.Items[B].HighlightParent(false);
						}
						else
						{
							arrayBox.Items[x].IsParent();
							StartCoroutine(arrayBox.Items[A].Blink(1, tableAnimationSpeed));
							yield return StartCoroutine(arrayBox.Items[x].Blink(0, tableAnimationSpeed));
							arrayBox.Items[A].HighlightParent(false);

							arrayBox.Items[y].IsParent();
							StartCoroutine(arrayBox.Items[B].Blink(1, tableAnimationSpeed));
							yield return StartCoroutine(arrayBox.Items[y].Blink(0, tableAnimationSpeed));
							arrayBox.Items[B].HighlightParent(false);

							arrayBox.Items[x].HighlightParent(true);
							arrayBox.Items[y].HighlightParent(true);
							arrayBox.Items[A].HighlightParent(false);
							arrayBox.Items[B].HighlightParent(false);
						}

						if (edges[i].A.GetColor().Equals(new Color32(255, 255, 255, 255)) && edges[i].B.GetColor().Equals(new Color32(255, 255, 255, 255)))
						{
							edges[i].A.ChangeColor(colors[c]);
							edges[i].B.ChangeColor(colors[c]);
							c++;
						}
						yield return new WaitForSeconds(animationSpeed / 2);
						Coroutine a = StartCoroutine(Union(x, y));
						yield return a;
						yield return new WaitForSeconds(tableAnimationSpeed / 2);

						// Reset VertexBox colors
						arrayBox.Items[x].HighlightIndex(false);
						arrayBox.Items[x].HighlightParent(false);
						arrayBox.Items[x].HighlightRank(false);
						arrayBox.Items[y].HighlightIndex(false);
						arrayBox.Items[y].HighlightParent(false);
						arrayBox.Items[y].HighlightRank(false);
						arrayBox.Items[A].HighlightIndex(false);
						arrayBox.Items[B].HighlightIndex(false);
						arrayBox.Items[A].HighlightParent(false);
						arrayBox.Items[B].HighlightParent(false);
						arrayBox.Items[A].HighlightRank(false);
						arrayBox.Items[B].HighlightRank(false);
						DestroyImmediate(arrayEdgeBox.Items[i].gameObject);
						yield return new WaitForSeconds(animationSpeed / 2);
						MSTCost += edges[i].Weight;
						e++;
					}
					else
					{
						arrayBox.Items[A].SameParent();
						arrayBox.Items[B].SameParent();
						yield return new WaitForSeconds(tableAnimationSpeed / 2);
						edges[i].ChangeColor(new Color32(200, 200, 200, 255));

						// Reset VertexBox colors
						arrayBox.Items[A].HighlightIndex(false);
						arrayBox.Items[B].HighlightIndex(false);
						arrayBox.Items[A].HighlightParent(false);
						arrayBox.Items[B].HighlightParent(false);
						DestroyImmediate(arrayEdgeBox.Items[i].gameObject);
						yield return new WaitForSeconds(animationSpeed / 2);
						
					}
					i++;
				}
				else
					i++;
			}

			mstCostText.text = "MST Cost: " + MSTCost.ToString();

			for (int a = 0; a < o; a++)
			{
				if (edges[a] != null && edges[a].GetColor().Equals(new Color32(200, 200, 200, 255)))
					DestroyImmediate(edges[a].gameObject);

				if (arrayEdgeBox.Items[a] != null)
					DestroyImmediate(arrayEdgeBox.Items[a].gameObject);
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

	private IEnumerator Union(int x, int y)
	{
		if (showTable)
		{
			arrayBox.Items[x].HighlightRank(true);
			arrayBox.Items[y].HighlightRank(true);
			yield return new WaitForSeconds(tableAnimationSpeed / 2);
		}

		if (vertices[x].Rank > vertices[y].Rank)
		{
			yield return StartCoroutine(arrayBox.Items[y].Blink(2, tableAnimationSpeed));
			yield return StartCoroutine(arrayBox.Items[y].Blink(1, tableAnimationSpeed));
			vertices[y].Parent = x;
			arrayBox.Items[y].Parent = x;
		}
		else if (vertices[x].Rank < vertices[y].Rank)
		{
			yield return StartCoroutine(arrayBox.Items[x].Blink(2, tableAnimationSpeed));
			yield return StartCoroutine(arrayBox.Items[x].Blink(1, tableAnimationSpeed));
			vertices[x].Parent = y;
			arrayBox.Items[x].Parent = y;
		}
		else
		{
			yield return StartCoroutine(arrayBox.Items[y].Blink(1, tableAnimationSpeed));
			vertices[y].Parent = x;
			arrayBox.Items[y].Parent = x;
			yield return StartCoroutine(arrayBox.Items[x].Blink(2, tableAnimationSpeed));
			vertices[x].Rank++;
			arrayBox.Items[x].Rank++;
		}
	}

	//================================================== Tree Graph Operations ==================================================//
	public void CreateGraph()
	{
		MSTCost = 0;
		mstCostText.text = "";
		vertices = new Vertex[size];
		edges = new Edge[size * (size - 1) / 2];
		usedPoints = new List<Vector2>();

		arrayBox = gameObject.transform.GetChild(1).GetComponent<ArrayVertexBox>();
		arrayEdgeBox = gameObject.transform.GetChild(2).GetComponent<ArrayEdgeBox>();
		arrayBox.Items = new ArrayVertex[size];

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
			{
				DestroyImmediate(vertices[i].gameObject);
				DestroyImmediate(arrayBox.Items[i].gameObject);
			}

		if (arrayEdgeBox.Items != null)
		{
			for (int i = 0; i < arrayEdgeBox.Items.Length; i++)
				if (arrayEdgeBox.Items[i] != null)
				{
					DestroyImmediate(arrayEdgeBox.Items[i].gameObject);
				}
			Array.Clear(arrayEdgeBox.Items, 0, arrayEdgeBox.Items.Length);
		}

		usedPoints.Clear();
		Array.Clear(edges, 0, edges.Length);
		Array.Clear(vertices, 0, vertices.Length);
		Array.Clear(arrayBox.Items, 0, arrayBox.Items.Length);
		
	}

	private void SetPositions()
	{
		for (int i = 0; i < size;)
		{
			Vector2 point = new Vector2(UnityEngine.Random.Range(-5.0f, 7.0f), UnityEngine.Random.Range(-2.5f, 3.0f));

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

		for (int i = 0; i < size; i++)
		{
			ArrayVertex a = Instantiate(arrayBox.arrayVertexPrefab);
			a.transform.position = new Vector3(arrayBox.gameObject.transform.position.x + i * a.transform.localScale.x, arrayBox.gameObject.transform.position.y);
			a.transform.parent = arrayBox.gameObject.transform;
			a.Index = vertices[i].Index;
			a.Parent = vertices[i].Parent;
			a.Rank = vertices[i].Rank;
			arrayBox.Items[i] = a;
		}
	}

	private void UpdateVerticesColors()
	{
		if (graphCreated)
		{
			for (int i = 0; i < size; i++)
				if (vertices[i] != null)
				{
					vertices[i].GetParentColor(vertices[vertices[i].Parent]);
					arrayBox.Items[i].GetParentColor(vertices[vertices[i].Parent]);
				}
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

	private void TableAnimationSpeedSwitch()
	{
		if (showTable)
		{
			tableAnimationSpeed = animationSpeed;
		}
		else
		{
			tableAnimationSpeed = 0.0f;
		}
	}

	private void CreateArrayEdgeBox()
	{
		arrayEdgeBox.Items = new ArrayEdge[o];
		for (int i = 0; i < o; i++)
		{
			ArrayEdge a = Instantiate(arrayEdgeBox.arrayEdgePrefab);
			a.transform.parent = gameObject.transform.GetChild(2).transform;
			a.transform.position = new Vector3(a.transform.parent.position.x, a.transform.parent.position.y - i * arrayEdgeBox.gameObject.transform.localScale.y / 2.5f, 0);
			a.E.Weight = edges[i].Weight;
			a.E.A.Index = edges[i].A.Index;
			a.E.B.Index = edges[i].B.Index;
			arrayEdgeBox.Items[i] = a;
		}
	}
}