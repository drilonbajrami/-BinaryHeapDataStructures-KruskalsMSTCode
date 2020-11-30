using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayEdge : MonoBehaviour
{
    [SerializeField]
    private Edge edgePrefab;
    [SerializeField]
    private Vertex vertexPrefab;

    private Edge e;
    public Edge E { get { return e; } set { e = value; } }

    void OnEnable()
    {
        e = Instantiate(edgePrefab);
        e.transform.parent = gameObject.transform;
        e.A = Instantiate(vertexPrefab);
        e.A.transform.parent = gameObject.transform;
        e.B = Instantiate(vertexPrefab);
        e.B.transform.parent = gameObject.transform;
        e.A.transform.localScale = new Vector3(0.25f, 0.25f, 0);
        e.B.transform.localScale = new Vector3(0.25f, 0.25f, 0);
    }

    void Update()
    {
        if (e.A != null && e.B != null)
        {
            e.A.transform.position = new Vector3(gameObject.transform.position.x - 0.5f - (float)e.Weight / 40, gameObject.transform.position.y, 0);
            e.B.transform.position = new Vector3(gameObject.transform.position.x + 0.5f + (float)e.Weight / 40, gameObject.transform.position.y, 0);
        }
    }
}
