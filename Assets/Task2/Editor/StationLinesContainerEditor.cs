using System.Collections.Generic;
using Task2;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StationLinesContainer))]
public class StationLinesContainerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
	}

	private void OnSceneGUI()
	{
		var script = target as StationLinesContainer;
		var labelOffset = new Vector3(-1f, -0.5f, 0f);
		var snap = Vector3.one * 0.5f;
		var lineWidth = 4f;

		foreach (var line in script.GetLines())
		{
			if (line == null || line.Points.Count <= 0)
				continue;
			var root = line.GetRootPoint();
			if (root == null)
				continue;

			var node = root;
			var nodesSet = new HashSet<StationPoint>();
			do
			{
				if (nodesSet.Contains(node)) //cycle trigger
					break;
				else
				{
					Handles.color = line.Color;
					nodesSet.Add(node);
					node.tf.position = Handles.FreeMoveHandle(node.tf.position, Quaternion.identity, 0.5f, snap, Handles.SphereHandleCap);
					Handles.Label(node.tf.position + labelOffset, $"{line.Tf.name} | {node.tf.name}", EditorStyles.boldLabel);
					if (node.prev != null)
						Handles.DrawLine(node.tf.position, node.prev.tf.position, lineWidth);
					Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
					foreach (var linkedNode in node.Links)
						Handles.DrawLine(node.tf.position, linkedNode.tf.position, lineWidth * 0.5f);
					node = node.next;
				}
			}
			while (node != null);
		}
	}
}
