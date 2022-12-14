using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StationLine
{
	[SerializeField] private Transform tf;
	[SerializeField] private Color color;
	[SerializeField] private bool isLoop;

	private StationPoint[] _points;

	public IReadOnlyCollection<StationPoint> Points => _points;
	public Transform Tf => tf;
	public Color Color => color;
	public bool IsLoop => isLoop;

	private StationLine() { }

	public StationPoint GetRootPoint() => _points.Length > 0 ? _points[0] : null;

	public StationPoint GetPointByIndex(int i)
	{
		if (_points.Length > 0 && i >= 0 && i < _points.Length)
			return _points[i];
		return null;
	}

	public void InstallPoints(StationPoint[] points)
	{
		_points = points ?? (new StationPoint[0]);
	}
}
