using System;
using Ships;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TMP_Dropdown;

namespace CoreGameplay
{
	public class UIShipsBuildingWindowController : MonoBehaviour
	{
		[SerializeField] private TMP_Dropdown moduleDropdownPrefab;
		[SerializeField] private Button confirmButton;
		[SerializeField] private UIShipConfigData[] shipConfigDatas;

		public Button.ButtonClickedEvent EndTurnButtonClicked => confirmButton.onClick;

		private void Start()
		{
			ClearLayoutGroups();
			FillLayoutGroups();
		}

		private void ClearLayoutGroups()
		{
			foreach (var shipConfigData in shipConfigDatas)
			{
				for (var i = 0; i < shipConfigData.passivesLayoutGroup.transform.childCount; i++)
					Destroy(shipConfigData.passivesLayoutGroup.transform.GetChild(i).gameObject);
				for (var i = 0; i < shipConfigData.weaponsLayoutGroup.transform.childCount; i++)
					Destroy(shipConfigData.weaponsLayoutGroup.transform.GetChild(i).gameObject);
			}
		}

		private void FillLayoutGroups()
		{
			foreach (var shipConfigData in shipConfigDatas)
			{
				for (var i = 0; i < shipConfigData.config.PassiveSlotsCount; i++)
				{
					var dropdown = Instantiate(moduleDropdownPrefab, shipConfigData.passivesLayoutGroup.transform);

					dropdown.ClearOptions();
					foreach (var passiveConfig in shipConfigData.config.GetAvailablePassivesConfigs())
					{
						var optionData = new OptionData($"{passiveConfig.ModuleName}");
						dropdown.options.Add(optionData);
					}
					dropdown.RefreshShownValue();
				}

				for (var i = 0; i < shipConfigData.config.WeaponSlotsCount; i++)
				{
					var dropdown = Instantiate(moduleDropdownPrefab, shipConfigData.weaponsLayoutGroup.transform);

					dropdown.ClearOptions();
					foreach (var weaponConfig in shipConfigData.config.GetAvailableWeaponsConfigs())
					{
						var optionData = new OptionData($"{weaponConfig.ModuleName}");
						dropdown.options.Add(optionData);
					}
					dropdown.RefreshShownValue();
				}
			}
		}

		[Serializable]
		private class UIShipConfigData
		{
			public VerticalLayoutGroup passivesLayoutGroup;
			public VerticalLayoutGroup weaponsLayoutGroup;
			public ShipConfig config;
		}
	}
}
