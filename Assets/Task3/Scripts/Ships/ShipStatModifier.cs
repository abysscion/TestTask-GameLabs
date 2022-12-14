using System;

namespace Ships
{
	[Serializable]
	public struct ShipStatModifier
	{
		public ShipStatType type;
		public float value;

		public ShipStatModifier(ShipStatType type, float value)
		{
			this.type = type;
			this.value = value;
		}
	}
}
