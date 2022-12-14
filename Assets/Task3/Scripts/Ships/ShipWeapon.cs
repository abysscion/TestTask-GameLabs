namespace Ships
{
	public class ShipWeapon
	{
		protected ShipModuleWeaponConfig config;
		protected Ship owner;
		protected float lastUsageTime;

		protected float AttackDelay => config.Cooldown - (config.Cooldown * owner.Stats[ShipStatType.WeaponCooldownReductionMultiplier]);

		public ShipWeapon(Ship owner, ShipModuleWeaponConfig config)
		{
			this.config = config;
			this.owner = owner;
		}

		public virtual bool TryUseWeapon(Ship target, float currentTime)
		{
			if (target == null || (currentTime - lastUsageTime) < AttackDelay)
				return false;

			lastUsageTime = currentTime;
			DealDamageToTarget(target, config.Damage); // _config.Damage * _owner.Stats.DamageMultiplier
			return true;
		}

		protected virtual void DealDamageToTarget(Ship target, float damageValue)
		{
			var stats = target.Stats;

			if (stats.GetValue(ShipStatType.Shield) <= 0)
				target.Stats.AddValue(ShipStatType.Health, -damageValue);
			else
			{
				var currentShield = stats[ShipStatType.Shield];
				var shieldAfterDamage = currentShield - damageValue;

				if (shieldAfterDamage >= 0)
					target.Stats.SetValue(ShipStatType.Shield, shieldAfterDamage);
				else
				{
					target.Stats.SetValue(ShipStatType.Shield, 0f);
					target.Stats.AddValue(ShipStatType.Health, shieldAfterDamage);
				}
			}
		}
	}
}
