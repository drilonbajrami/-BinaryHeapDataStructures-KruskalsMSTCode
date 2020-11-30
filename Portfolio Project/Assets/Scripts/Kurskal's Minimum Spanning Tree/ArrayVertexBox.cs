using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayVertexBox : MonoBehaviour
{
    public ArrayVertex arrayVertexPrefab;

    private ArrayVertex[] items;
    public ArrayVertex[] Items { get { return items; } set { items = value; } }
}
