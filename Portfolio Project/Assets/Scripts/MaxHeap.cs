using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxHeap : MonoBehaviour
{
	Color32 RED = new Color32(255, 50, 50, 255);
	Color32 GREEN = new Color32(50, 255, 50, 255);
	Color32 BLUE = new Color32(50, 255, 50, 255);
	Color32 YELLOW = new Color32(255, 255, 50, 255);
	Color32 TRANSPARENT = new Color32(0, 0, 0, 0);

	float animationSpeed = 1.0f;
	bool working = false;
	public string value;

	private int capacity = 31;
	[Range(1, 31)]
	public int size;
	private int gens = 0;

	Node[] items;
	List<Connection> connections;

	public Node nodePrefab;
	public Connection connectionPrefab;

	private void OnEnable() {
		size = UnityEngine.Random.Range(5, 20);
		items = new Node[capacity];
		connections = new List<Connection>();
		addNodes();
		placeNodes();
		addConnections();
	}

	private void OnDisable() {
		for (int i = 0; i < size; i++)
			DestroyImmediate(items[i].gameObject);

		foreach (Connection c in connections)
			if (c != null)
				DestroyImmediate(c.gameObject);

		Array.Clear(items, 0, items.Length);
		connections.Clear();
		size = 0;
	}

	void Update() {
		value = FindObjectOfType<InputField>().text;
		GameObject.FindGameObjectWithTag("AnimationSpeedNumberMax").GetComponent<Text>().text = animationSpeed.ToString("0.00");

		for (int i = 0; i < connections.Count; i++)
			if (connections[i] != null)
				checkConnectionColor(connections[i]);
	}

	//============================================================================================================================//
	//================================================== Binary Heap operations ==================================================//
	//============================================================================================================================//
	int getLeftChildIndex(int parentIndex) { return parentIndex * 2 + 1; }
	int getRightChildIndex(int parentIndex) { return parentIndex * 2 + 2; }
	int getParentIndex(int childIndex) { return (childIndex - 1) / 2; }

	bool hasLeftChild(int index) { return getLeftChildIndex(index) < size; }
	bool hasRightChild(int index) { return getRightChildIndex(index) < size; }
	bool hasParent(int index) { return getParentIndex(index) >= 0; }

	IEnumerator swap(int indexA, int indexB) {
		// Change colors
		items[indexA].transform.GetChild(1).GetComponent<SpriteRenderer>().color = RED;
		items[indexB].transform.GetChild(1).GetComponent<SpriteRenderer>().color = RED;
		yield return new WaitForSeconds(animationSpeed);
		// Swap items
		int temp = items[indexA].Value;
		items[indexA].Value = items[indexB].Value;
		items[indexB].Value = temp;
		// Change colors
		items[indexA].transform.GetChild(1).GetComponent<SpriteRenderer>().color = GREEN;
		items[indexB].transform.GetChild(1).GetComponent<SpriteRenderer>().color = GREEN;
		yield return new WaitForSeconds(animationSpeed);
		// Change colors
		items[indexA].transform.GetChild(1).GetComponent<SpriteRenderer>().color = TRANSPARENT;
		items[indexB].transform.GetChild(1).GetComponent<SpriteRenderer>().color = TRANSPARENT;
	}

	public int peek() {
		if (items[0] != null)
			return items[0].Value;
		else 
			return 0;
	}

	// Calls insertCoroutine()
	public void insert() {
		StartCoroutine(insertCoroutine());
	}

	// Calls extractCoroutine()
	public void extract() {
		StartCoroutine(extractCoroutine());
	}

	IEnumerator insertCoroutine() {
		if (working == false && items.Length > 0) {
			working = true;
			if (size < capacity) {
				Node node;
				node = Instantiate(nodePrefab, UnityEngine.Random.insideUnitCircle, Quaternion.identity);
				if (value == "")
					node.Value = UnityEngine.Random.Range(0, 100);
				else
					node.Value = int.Parse(value);
				node.transform.parent = gameObject.transform;
				items[size] = node;
				size++;
				placeNodes();
				addConnection();
				Coroutine a = StartCoroutine(heapifyUp());
				yield return a;
			}
			working = false;
		}
	}

	IEnumerator extractCoroutine() {
		if (working == false) {
			working = true;
			if (size > 1) {
				// Change colors
				items[0].transform.GetChild(1).GetComponent<SpriteRenderer>().color = RED;
				items[size - 1].transform.GetChild(1).GetComponent<SpriteRenderer>().color = BLUE;
				yield return new WaitForSeconds(animationSpeed);
				items[0].Value = items[size - 1].Value;
				// Change colors
				items[0].transform.GetChild(1).GetComponent<SpriteRenderer>().color = BLUE;
				// Extract
				DestroyImmediate(items[size - 1].gameObject);
				items[size - 1] = null;
				DestroyImmediate(connections[connections.Count - 1].gameObject);
				connections.RemoveAt(connections.Count - 1);
				size--;
				yield return new WaitForSeconds(animationSpeed);
				// Change colors
				items[0].transform.GetChild(1).GetComponent<SpriteRenderer>().color = TRANSPARENT;
				yield return new WaitForSeconds(animationSpeed);
				Coroutine a = StartCoroutine(heapifyDown());
				yield return a;
			}
			else if (size == 1) {
				DestroyImmediate(items[0].gameObject);
				size--;
				yield return new WaitForSeconds(animationSpeed);
			}
			working = false;
		}
	}

	IEnumerator heapifyUp() {
		int index = size - 1;
		while (hasParent(index) && items[getParentIndex(index)].Value < items[index].Value) {
			Coroutine a = StartCoroutine(swap(getParentIndex(index), index));
			yield return a;
			index = getParentIndex(index);
		}
	}

	IEnumerator heapifyDown() {
		int index = 0;
		Coroutine a;
		while (hasLeftChild(index)) {
			// Change colors
			items[index].transform.GetChild(1).GetComponent<SpriteRenderer>().color = RED;
			items[getLeftChildIndex(index)].transform.GetChild(1).GetComponent<SpriteRenderer>().color = YELLOW;
			int largerChildIndex = getLeftChildIndex(index);
			yield return new WaitForSeconds(animationSpeed);

			if (hasRightChild(index) && items[getRightChildIndex(index)].Value > items[getLeftChildIndex(index)].Value) {
				// Change colors
				items[getLeftChildIndex(index)].transform.GetChild(1).GetComponent<SpriteRenderer>().color = TRANSPARENT;
				items[getRightChildIndex(index)].transform.GetChild(1).GetComponent<SpriteRenderer>().color = YELLOW;
				largerChildIndex = getRightChildIndex(index);
				yield return new WaitForSeconds(animationSpeed);
			}

			if (items[index].Value > items[largerChildIndex].Value) {
				// Change colors
				items[index].transform.GetChild(1).GetComponent<SpriteRenderer>().color = GREEN;
				items[largerChildIndex].transform.GetChild(1).GetComponent<SpriteRenderer>().color = GREEN;
				yield return new WaitForSeconds(animationSpeed);
				// Change colors
				items[index].transform.GetChild(1).GetComponent<SpriteRenderer>().color = TRANSPARENT;
				items[largerChildIndex].transform.GetChild(1).GetComponent<SpriteRenderer>().color = TRANSPARENT;
				break;
			}
			else
				a = StartCoroutine(swap(index, largerChildIndex));

			yield return a;
			index = largerChildIndex;
		}
	}

	// Animation speed slider
	public void OnValueChanged(float newValue) {
		animationSpeed = newValue;
	}

	// Value insert input
	public void OnValueChanged(string newValue) {
		value = newValue;
	}

	//============================================================================================================================//
	//================================================== Group "Helper methods" ==================================================//
	//============================================================================================================================//
	/// <summary>
	/// Sort the "items" array by node value
	/// </summary>
	void sortItems() {
		int[] tempItems = new int[size];
		for (int i = 0; i < size; i++)
			tempItems[i] = items[i].Value;

		Array.Sort(tempItems);
		Array.Reverse(tempItems);

		for (int i = 0; i < size; i++)
			items[i].Value = tempItems[i];
	}

	/// <summary>
	/// Adds nodes for creating a simple starting binary heap tree
	/// </summary>
	void addNodes() {
		for (int i = 0; i < size; i++) {
			Node node;
			node = Instantiate(nodePrefab, UnityEngine.Random.insideUnitCircle, Quaternion.identity);
			node.Value = UnityEngine.Random.Range(0, 100);
			node.transform.parent = gameObject.transform;
			items[i] = node;
		}
		sortItems();
	}

	/// <summary>
	/// Places the nodes evenly spaced in a binary tree formation
	/// depending on the number of node generations
	/// </summary>
	void placeNodes() {
		gens = getNumberOfGenerations();
		for (int i = 0; i < size; i++) {
			if (i == 0) items[i].transform.position = new Vector2(0, 2);
			else {
				Vector3 parentNode = items[getParentIndex(i)].transform.position;
				if (i % 2 != 0)
					items[i].transform.position = new Vector2(parentNode.x - halfWayFromParent(getInverseGeneration(i, gens)), parentNode.y - 1.5f);
				else
					items[i].transform.position = new Vector2(parentNode.x + halfWayFromParent(getInverseGeneration(i, gens)), parentNode.y - 1.5f);
			}
		}
	}

	/// <summary>
	/// Adds all the connections after the binary heap nodes are created
	/// </summary>
	void addConnections() {
		for (int i = 0; i < size; i++) {
			if (hasLeftChild(i)) {
				Connection c;
				c = Instantiate(connectionPrefab, Vector3.zero, Quaternion.identity);
				c.Parent = items[i];
				c.Child = items[getLeftChildIndex(i)];
				c.transform.parent = items[i].transform;
				connections.Add(c);

				if (hasRightChild(i)) {
					c = Instantiate(connectionPrefab, Vector3.zero, Quaternion.identity);
					c.Parent = items[i];
					c.Child = items[getRightChildIndex(i)];
					c.transform.parent = items[i].transform;
					connections.Add(c);
				}
			}
		}
	}

	/// <summary>
	/// Adds a new connection between the last added node and its parent node
	/// </summary>
	void addConnection() {
		Connection c;
		c = Instantiate(connectionPrefab, Vector3.zero, Quaternion.identity);
		c.Parent = items[getParentIndex(size - 1)];
		c.Child = items[size - 1];
		c.transform.parent = items[getParentIndex(size - 1)].transform;
		connections.Add(c);
	}

	//============================================================================================================================//
	//================================================== Group "Helper methods" ==================================================//
	//============================================================================================================================//
	/// <summary>
	/// Returns the number of generations of a binary heap tree.
	/// </summary>
	/// <returns></returns>
	int getNumberOfGenerations() {
		bool found = false;
		int generations = 0;
		for (int i = 0; i < size; i++) {
			if (found) break;
			if (size == 0) {
				generations = 0;
				found = true;
			} else if (size > Mathf.Pow(2, i) - 1 && size < Mathf.Pow(2, i + 1)) {
				generations = i + 1;
				found = true;
			}
		}
		return generations;
	}

	/// <summary>
	/// Returns the generation of a node.
	/// </summary>
	/// <param name="i"></param>
	/// <returns></returns>
	int getGeneration(int i) {
		int generation = 0;
		if (i == 0) generation = 1;
		else {
			for (int j = 2; j < size; j++) {
				if (i + 1 < Mathf.Pow(2, j)) {
					generation = j;
					break;
				}
			}
		}
		return generation;
	}

	/// <summary>
	/// Returns the inverse generation of a node.
	/// It is used for evenly placement of nodes in a binary tree shape.
	/// </summary>
	/// <param name="index"></param>
	/// <param name="generations"></param>
	/// <returns></returns>
	int getInverseGeneration(int index, int generations) {
		float middle = generations / 2 + 0.5f;
		int iG = 0;
		if (generations % 2 == 0)
			iG = (int)(middle + (middle - getGeneration(index)));
		else if (generations % 2 != 0)
			iG = (int)(middle + (middle - getGeneration(index)) + 1);
		return iG;
	}

	/// <summary>
	/// Returns a value (distance) which determines the placement of the child nodes
	/// in relation to the parent node's position.
	/// </summary>
	/// <param name="itemGeneration"></param>
	/// <returns></returns>
	float halfWayFromParent(int itemGeneration) {
		return Mathf.Pow(2, itemGeneration - 2 - 1) * 2;
	}

	/// <summary>
	/// For MIN HEAP
	/// If parent node value is smaller than child node value then color green (CORRECT ORDER)
	/// If parent node value is greater than child node value then color red (INCORRECT ORDER)
	/// </summary>
	/// <param name="c"></param>
	void checkConnectionColor(Connection c) {
		if (c.Parent.Value >= c.Child.Value) {
			c.GetComponent<LineRenderer>().startColor = Color.green;
			c.GetComponent<LineRenderer>().endColor = Color.green;
		} else {
			c.GetComponent<LineRenderer>().startColor = Color.red;
			c.GetComponent<LineRenderer>().endColor = Color.red;
		}
	}
}