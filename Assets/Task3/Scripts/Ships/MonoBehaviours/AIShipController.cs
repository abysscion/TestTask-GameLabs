using System;
using System.Collections.Generic;
using CoreGameplay;
using UnityEngine;
using static CoreGameplay.UIShipsBuildingWindowController;

namespace Ships
{
	public class AIShipController : MonoBehaviour
	{
		[SerializeField] private ShipType shipType;

		private IReadOnlyCollection<ShipWeapon> _shipWeapons;
		private Ship _ship;
		private bool _shipCreated;

		public event Action ShipCreated;
		private event Action<float> _processShipSpecialStatsMethod;

		public Ship Ship => _ship;

		private void Start()
		{
			UIMainCanvasController.Instance.ShipBuildingWindow.ConfirmButtonClicked += OnConfirmButtonClicked;
		}

		private void Update()
		{
			if (!_shipCreated)
				return;

			var enemyShip = shipType == ShipType.A ? GameController.Instance.ShipB : GameController.Instance.ShipA;
			foreach (var weapon in _shipWeapons)
			{
				if (weapon.TryUseWeapon(enemyShip, Time.time, out var message))
					Debug.Log(message);
			}
			_processShipSpecialStatsMethod.Invoke(Time.time);
		}

		private void OnConfirmButtonClicked(SelectedShipData shipAData, SelectedShipData shipBData)
		{
			var shipData = shipType == ShipType.A ? shipAData : shipBData;

			_ship = new Ship(shipData.config, shipData.weaponConfigs.ToArray(), shipData.passiveConfigs.ToArray(),
								out _shipWeapons, out _processShipSpecialStatsMethod);
			_shipCreated = true;
			ShipCreated?.Invoke();
		}

		private enum ShipType { A, B }
	}
}
