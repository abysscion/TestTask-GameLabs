using Ships;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Components;

namespace CoreGameplay
{
	public class GameController : MonoSingleton<GameController>
	{
		[SerializeField] private AIShipController shipControllerA;
		[SerializeField] private AIShipController shipControllerB;

		public Ship ShipA => shipControllerA.Ship;
		public Ship ShipB => shipControllerB.Ship;

		public override void Initialize()
		{
			Time.timeScale = 1f;
			shipControllerA.ShipCreated += OnAnyShipCreated;
			shipControllerB.ShipCreated += OnAnyShipCreated;
		}

		private void OnAnyShipCreated()
		{
			if (ShipA == null || ShipB == null)
				return;

			ShipA.Died += OnAnyShipDied;
			ShipB.Died += OnAnyShipDied;
		}

		private void OnAnyShipDied()
		{
			SceneManager.LoadScene(1);
		}

		private void Update()
		{
			if (!Input.anyKeyDown)
				return;

			if (Input.GetKeyDown(KeyCode.R))
				SceneManager.LoadScene(1);
			if (Input.GetKeyDown(KeyCode.Escape))
				Application.Quit();
		}
	}
}
