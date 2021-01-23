using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinHeap : MonoBehaviour
{
	private float animationSpeed = 1.0f;
	private string value;

	private bool working = false;

	private const int CAPACITY = 31;
	private int size;

	private Node[] items;
	private List<Connection> connections;

	public Node nodePrefab;
	public Connection connectionPrefab;

	private ArrayBox arrayBox;

	private void OnEnable()
	{
		size = UnityEngine.Random.Range(10, 20);
		items = new Node[CAPACITY];
		connections = new List<Connection>();
		TreeUtils.AddNodes(this.gameObject, items, size, nodePrefab);
		TreeUtils.PlaceNodes(items, size);
		TreeUtils.AddConnections(connections, items, size, connectionPrefab);

		arrayBox = gameObject.transform.GetChild(0).GetComponent<ArrayBox>();
		arrayBox.Items = new ArrayItem[CAPACITY];
		for (int i = 0; i < size; i++)
		{
			ArrayItem a = Instantiate(arrayBox.arrayItemPrefab);
			a.transform.position = new Vector3(arrayBox.gameObject.transform.position.x + i * a.transform.localScale.x, arrayBox.gameObject.transform.position.y);
			a.transform.parent = arrayBox.gameObject.transform;
			a.Index = i;
			a.Value = items[i].Value;
			arrayBox.Items[i] = a;
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
		working = false;

		for (int i = 0; i < size; i++)
			if (items[i] != null)
				DestroyImmediate(items[i].gameObject);

		foreach (Connection c in connections)
			if (c != null)
				DestroyImmediate(c.gameObject);

		for (int i = 0; i < size; i++)
			if (arrayBox.Items[i] != null)
				DestroyImmediate(arrayBox.Items[i].gameObject);

		Array.Clear(items, 0, items.Length);
		Array.Clear(arrayBox.Items, 0, arrayBox.Items.Length);
		connections.Clear();
		size = 0;
	}

	private void Update()
	{
		TreeUtils.CheckConnectionsMin(connections);
	}

	// Animation speed slider
	public void OnValueChanged(float newValue)
	{
		animationSpeed = newValue;
	}

	// Value insert input
	public void OnValueChanged(string newValue)
	{
		value = newValue;
	}

	// Calls Peek Coroutine
	public void Peek()
	{
		StartCoroutine(PeekCoroutine());
	}

	// Calls Insert Coroutine 
	public void Insert()
	{
		StartCoroutine(InsertCoroutine());
	}

	// Calls Extract Coroutine
	public void Extract()
	{
		StartCoroutine(ExtractCoroutine());
	}

	// Calls Heapsort Coroutine
	public void HeapSort()
	{
		StartCoroutine(HeapSort(size));
	}

	//================================================== Binary Heap operations ==================================================//
	// Index functions
	private int GetLeftChildIndex(int parentIndex) { return parentIndex * 2 + 1; }
	private int GetRightChildIndex(int parentIndex) { return parentIndex * 2 + 2; }
	private int GetParentIndex(int childIndex) { return (childIndex - 1) / 2; }

	// Relation check functions
	private bool HasLeftChild(int index) { return GetLeftChildIndex(index) < size; }
	private bool HasRightChild(int index) { return GetRightChildIndex(index) < size; }
	private bool HasParent(int index) { return GetParentIndex(index) >= 0; }

	private IEnumerator PeekCoroutine()
	{
		if (working == false && size > 0)
		{
			working = true;
			items[0].ChangeColor(TreeUtils.YELLOW);
			arrayBox.Items[0].ChangeColor(TreeUtils.YELLOW);
			yield return new WaitForSeconds(animationSpeed / 2);
			items[0].ChangeColor(TreeUtils.TRANSPARENT);
			arrayBox.Items[0].ChangeColor(TreeUtils.TRANSPARENT);
			working = false;
		}
	}

	private IEnumerator Swap(int indexA, int indexB)
	{
		// Animation: Change colors
		items[indexA].ChangeColor(TreeUtils.RED);
		items[indexB].ChangeColor(TreeUtils.RED);
		arrayBox.Items[indexA].ChangeColor(TreeUtils.RED);
		arrayBox.Items[indexB].ChangeColor(TreeUtils.RED);
		yield return new WaitForSeconds(animationSpeed);

		// Swap items
		int temp = items[indexA].Value;
		items[indexA].Value = items[indexB].Value;
		items[indexB].Value = temp;
		arrayBox.Swap(indexA, indexB);

		// Animation: Change colors
		items[indexA].ChangeColor(TreeUtils.GREEN);
		items[indexB].ChangeColor(TreeUtils.GREEN);
		arrayBox.Items[indexA].ChangeColor(TreeUtils.GREEN);
		arrayBox.Items[indexB].ChangeColor(TreeUtils.GREEN);
		yield return new WaitForSeconds(animationSpeed);

		// Animation: Change colors
		items[indexA].ChangeColor(TreeUtils.TRANSPARENT);
		items[indexB].ChangeColor(TreeUtils.TRANSPARENT);
		arrayBox.Items[indexA].ChangeColor(TreeUtils.TRANSPARENT);
		arrayBox.Items[indexB].ChangeColor(TreeUtils.TRANSPARENT);
	}

	private IEnumerator InsertCoroutine()
	{
		// Check if there is any work being done on the heap at the moment
		if (working == false && size >= 0 && size < CAPACITY)
		{
			working = true;
			Node node = Instantiate(nodePrefab);
			ArrayItem arrayItem = Instantiate(arrayBox.arrayItemPrefab);

			// If there is no value input then randomize node's value
			if (value == null || value == "") 
				node.Value = UnityEngine.Random.Range(0, 100);
			else 
				node.Value = int.Parse(value);

			node.transform.parent = gameObject.transform;
			items[size] = node;

			arrayItem.Value = node.Value;
			arrayItem.Index = size;
			arrayItem.transform.position = new Vector3(arrayBox.gameObject.transform.position.x + size * arrayItem.transform.localScale.x, arrayBox.gameObject.transform.position.y);
			arrayItem.transform.parent = arrayBox.gameObject.transform;
			arrayBox.Items[size] = arrayItem;

			size++;

			// Update binary heap tree upon insertion
			TreeUtils.PlaceNodes(items, size);
			TreeUtils.AddConnection(connections, items, size, connectionPrefab);

			// Heapify up upon insertion
			Coroutine a = StartCoroutine(HeapifyUp());
			yield return a;

			// Operation complete, set working to false
			working = false;
		}
	}

	private IEnumerator ExtractCoroutine()
	{
		// Check if there is any work being done on the heap at the moment
		if (working == false)
		{
			working = true;
			if (size > 1)
			{
				// Animation: Change colors
				items[0].ChangeColor(TreeUtils.RED);
				items[size - 1].ChangeColor(TreeUtils.BLUE);
				arrayBox.Items[0].ChangeColor(TreeUtils.RED);
				arrayBox.Items[size - 1].ChangeColor(TreeUtils.BLUE);
				yield return new WaitForSeconds(animationSpeed);

				items[0].Value = items[size - 1].Value;
				arrayBox.Items[0].Value = arrayBox.Items[size - 1].Value;

				// Animation: Change color
				items[0].ChangeColor(TreeUtils.BLUE);
				arrayBox.Items[0].ChangeColor(TreeUtils.BLUE);

				// Extract first element
				DestroyImmediate(items[size - 1].gameObject);
				items[size - 1] = null;
				DestroyImmediate(arrayBox.Items[size - 1].gameObject);
				arrayBox.Items[size - 1] = null;

				DestroyImmediate(connections[connections.Count - 1].gameObject);
				connections.RemoveAt(connections.Count - 1);
				size--;
				yield return new WaitForSeconds(animationSpeed);

				// Animation: Change color
				items[0].ChangeColor(TreeUtils.TRANSPARENT);
				arrayBox.Items[0].ChangeColor(TreeUtils.TRANSPARENT);
				yield return new WaitForSeconds(animationSpeed);

				// Update binary heap tree upon extraction
				TreeUtils.PlaceNodes(items, size);

				// Heapify down upon extraction
				Coroutine a = StartCoroutine(HeapifyDown());
				yield return a;
			}
			else if (size == 1)
			{
				// Extract first element
				DestroyImmediate(items[0].gameObject);
				items[0] = null;
				DestroyImmediate(arrayBox.Items[0].gameObject);
				arrayBox.Items[0] = null;
				size--;
				yield return new WaitForSeconds(animationSpeed);
			}
			working = false;
		}
	}

	private IEnumerator HeapifyUp()
	{
		int index = size - 1;
		while (HasParent(index) && items[GetParentIndex(index)].Value > items[index].Value)
		{
			// If node has parent and parent's value is greater then swap
			Coroutine a = StartCoroutine(Swap(GetParentIndex(index), index));
			yield return a;
			index = GetParentIndex(index);
		}
	}

	private IEnumerator HeapifyDown()
	{
		int index = 0;
		Coroutine a;

		// Keep checking as long as current node has a left child
		while (HasLeftChild(index))
		{
			int l = GetLeftChildIndex(index);
			int r = GetRightChildIndex(index);

			// Animation: Change colors
			items[index].ChangeColor(TreeUtils.RED);
			items[l].ChangeColor(TreeUtils.YELLOW);
			arrayBox.Items[index].ChangeColor(TreeUtils.RED);
			arrayBox.Items[l].ChangeColor(TreeUtils.YELLOW);

			int smallerChildIndex = l;
			yield return new WaitForSeconds(animationSpeed);

			if (HasRightChild(index) && items[r].Value < items[l].Value)
			{
				// Animation: Change colors
				items[l].ChangeColor(TreeUtils.TRANSPARENT);
				items[r].ChangeColor(TreeUtils.YELLOW);
				smallerChildIndex = r;

				arrayBox.Items[l].ChangeColor(TreeUtils.TRANSPARENT);
				arrayBox.Items[r].ChangeColor(TreeUtils.YELLOW);
				yield return new WaitForSeconds(animationSpeed);
			}

			// If current node's value is smaller than child node's value then done
			if (items[index].Value < items[smallerChildIndex].Value)
			{
				// Animation: Change colors
				items[index].ChangeColor(TreeUtils.GREEN);
				items[smallerChildIndex].ChangeColor(TreeUtils.GREEN);
				arrayBox.Items[index].ChangeColor(TreeUtils.GREEN);
				arrayBox.Items[smallerChildIndex].ChangeColor(TreeUtils.GREEN);
				yield return new WaitForSeconds(animationSpeed);

				// Animation: Change colors
				items[index].ChangeColor(TreeUtils.TRANSPARENT);
				items[smallerChildIndex].ChangeColor(TreeUtils.TRANSPARENT);
				arrayBox.Items[index].ChangeColor(TreeUtils.TRANSPARENT);
				arrayBox.Items[smallerChildIndex].ChangeColor(TreeUtils.TRANSPARENT);
				break;
			}
			else
			{
				a = StartCoroutine(Swap(index, smallerChildIndex));
			}

			yield return a;
			index = smallerChildIndex;
		}
	}

	private IEnumerator HeapSort(int size)
	{
		working = true;
		for (int i = size / 2 - 1; i >= 0; i--)
		{
			Coroutine a = StartCoroutine(HeapifyMin(i, size));
			yield return a;
		}
		working = false;
	}

	private IEnumerator HeapifyMin(int index, int size)
	{
		int largest = index;
		int l = GetLeftChildIndex(index);
		int r = GetRightChildIndex(index);

		items[index].ChangeColor(TreeUtils.RED);
		arrayBox.Items[index].ChangeColor(TreeUtils.RED);
		yield return new WaitForSeconds(animationSpeed);

		if (l < size && items[largest].Value > items[l].Value)
		{
			largest = l;
			items[l].ChangeColor(TreeUtils.YELLOW);
			arrayBox.Items[l].ChangeColor(TreeUtils.YELLOW);
			yield return new WaitForSeconds(animationSpeed);
		}

		if (r < size && items[largest].Value > items[r].Value)
		{
			largest = r;
			items[l].ChangeColor(TreeUtils.TRANSPARENT);
			items[r].ChangeColor(TreeUtils.YELLOW);
			arrayBox.Items[l].ChangeColor(TreeUtils.TRANSPARENT);
			arrayBox.Items[r].ChangeColor(TreeUtils.YELLOW);
			yield return new WaitForSeconds(animationSpeed);
		}

		if (largest != index)
		{
			items[l].ChangeColor(TreeUtils.TRANSPARENT);
			arrayBox.Items[l].ChangeColor(TreeUtils.TRANSPARENT);
			if (r < size)
			{
				items[r].ChangeColor(TreeUtils.TRANSPARENT);
				arrayBox.Items[r].ChangeColor(TreeUtils.TRANSPARENT);
			}
			Coroutine a = StartCoroutine(Swap(index, largest));
			yield return a;
			Coroutine b = StartCoroutine(HeapifyMin(largest, size));
			yield return b;
		}

		items[index].ChangeColor(TreeUtils.GREEN);
		arrayBox.Items[index].ChangeColor(TreeUtils.GREEN);
		yield return new WaitForSeconds(animationSpeed);
		items[index].ChangeColor(TreeUtils.TRANSPARENT);
		arrayBox.Items[index].ChangeColor(TreeUtils.TRANSPARENT);
	}
}