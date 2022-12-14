using System.Collections.Generic;
using UnityEngine;

public class StationPoint
{
	public StationPoint prev, next;

	public readonly StationLine line;
	public readonly Transform tf;

	private StationPoint[] _links;

	public IReadOnlyCollection<StationPoint> Links => _links;

	public StationPoint(StationLine line, Transform tf)
	{
		this.line = line;
		this.tf = tf;
	}

	public void InstallLinks(StationPoint[] links)
	{
		if (links == null || links.Length <= 0)
		{
			_links = new StationPoint[0];
			return;
		}

		var candidatePoints = new List<StationPoint>(links);

		for (var i = 0; i < candidatePoints.Count; i++)
		{
			var candidate = candidatePoints[i];
			if (candidate == null || candidate == this)
			{
				candidatePoints.Remove(candidate);
				i--;
			}
		}

		_links = candidatePoints.ToArray();
	}
}
