using System;
using System.Collections.Generic;
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

		public Action<SelectedShipData, SelectedShipData> ConfirmButtonClicked;

		private void Start()
		{
			ClearLayoutGroups();
			FillLayoutGroups();
			confirmButton.onClick.AddListener(OnConfirmButtonClicked);
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

		private void OnConfirmButtonClicked()
		{
			var shipDatas = GatherShipDatas();
			ConfirmButtonClicked?.Invoke(shipDatas[0], shipDatas[1]);
		}

		private SelectedShipData[] GatherShipDatas()
		{
			var selectedShipDatas = new SelectedShipData[2];
			var shipConfigDataA = shipConfigDatas[0];
			var shipConfigDataB = shipConfigDatas[1];
			var shipAWeaponConfigs = shipConfigDataA.config.GetAvailableWeaponsConfigs();
			var shipAPassiveConfigs = shipConfigDataA.config.GetAvailablePassivesConfigs();
			var shipBWeaponConfigs = shipConfigDataB.config.GetAvailableWeaponsConfigs();
			var shipBPassiveConfigs = shipConfigDataB.config.GetAvailablePassivesConfigs();

			selectedShipDatas[0].config = shipConfigDataA.config;
			selectedShipDatas[0].weaponConfigs = new List<ShipModuleWeaponConfig>();
			foreach (var dropdown in shipConfigDataA.weaponsLayoutGroup.GetComponentsInChildren<TMP_Dropdown>())
				selectedShipDatas[0].weaponConfigs.Add(shipAWeaponConfigs[dropdown.value]);
			selectedShipDatas[0].passiveConfigs = new List<ShipModulePassiveConfig>();
			foreach (var dropdown in shipConfigDataA.passivesLayoutGroup.GetComponentsInChildren<TMP_Dropdown>())
				selectedShipDatas[0].passiveConfigs.Add(shipAPassiveConfigs[dropdown.value]);

			selectedShipDatas[1].config = shipConfigDataB.config;
			selectedShipDatas[1].weaponConfigs = new List<ShipModuleWeaponConfig>();
			foreach (var dropdown in shipConfigDataB.weaponsLayoutGroup.GetComponentsInChildren<TMP_Dropdown>())
				selectedShipDatas[1].weaponConfigs.Add(shipBWeaponConfigs[dropdown.value]);
			selectedShipDatas[1].passiveConfigs = new List<ShipModulePassiveConfig>();
			foreach (var dropdown in shipConfigDataB.passivesLayoutGroup.GetComponentsInChildren<TMP_Dropdown>())
				selectedShipDatas[1].passiveConfigs.Add(shipBPassiveConfigs[dropdown.value]);

			return selectedShipDatas;
		}

		[Serializable]
		private class UIShipConfigData
		{
			public VerticalLayoutGroup passivesLayoutGroup;
			public VerticalLayoutGroup weaponsLayoutGroup;
			public ShipConfig config;
		}

		public struct SelectedShipData
		{
			public ShipConfig config;
			public List<ShipModuleWeaponConfig> weaponConfigs;
			public List<ShipModulePassiveConfig> passiveConfigs;
		}
	}
}
