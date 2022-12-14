using System.Collections.Generic;
using UnityEngine;

public class StationLinesContainer : MonoBehaviour
{
	[SerializeField] private StationLine[] lines;
	[SerializeField] private List<ConnectionList> listOfConnectionList;

	private Dictionary<Transform, StationPoint> _tfToStationPoint;

	private void OnValidate()
	{
		ValidateLines();
		InstallPointsLinks();
	}

	public StationLine[] GetLines()
	{
		var result = new StationLine[lines.Length];
		lines.CopyTo(result, 0);
		return result;
	}

	private void ValidateLines()
	{
		var linesCount = transform.childCount;

		_tfToStationPoint = new Dictionary<Transform, StationPoint>();
		for (var i = 0; i < linesCount; i++)
		{
			var lineTf = transform.GetChild(i);
			var pointsCount = lineTf.childCount;
			var points = new StationPoint[pointsCount];

			for (var j = 0; j < pointsCount; j++)
			{
				var pointTf = lineTf.GetChild(j);

				points[j] = new StationPoint(lines[i], pointTf);
				points[j].prev = j > 0 ? points[j - 1] : null;
				points[j].next = j < pointsCount - 1 ? points[pointsCount - 1] : null;
				if (points[j].prev != null && points[j].prev.next == null)
					points[j].prev.next = points[j];
				if (lines[i].IsLoop && j == pointsCount - 1 && pointsCount > 1)
				{
					points[j].next = points[0];
					points[0].prev = points[j];
				}
				_tfToStationPoint.Add(pointTf, points[j]);
			}
			lines[i].InstallPoints(points);
		}
	}

	private void InstallPointsLinks()
	{
		foreach (var connectionsList in listOfConnectionList)
		{
			var linkedPoints = new StationPoint[connectionsList.connectedTfs.Count];
			for (var i = 0; i < linkedPoints.Length; i++)
			{
				if (_tfToStationPoint.TryGetValue(connectionsList.connectedTfs[i], out var point))
					linkedPoints[i] = point;
			}
			foreach (var point in linkedPoints)
				point.InstallLinks(linkedPoints);
		}
	}

	[System.Serializable]
	private class ConnectionList { public List<Transform> connectedTfs; }
}
