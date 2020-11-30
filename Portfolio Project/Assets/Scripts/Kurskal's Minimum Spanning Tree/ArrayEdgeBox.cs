using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayEdgeBox : MonoBehaviour
{
    public ArrayEdge arrayEdgePrefab;

    private ArrayEdge[] items;
    public ArrayEdge[] Items { get { return items; } set { items = value; } }
}
