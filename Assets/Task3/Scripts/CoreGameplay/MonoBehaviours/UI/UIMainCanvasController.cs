using UnityEngine;
using Utilities.Components;

namespace CoreGameplay
{
	public class UIMainCanvasController : MonoSingleton<UIMainCanvasController>
	{
		[SerializeField] private UIShipsBuildingWindowController shipBuildingWindow;

		public UIShipsBuildingWindowController ShipBuildingWindow => shipBuildingWindow;

		public override void Initialize()
		{

		}
	}
}
