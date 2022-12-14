using System;
using CoreGameplay;
using UnityEngine;

namespace Ships
{
	public class Ship
	{
		[SerializeField] private ShipConfig config;
		[SerializeField] private Transform cardSpawnPoint;
		[SerializeField] private GameTeamType team;

		private ShipStatsContainer _stats;

		public event Action Died;

		public ShipStatsContainer Stats => _stats;
		public Transform CardSpawnPoint => cardSpawnPoint;
		public GameTeamType Team => team;

		private void Awake()
		{
			_stats = new ShipStatsContainer(config);
			Stats.AddSubscriberToValueChanged(ShipStatType.Health, OnHealthChanged);
		}

		private void OnHealthChanged(float value)
		{
			if (value <= 0)
			{
				Stats.RemoveSubscriberFromValueChanged(ShipStatType.Health, OnHealthChanged);
				Died?.Invoke();
			}
		}
	}
}
