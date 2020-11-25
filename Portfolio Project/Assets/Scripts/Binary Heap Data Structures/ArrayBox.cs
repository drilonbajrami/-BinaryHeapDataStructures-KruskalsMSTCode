using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayBox : MonoBehaviour
{
    public ArrayItem arrayItemPrefab;

    private ArrayItem[] items;
    public ArrayItem[] Items { get { return items; } set { items = value; } }

    public void Swap(int indexA, int indexB)
    {
        int temp = items[indexA].Value;
        items[indexA].Value = items[indexB].Value;
        items[indexB].Value = temp;
    }
}
