using UnityEngine;

namespace Ships
{
	public abstract class ShipModuleConfig : ScriptableObject
	{
		[SerializeField] private string moduleName;

		public string ModuleName => moduleName;
	}
}
