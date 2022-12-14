namespace Ships
{
	public class ShipStat
	{
		public ShipStatType type;

		private float _value;

		/// <summary> T0 refers to a new value after change </summary>
		public event System.Action<float> ValueChanged;

		public float Value
		{
			get => _value;
			set
			{
				_value = value;
				ValueChanged?.Invoke(_value);
			}
		}

		public ShipStat(ShipStatType type, float value)
		{
			this.type = type;
			Value = value;
		}
	}
}
