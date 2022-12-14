using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ships
{
	public class UIShipCanvasController : MonoBehaviour
	{
		[SerializeField] private AIShipController shipController;
		[SerializeField] private TMP_Text healthText;
		[SerializeField] private TMP_Text shieldText;
		[SerializeField] private Image healthFillImage;
		[SerializeField] private Image shieldFillImage;

		private void Start()
		{
			shipController.ShipCreated += OnShipCreated;
		}

		private void OnDestroy()
		{
			shipController.Ship.Stats.AnyStatChanged -= OnStatChanged;
		}

		private void OnStatChanged(ShipStatType _, float __) => UpdateView();

		private void OnShipCreated()
		{
			UpdateView();
			shipController.Ship.Stats.AnyStatChanged += OnStatChanged;
			shipController.ShipCreated -= OnShipCreated;
		}

		private void UpdateView()
		{
			var maxHealthValue = shipController.Ship.Stats.GetValue(ShipStatType.MaxHealth);
			var maxShieldValue = shipController.Ship.Stats.GetValue(ShipStatType.MaxShield);
			var healthValue = shipController.Ship.Stats.GetValue(ShipStatType.Health);
			var shieldValue = shipController.Ship.Stats.GetValue(ShipStatType.Shield);

			healthText.text = $"{Mathf.CeilToInt(healthValue)}/{Mathf.CeilToInt(maxHealthValue)}";
			healthFillImage.fillAmount = healthValue / maxHealthValue;
			shieldText.text = $"{Mathf.CeilToInt(shieldValue)}/{Mathf.CeilToInt(maxShieldValue)}";
			shieldFillImage.fillAmount = shieldValue / maxShieldValue;
		}
	}
}
