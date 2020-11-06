using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (GenerateNode))]
public class GenerateNodeEditor : Editor
{
	public override void OnInspectorGUI()
	{
		GenerateNode generateNode = target as GenerateNode;

		DrawDefaultInspector();

		if (GUILayout.Button("Generate"))
		{
			generateNode.Generate();
		}
	}
}
