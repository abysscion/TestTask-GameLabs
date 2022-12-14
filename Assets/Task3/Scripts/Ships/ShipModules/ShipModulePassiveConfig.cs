using UnityEngine;

namespace Ships
{
	[CreateAssetMenu(fileName = "NewPassiveModuleConfig", menuName = "Configs/Create passive module config")]
	public class ShipModulePassiveConfig : ShipModuleConfig
	{
		[SerializeField] private ShipStatModifier[] statModifiers;

		public ShipStatModifier[] GetStatModifiers()
		{
			var result = new ShipStatModifier[statModifiers.Length];

			statModifiers.CopyTo(result, 0);
			return result;
		}
	}
}
