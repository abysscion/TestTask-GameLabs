using System;
using System.Collections.Generic;

namespace Ships
{
	[Serializable]
	public class ShipStatsContainer
	{
		private Dictionary<ShipStatType, ShipStat> _statTypeToStatDic;

		public event Action<ShipStatType, float> AnyStatChanged;

		public float this[ShipStatType statType]
		{
			get => _statTypeToStatDic[statType].Value;
		}

		public ShipStatsContainer(ShipConfig config)
		{
			var allStats = Enum.GetValues(typeof(ShipStatType));

			_statTypeToStatDic = new Dictionary<ShipStatType, ShipStat>(allStats.Length);
			foreach (int statTypeValue in allStats)
			{
				if (statTypeValue < 0)
					continue;

				var newStatType = (ShipStatType)statTypeValue;
				var newStat = new ShipStat(newStatType, 0f);

				_statTypeToStatDic.Add(newStat.type, newStat);
				newStat.ValueChanged += (changedValue) => AnyStatChanged?.Invoke(newStatType, newStat.Value);
			}

			var statSettings = config.GetStatsSettings();
			foreach (var setting in statSettings)
			{
				switch (setting.type)
				{
					case ShipStatType.MaxHealth:
					case ShipStatType.MaxShield:
					case ShipStatType.ShieldRegenRate:
					case ShipStatType.ShieldRegenRateMultiplier:
					case ShipStatType.WeaponCooldownReductionMultiplier:
					case ShipStatType.Health:
					case ShipStatType.Shield:
						//TODO: validate health/shield?
						_statTypeToStatDic[setting.type].Value = setting.value;
						break;
					default:
						throw new NotImplementedException();
				}
			}
		}

		public float GetValue(ShipStatType type)
		{
			return _statTypeToStatDic[type].Value;
		}

		//TODO: way to strict stat set method access?
		public void SetValue(ShipStatType type, float value)
		{
			_statTypeToStatDic[type].Value = value;
		}

		public void AddValue(ShipStatType type, float value)
		{
			_statTypeToStatDic[type].Value += value;
		}

		public void AddSubscriberToValueChanged(ShipStatType type, Action<float> OnChanged)
		{
			_statTypeToStatDic[type].ValueChanged += OnChanged;
		}

		public void RemoveSubscriberFromValueChanged(ShipStatType type, Action<float> OnChanged)
		{
			_statTypeToStatDic[type].ValueChanged -= OnChanged;
		}
	}
}
