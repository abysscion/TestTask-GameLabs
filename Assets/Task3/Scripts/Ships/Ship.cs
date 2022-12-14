using System;
using System.Collections.Generic;

namespace Ships
{
	public class Ship
	{
		private ShipStatsContainer _stats;
		private ShipWeapon[] _weapons;
		private ShipConfig _config;

		public event Action Died;

		public ShipStatsContainer Stats => _stats;

		public Ship(ShipConfig config, ShipModuleWeaponConfig[] weaponConfigs, ShipModulePassiveConfig[] passiveConfigs,
					out IReadOnlyCollection<ShipWeapon> weapons)
		{
			_config = config;
			_stats = new ShipStatsContainer(_config);
			weapons = CreateWeapons(weaponConfigs);
			ApplyPassives(passiveConfigs);
			Stats.AddSubscriberToValueChanged(ShipStatType.Health, OnHealthChanged);
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
