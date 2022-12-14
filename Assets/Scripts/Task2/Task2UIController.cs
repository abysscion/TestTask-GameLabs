using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.TMP_Dropdown;

namespace Task2
{
	public class Task2UIController : MonoBehaviour
	{
		[Header("UI Controls")]
		[SerializeField] private TMP_InputField inputFieldResult;
		[SerializeField] private TMP_Dropdown dropdownFrom;
		[SerializeField] private TMP_Dropdown dropdownTo;
		[Header("Other")]
		[SerializeField] private StationLinesContainer linesContainer;

		private Dictionary<OptionData, StationPoint> optionDataToStationPointDic;

		private void Start()
		{
			FillDropDownsOptions();
			dropdownFrom.onValueChanged.AddListener(OnAnyDropdownChanged);
			dropdownTo.onValueChanged.AddListener(OnAnyDropdownChanged);
		}

		private void OnAnyDropdownChanged(int _)
		{

		}

		private void FillDropDownsOptions()
		{
			var lines = linesContainer.GetLines();

			dropdownFrom.ClearOptions();
			dropdownTo.ClearOptions();
			optionDataToStationPointDic = new Dictionary<OptionData, StationPoint>();
			foreach (var line in lines)
			{
				foreach (var point in line.Points)
				{
					var option = new OptionData($"{line.Tf.name} | {point.tf.name}");

					dropdownFrom.options.Add(option);
					dropdownTo.options.Add(option);
					optionDataToStationPointDic.Add(option, point);
				}
			}
		}
	}
}
