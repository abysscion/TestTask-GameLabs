using System;
using System.Collections.Generic;

namespace Ships
{
	public class Ship
	{
		private const float DELTA_TIME_FOR_SPECIAL_STATS = 1f;
		private ShipStatsContainer _stats;
		private ShipWeapon[] _weapons;
		private ShipConfig _config;
		private float _lastTimeStatsWereProcessed;

		public event Action Died;

		public ShipStatsContainer Stats => _stats;
		public string Name => _config.ShipName;

		public Ship(ShipConfig config, ShipModuleWeaponConfig[] weaponConfigs, ShipModulePassiveConfig[] passiveConfigs,
					out IReadOnlyCollection<ShipWeapon> weapons, out Action<float> processShipSpecialStatsMethod)
		{
			_config = config;
			_stats = new ShipStatsContainer(_config);
			weapons = CreateWeapons(weaponConfigs);
			ApplyPassives(passiveConfigs);
			processShipSpecialStatsMethod = ProcessShipSpecialStats;
			Stats.AddSubscriberToValueChanged(ShipStatType.Health, OnHealthChanged);
		}

		private void ProcessShipSpecialStats(float currentTime)
		{
			if ((currentTime - _lastTimeStatsWereProcessed) < DELTA_TIME_FOR_SPECIAL_STATS)
				return;

			var missingShield = Stats[ShipStatType.MaxShield] - Stats[ShipStatType.Shield];
			var shieldRegen = Stats[ShipStatType.ShieldRegenRate] + Stats[ShipStatType.ShieldRegenRate] * Stats[ShipStatType.ShieldRegenRateMultiplier];

			Stats.AddValue(ShipStatType.Shield, Math.Clamp(shieldRegen, 0f, missingShield));
			_lastTimeStatsWereProcessed = currentTime;
		}

		private IReadOnlyCollection<ShipWeapon> CreateWeapons(ShipModuleWeaponConfig[] weaponConfigs)
		{
			var weaponsLst = new List<ShipWeapon>(Math.Min(weaponConfigs.Length, _config.WeaponSlotsCount));

			for (var i = 0; i < _config.WeaponSlotsCount; i++)
			{
				if (i >= weaponConfigs.Length)
					break;
				weaponsLst.Add(new ShipWeapon(this, weaponConfigs[i]));
			}

			_weapons = weaponsLst.ToArray();
			return _weapons;
		}

		private void ApplyPassives(ShipModulePassiveConfig[] passiveConfigs)
		{
			foreach (var passive in passiveConfigs)
			{
				var statModifiers = passive.GetStatModifiers();
				foreach (var statModifier in statModifiers)
				{
					var modType = statModifier.type;
					var modValue = statModifier.value;
					switch (statModifier.type)
					{
						case ShipStatType.MaxHealth:
							Stats.AddValue(modType, modValue);
							Stats.AddValue(ShipStatType.Health, modValue);
							break;
						case ShipStatType.Health:
							var newHp = Stats[modType] + modValue;
							Stats.SetValue(modType, newHp <= Stats[ShipStatType.MaxHealth] ? newHp : Stats[ShipStatType.MaxHealth]);
							break;
						case ShipStatType.MaxShield:
							Stats.AddValue(modType, modValue);
							Stats.AddValue(ShipStatType.Shield, modValue);
							break;
						case ShipStatType.Shield:
							var newShield = Stats[modType] + modValue;
							Stats.SetValue(modType, newShield <= Stats[ShipStatType.MaxShield] ? newShield : Stats[ShipStatType.MaxShield]);
							break;
						case ShipStatType.ShieldRegenRate:
							Stats.AddValue(modType, modValue);
							break;
						case ShipStatType.ShieldRegenRateMultiplier:
							Stats.AddValue(modType, modValue);
							break;
						case ShipStatType.WeaponCooldownReductionMultiplier:
							Stats.AddValue(modType, modValue);
							break;
						default:
							throw new NotImplementedException();
					}
				}
			}
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
