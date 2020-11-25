using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public static class TreeUtils
{
	// Colors
	public static Color32 RED = new Color32(255, 77, 77, 255);
	public static Color32 GREEN = new Color32(77, 255, 77, 255);
	public static Color32 BLUE = new Color32(77, 77, 255, 255);
	public static Color32 YELLOW = new Color32(255, 200, 0, 255);
	public static Color32 TRANSPARENT = new Color32(0, 0, 0, 0);

	/// <summary>
	/// Places nodes evenly in a binary tree formation.
	/// </summary>
	/// <param name="items"></param>
	/// <param name="sizeOfItems"></param>
	public static void PlaceNodes(Node[] items, int sizeOfItems)
	{
		for (int i = 0; i < sizeOfItems; i++)
		{
			if (i == 0) items[i].transform.position = new Vector2(0, 3);
			else
			{
				Vector2 parentNode = items[(i - 1) / 2].transform.position;
				if (i % 2 != 0)
					items[i].transform.position = new Vector2(parentNode.x - HalfWayFromParent(sizeOfItems, i), parentNode.y - 1.5f);
				else
					items[i].transform.position = new Vector2(parentNode.x + HalfWayFromParent(sizeOfItems, i), parentNode.y - 1.5f);
			}
		}
	}

	/// <summary>
	/// Adds nodes to the binary heap tree depending on size input.
	/// </summary>
	/// <param name="gameObject"></param>
	/// <param name="items"></param>
	/// <param name="sizeOfItems"></param>
	/// <param name="nodePrefab"></param>
	public static void AddNodes(GameObject gameObject, Node[] items, int sizeOfItems, Node nodePrefab)
	{
		for (int i = 0; i < sizeOfItems; i++)
		{
			Node node = GameObject.Instantiate(nodePrefab);
			node.Value = UnityEngine.Random.Range(0, 100);
			node.transform.parent = gameObject.transform;
			items[i] = node;
		}
	}

	/// <summary>
	/// Adds all connections to nodes when first spawning the binary heap tree.
	/// </summary>
	/// <param name="connections"></param>
	/// <param name="items"></param>
	/// <param name="sizeOfItems"></param>
	/// <param name="connectionPrefab"></param>
	public static void AddConnections(List<Connection> connections, Node[] items, int sizeOfItems, Connection connectionPrefab)
	{
		for (int i = 0; i < sizeOfItems; i++)
		{
			if ((i * 2 + 1) < sizeOfItems)
			{
				Connection c = GameObject.Instantiate(connectionPrefab);
				c.Parent = items[i];
				c.Child = items[i * 2 + 1];
				c.transform.parent = items[i].transform;
				connections.Add(c);

				if ((i * 2 + 2) < sizeOfItems)
				{
					c = GameObject.Instantiate(connectionPrefab);
					c.Parent = items[i];
					c.Child = items[i * 2 + 2];
					c.transform.parent = items[i].transform;
					connections.Add(c);
				}
			}
		}
	}

	/// <summary>
	/// Adds a new connection when a new node is added to the binary heap tree.
	/// </summary>
	/// <param name="connections"></param>
	/// <param name="items"></param>
	/// <param name="sizeOfItems"></param>
	/// <param name="connectionPrefab"></param>
	public static void AddConnection(List<Connection> connections, Node[] items, int sizeOfItems, Connection connectionPrefab)
	{
		Connection c = GameObject.Instantiate(connectionPrefab);
		c.Parent = items[(sizeOfItems - 2) / 2];
		c.Child = items[sizeOfItems - 1];
		c.transform.parent = items[(sizeOfItems - 2) / 2].transform;
		connections.Add(c);
	}

	/// <summary>
	/// Returns the number of generations of a binary heap tree.
	/// </summary>
	/// <param name="sizeOfHeap"></param>
	/// <returns></returns>
	public static int GetNumberOfGenerations(int sizeOfHeap)
	{
		bool found = false;
		int generations = 0;
		for (int i = 0; i < sizeOfHeap; i++)
		{
			if (found) break;
			if (sizeOfHeap == 0)
			{
				generations = 0;
				found = true;
			}
			else if (sizeOfHeap > Mathf.Pow(2, i) - 1 && sizeOfHeap < Mathf.Pow(2, i + 1))
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
	/// <param name="sizeOfHeap"></param>
	/// <param name="index"></param>
	/// <returns></returns>
	public static int GetGeneration(int sizeOfHeap, int index)
	{
		int generation = 0;
		if (index == 0) generation = 1;
		else
		{
			for (int j = 2; j < sizeOfHeap; j++)
			{
				if (index + 1 < Mathf.Pow(2, j))
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
	/// <param name="sizeOfHeap"></param>
	/// <param name="generations"></param>
	/// <param name="index"></param>
	/// <returns></returns>
	public static int GetInverseGeneration(int sizeOfHeap, int index)
	{
		int generations = GetNumberOfGenerations(sizeOfHeap);
		float middle = generations / 2 + 0.5f;
		int iG = 0;
		if (generations % 2 == 0)
			iG = (int)(middle + (middle - GetGeneration(sizeOfHeap, index)));
		else if (generations % 2 != 0)
			iG = (int)(middle + (middle - GetGeneration(sizeOfHeap, index)) + 1);
		return iG;
	}

	/// <summary>
	/// Returns a value (distance) which determines the placement of the child nodes
	/// in relation to the parent node's position.
	/// </summary>
	/// <param name="itemInverseGeneration"></param>
	/// <returns></returns>
	public static float HalfWayFromParent(int sizeOfHeap, int index)
	{
		int itemInverseGeneration = GetInverseGeneration(sizeOfHeap, index);
		return Mathf.Pow(2, itemInverseGeneration - 2 - 1) * 2;
	}

	/// <summary>
	/// For MAX HEAP
	/// If parent node value is greater or equal to child node value then color green (CORRECT ORDER)
	/// If parent node value is smaller or equal to child node value then color red (INCORRECT ORDER)
	/// </summary>
	/// <param name="c"></param>
	public static void CheckConnectionColorMax(Connection c)
	{
		if (c.Parent.Value >= c.Child.Value)
			c.ChangeColor(Color.green);
		else
			c.ChangeColor(Color.red);
	}

	/// <summary>
	/// /// For MIN HEAP
	/// If parent node value is smaller or equal to child node value then color green (CORRECT ORDER)
	/// If parent node value is greater or equal child node value then color red (INCORRECT ORDER)
	/// </summary>
	/// <param name="c"></param>
	public static void CheckConnectionColorMin(Connection c)
	{
		if (c.Parent.Value <= c.Child.Value)
			c.ChangeColor(Color.green);
		else
			c.ChangeColor(Color.red);
	}

	/// <summary>
	/// Check all connections colors for a Min Heap
	/// </summary>
	/// <param name="connections"></param>
	public static void CheckConnectionsMin(List<Connection> connections)
	{
		for (int i = 0; i < connections.Count; i++)
			if (connections[i] != null)
				CheckConnectionColorMin(connections[i]);
	}

	/// <summary>
	/// Check all connections colors for a Max Heap
	/// </summary>
	/// <param name="connections"></param>
	public static void CheckConnectionsMax(List<Connection> connections)
	{
		for (int i = 0; i < connections.Count; i++)
			if (connections[i] != null)
				CheckConnectionColorMax(connections[i]);
	}
}
