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
		private StationPointsPathFinder _pathFinder;

		private void Start()
		{
			FillDropDownsOptions();
			_pathFinder = new StationPointsPathFinder(linesContainer.GetLines());
			inputFieldResult.gameObject.SetActive(false);
			dropdownFrom.onValueChanged.AddListener(OnAnyDropdownChanged);
			dropdownTo.onValueChanged.AddListener(OnAnyDropdownChanged);
		}

		private void OnAnyDropdownChanged(int _)
		{
			var from = optionDataToStationPointDic[dropdownFrom.options[dropdownFrom.value]];
			var to = optionDataToStationPointDic[dropdownTo.options[dropdownTo.value]];
			var path = _pathFinder.FindPathFromTo(from, to);

			if (path != null && path.points.Count > 0)
			{
				inputFieldResult.text = $"Line changes: {path.lineChanges}. Path: {path}";
				inputFieldResult.gameObject.SetActive(true);
			}
			else
			{
				Debug.LogError($"Unable to find path from {from} to {to}");
				inputFieldResult.gameObject.SetActive(false);
			}
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
			dropdownFrom.RefreshShownValue();
			dropdownTo.RefreshShownValue();
		}
	}
}
