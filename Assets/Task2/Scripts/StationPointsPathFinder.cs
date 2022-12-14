using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task2
{
	public class StationPointsPathFinder
	{
		private Dictionary<StationPoint, float> _pointToMinDepthDic;
		private HashSet<StationPoint> _pointsParticipatedInSearch;
		private float _maxDepth;

		public StationPointsPathFinder(ICollection<StationLine> allAvailableLines)
		{
			_pointToMinDepthDic = new Dictionary<StationPoint, float>();
			_pointsParticipatedInSearch = new HashSet<StationPoint>();
			foreach (var line in allAvailableLines)
			{
				_maxDepth += line.Points.Count;
				foreach (var point in line.Points)
					_pointToMinDepthDic.Add(point, float.MaxValue);
			}
			_maxDepth = _pointToMinDepthDic.Keys.Count - 1;
			_pointToMinDepthDic.TrimExcess();
			_pointsParticipatedInSearch.EnsureCapacity(_pointToMinDepthDic.Keys.Count);
		}

		public RoutePath FindPathFromTo(StationPoint from, StationPoint target)
		{
			if (!_pointToMinDepthDic.ContainsKey(from) || !_pointToMinDepthDic.ContainsKey(target))
				return null;
			foreach (var point in _pointToMinDepthDic.Keys.ToList())
				_pointToMinDepthDic[point] = float.MaxValue;
			_pointsParticipatedInSearch.Clear();
			RecursivlyMarkPointsDepth(from, target, 0);
			return GetBestPath(from, target);
		}

		private void RecursivlyMarkPointsDepth(StationPoint point, StationPoint target, float curDepth)
		{
			if (curDepth > _maxDepth || point == null)
				return;
			if (curDepth > _pointToMinDepthDic[point])
				return;

			_pointToMinDepthDic[point] = curDepth;
			_pointsParticipatedInSearch.Add(point);
			foreach (var linkedPoint in point.Links)
				RecursivlyMarkPointsDepth(linkedPoint, target, curDepth + 0.001f);
			RecursivlyMarkPointsDepth(point.next, target, curDepth + 1f);
			RecursivlyMarkPointsDepth(point.prev, target, curDepth + 1f);
		}

		private RoutePath GetBestPath(StationPoint searchStart, StationPoint target)
		{
			var currentPoint = target;
			var stepsCounter = 1000;
			var path = new RoutePath();
			var candidate = new Candidate(target, _pointToMinDepthDic[target]);

			path.points.Insert(0, candidate.point);
			while (currentPoint != searchStart)
			{
				stepsCounter--;
				if (stepsCounter <= 0)
					return null;

				candidate = PickBestCandidateAmongPointNeighbours(currentPoint, candidate);
				path.points.Insert(0, candidate.point);
				if (currentPoint.line != candidate.point.line)
					path.lineChanges++;
				currentPoint = candidate.point;
			}

			return path;
		}

		private Candidate PickBestCandidateAmongPointNeighbours(StationPoint currentPoint, Candidate prevCandidate)
		{
			var candidates = new List<Candidate>(2 + currentPoint.Links.Count);
			if (currentPoint.next != null && _pointToMinDepthDic[currentPoint.next] < prevCandidate.depth)
				candidates.Add(new Candidate(currentPoint.next, _pointToMinDepthDic[currentPoint.next]));
			if (currentPoint.prev != null && _pointToMinDepthDic[currentPoint.prev] < prevCandidate.depth)
				candidates.Add(new Candidate(currentPoint.prev, _pointToMinDepthDic[currentPoint.prev]));
			foreach (var linkedPoint in currentPoint.Links)
			{
				if (linkedPoint != null && _pointToMinDepthDic[linkedPoint] < prevCandidate.depth)
					candidates.Add(new Candidate(linkedPoint, _pointToMinDepthDic[linkedPoint]));
			}

			var bestCandidate = candidates[0];
			var bestDelta = float.MaxValue;
			for (var j = 0; j < candidates.Count; j++)
			{
				var delta = Math.Abs(candidates[j].depth - prevCandidate.depth);
				if (delta < bestDelta)
				{
					bestDelta = delta;
					bestCandidate = candidates[j];
				}
			}

			return bestCandidate;
		}

		public class RoutePath
		{
			public List<StationPoint> points = new List<StationPoint>();
			public int lineChanges;

			public override string ToString()
			{
				var builder = new StringBuilder();

				for (int i = 0; i < points.Count - 1; i++)
					builder.Append($"[{points[i].line.Tf.name} | {points[i].tf.name}] -> ");
				builder.Append($"[{points[points.Count - 1].line.Tf.name} | {points[points.Count - 1].tf.name}]");
				return builder.ToString();
			}
		}

		public struct Candidate
		{
			public StationPoint point;
			public float depth;

			public Candidate(StationPoint point, float depth)
			{
				this.point = point;
				this.depth = depth;
			}
		}
	}
}
