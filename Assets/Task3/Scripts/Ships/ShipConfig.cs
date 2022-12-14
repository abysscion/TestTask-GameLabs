using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{
	[CreateAssetMenu(fileName = "NewShipConfig", menuName = "Configs/Create ship config")]
	public class ShipConfig : ScriptableObject
	{
		[SerializeField] private string shipName;
		[SerializeField] private int passiveSlotsCount;
		[SerializeField] private int weaponSlotsCount;
		[SerializeField] private List<ShipStatModifier> statsSettings;
		[SerializeField] private List<ShipModulePassiveConfig> availablePassives;
		[SerializeField] private List<ShipModuleWeaponConfig> availableWeapons;

		public string ShipName => shipName;
		public int PassiveSlotsCount => passiveSlotsCount;
		public int WeaponSlotsCount => weaponSlotsCount;

		private void OnValidate()
		{
			var existedStatTypes = new HashSet<ShipStatType>(statsSettings.Count);

			foreach (var statModifier in statsSettings)
				existedStatTypes.Add(statModifier.type);

			foreach (var statTypeEnumValue in Enum.GetValues(typeof(ShipStatType)))
			{
				var statType = (ShipStatType)statTypeEnumValue;
				if (!existedStatTypes.Contains(statType))
				{
					statsSettings.Add(new ShipStatModifier(statType, 0f));
					existedStatTypes.Add(statType);
				}
			}
		}

		public ShipStatModifier[] GetStatsSettings()
		{
			var result = new ShipStatModifier[statsSettings.Count];

			statsSettings.CopyTo(result, 0);
			return result;
		}

		public IReadOnlyList<ShipModulePassiveConfig> GetAvailablePassivesConfigs() => availablePassives;

		public IReadOnlyList<ShipModuleWeaponConfig> GetAvailableWeaponsConfigs() => availableWeapons;
	}
}
