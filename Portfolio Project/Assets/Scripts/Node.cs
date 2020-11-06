using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    private int _value;
	[SerializeField]
    private Text textValue;

	private Node parent, leftChild, rightChild;

    public int Value { get { return _value; } set { _value = value; } }
	public Node Parent { get { return parent; } set { parent = value; } }
	public Node LeftChild { get { return LeftChild; } set { LeftChild = value; } }
	public Node RightChild { get { return RightChild; } set { RightChild = value; } }

	private void Update()
	{
		textValue.text = _value.ToString();
	}


}
