using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MinHeap : MonoBehaviour
{
	[Range(1, 31)]
	public int capacity;
	private int size = 0;
	private int gens = 0;

	Node[] items;
	List<Connection> connections;

	public Node nodePrefab;
	public Connection connectionPrefab;

	void Start()
	{
		items = new Node[capacity];
		connections = new List<Connection>();

		// Add nodes
		while (size < capacity)
		{
			Node node;
			node = Instantiate(nodePrefab, Random.insideUnitCircle, Quaternion.identity);
			node.Value = Random.Range(0, 100);
			node.transform.parent = gameObject.transform;

			items[size] = node;
			size++;
		}

		//for (int i = 0; i < size; i++)
		//{
		//	Node node;
		//	node = Instantiate(nodePrefab, Random.insideUnitCircle, Quaternion.identity);
		//	node.Value = Random.Range(0, 100);
		//	node.transform.parent = gameObject.transform;

		//	items[i] = node;
		//}

		gens = getNumberOfGenerations();

		// Place nodes in a binary tree formation
		for (int i = 0; i < items.Length; i++)
		{
			if (i == 0) { items[i].transform.position = new Vector2(0, 4); }
			else
			{
				Vector3 parentNode = items[getParentIndex(i)].transform.position;

				if (i % 2 != 0)
					items[i].transform.position = new Vector2(parentNode.x - halfWayFromParent(getInverseGeneration(i, gens)), parentNode.y - 1);
				else
					items[i].transform.position = new Vector2(parentNode.x + halfWayFromParent(getInverseGeneration(i, gens)), parentNode.y - 1);
			}
		}

		// Add connections between parent and child nodes
		for (int i = 0; i < items.Length; i++)
		{
			if (hasLeftChild(i))
			{
				Connection c;
				c = Instantiate(connectionPrefab, Vector3.zero, Quaternion.identity);
				c.Parent = items[i];
				c.Child = items[getLeftChildIndex(i)];
				c.transform.parent = items[i].transform;
				connections.Add(c);

				if (hasRightChild(i))
				{
					c = Instantiate(connectionPrefab, Vector3.zero, Quaternion.identity);
					c.Parent = items[i];
					c.Child = items[getRightChildIndex(i)];
					c.transform.parent = items[i].transform;
					connections.Add(c);
				}
			}
		}
	}

	void Update()
	{
		gens = getNumberOfGenerations();

		for (int i = 0; i < connections.Count; i++)
		{
			checkConnectionColor(connections[i]);
		}
	}

	int getLeftChildIndex(int parentIndex) { return parentIndex * 2 + 1; }
	int getRightChildIndex(int parentIndex) { return parentIndex * 2 + 2; }
	int getParentIndex(int childIndex) { return (childIndex - 1) / 2; }

	bool hasLeftChild(int index) { return getLeftChildIndex(index) < items.Length; }
	bool hasRightChild(int index) { return getRightChildIndex(index) < items.Length; }
	bool hasParent(int index) { return getParentIndex(index) >= 0; }

	Node leftChild(int index) { return items[getLeftChildIndex(index)]; }
	Node rightChild(int index) { return items[getRightChildIndex(index)]; }
	Node parent(int index) { return items[getParentIndex(index)]; }

	void swap(int indexOne, int indexTwo) 
	{
		Node temp = items[indexOne];
		items[indexOne] = items[indexTwo];
		items[indexTwo] = temp;
	}
	void ensureCapacity() { }
	Node peek() { return items[0]; }
	void insert(Node node) 
	{

	}
	Node extract()
	{
		Node node = items[0];
		items[0] = items[items.Length - 1];
		return node;
	}
	
	void heapifyUp() { }
	void heapifyDown() { }
	    
	// Group "Helper methods"
	/// <summary>
	/// Returns the number of generations of a binary heap tree.
	/// </summary>
	/// <returns></returns>
	int getNumberOfGenerations()
	{
		bool found = false;
		int generations = 0;

		for (int i = 0; i < items.Length; i++)
		{
			if (found)
				break;

			if (items.Length == 0)
			{
				generations = 0;
				found = true;
			}
			else if (items.Length > Mathf.Pow(2, i) - 1 && items.Length < Mathf.Pow(2, i + 1))
			{
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
	int getGeneration(int i)
	{
		int generation = 0;

		if (i == 0) 
			generation = 1;
		else
		{
			for (int j = 2; j < items.Length; j++)
			{
				if (i + 1 < Mathf.Pow(2, j))
				{
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
	int getInverseGeneration(int index, int generations)
	{
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
	float halfWayFromParent(int itemGeneration)
	{
		return Mathf.Pow(2, itemGeneration - 2 - 1)*2;
	}

	/// <summary>
	/// For MIN HEAP
	/// If parent node value is smaller than child node value then color green (CORRECT ORDER)
	/// If parent node value is greater than child node value then color red (INCORRECT ORDER)
	/// </summary>
	/// <param name="c"></param>
	void checkConnectionColor(Connection c)
	{
		if (c.Parent.Value <= c.Child.Value) {
			c.GetComponent<LineRenderer>().startColor = Color.green;
			c.GetComponent<LineRenderer>().endColor = Color.green;
		} else {
			c.GetComponent<LineRenderer>().startColor = Color.red;
			c.GetComponent<LineRenderer>().endColor = Color.red;
		}
	}
}
