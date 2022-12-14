using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{
	[CreateAssetMenu(fileName = "NewShipConfig", menuName = "Configs/Create ship config")]
	public class ShipConfig : ScriptableObject
	{
		[SerializeField] private List<ShipStatModifier> statsSettings;
		[SerializeField] private List<ShipModulePassiveConfig> availablePassives;
		[SerializeField] private List<ShipModuleWeaponConfig> availableWeapons;
		[SerializeField] private int weaponSlotsCount;
		[SerializeField] private int passiveSlotsCount;

		public int WeaponSlotsCount => weaponSlotsCount;
		public int PassiveSlotsCount => passiveSlotsCount;

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

		public IReadOnlyCollection<ShipModulePassiveConfig> GetAvailablePassivesConfigs() => availablePassives;

		public IReadOnlyCollection<ShipModuleWeaponConfig> GetAvailableWeaponsConfigs() => availableWeapons;
	}
}
