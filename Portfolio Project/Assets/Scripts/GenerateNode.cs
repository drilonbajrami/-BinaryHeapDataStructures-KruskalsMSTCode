using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNode : MonoBehaviour
{
    [SerializeField]
    private Node nodePrefab;
    [SerializeField]
    private int value;

    private Node node;
    
    public void Generate()
    {
        value = Random.Range(0, 100);
        node = Instantiate(nodePrefab, Random.insideUnitCircle, Quaternion.identity);
        node.Value = value;
        node.transform.parent = this.transform;
    }
}
