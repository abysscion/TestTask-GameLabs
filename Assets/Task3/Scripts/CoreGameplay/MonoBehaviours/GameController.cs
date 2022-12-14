using System;
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

		public static event Action GameInitialized;

		public Ship ShipA => shipControllerA.Ship;
		public Ship ShipB => shipControllerB.Ship;

		public override void Initialize()
		{
			GameInitialized?.Invoke();
			Time.timeScale = 1f;
		}

		private void OnDestroy()
		{

		}

		private void Update()
		{
			if (!Input.anyKeyDown)
				return;

			if (Input.GetKeyDown(KeyCode.R))
				SceneManager.LoadScene(0);
			if (Input.GetKeyDown(KeyCode.Escape))
				Application.Quit();
		}
	}
}
