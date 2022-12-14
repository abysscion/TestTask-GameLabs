using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ships
{
	public class ShipCanvasController : MonoBehaviour
	{
		[SerializeField] private LayoutGroup iconsGroup;
		[SerializeField] private Ship creature;
		[SerializeField] private TMP_Text healthText;
		[SerializeField] private TMP_Text shieldText;
		[SerializeField] private Image healthFillImage;
		[SerializeField] private Image shieldFillImage;


		private void Start()
		{
			UpdateView();
			creature.Stats.AnyStatChanged += OnStatChanged;
		}

		private void OnDestroy()
		{
			creature.Stats.AnyStatChanged -= OnStatChanged;
		}

		private void OnStatChanged(ShipStatType _, float __) => UpdateView();

		private void UpdateView()
		{
			var maxHealthValue = creature.Stats.GetValue(ShipStatType.MaxHealth);
			var healthValue = creature.Stats.GetValue(ShipStatType.Health);
			var shieldValue = creature.Stats.GetValue(ShipStatType.Shield);

			healthText.text = $"{Mathf.CeilToInt(healthValue)}/{Mathf.CeilToInt(maxHealthValue)}";
			healthFillImage.fillAmount = healthValue / maxHealthValue;

			if (shieldValue <= 0)
			{
				shieldText.gameObject.SetActive(false);
				shieldFillImage.gameObject.SetActive(false);
			}
			else
			{
				shieldText.gameObject.SetActive(true);
				shieldFillImage.gameObject.SetActive(true);
				shieldText.text = $"({Mathf.CeilToInt(shieldValue)})";
				shieldFillImage.fillAmount = shieldValue / maxHealthValue;
			}
		}
	}
}
