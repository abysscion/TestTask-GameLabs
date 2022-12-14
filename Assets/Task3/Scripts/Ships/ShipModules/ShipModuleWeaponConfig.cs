using UnityEngine;

namespace Ships
{
	[CreateAssetMenu(fileName = "NewWeaponModuleConfig", menuName = "Configs/Create weapon module config")]
	public class ShipModuleWeaponConfig : ShipModuleConfig
	{
		[SerializeField, Min(0.01f)] private float attackDelay;
		[SerializeField, Min(0.01f)] private float damage;

		public float Cooldown => attackDelay;
		public float Damage => damage;
	}
}
