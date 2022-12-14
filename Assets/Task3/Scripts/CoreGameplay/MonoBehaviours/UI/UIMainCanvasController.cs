using UnityEngine;
using Utilities.Components;

namespace CoreGameplay
{
	public class UIMainCanvasController : MonoSingleton<UIMainCanvasController>
	{
		[SerializeField] private UIShipsBuildingWindowController shipBuildingWindow;
		[SerializeField] private GameObject gameOverPanel;

		public UIShipsBuildingWindowController ShipBuildingWindow => shipBuildingWindow;

		public override void Initialize()
		{

		}

		private void OnDestroy()
		{

		}
	}
}
