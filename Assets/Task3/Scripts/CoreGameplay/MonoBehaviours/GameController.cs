using System;
using Ships;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Components;

namespace CoreGameplay
{
	public class GameController : MonoSingleton<GameController>
	{
		[SerializeField] private ShipController playerCreatures;
		[SerializeField] private ShipController aiCreatures;

		public static event Action<GameTeamType> TurnStarted;
		public static event Action<GameTeamType> TurnEnded;
		public static event Action<GameTeamType> TeamWon;
		public static event Action GameInitialized;

		public GameTeamType CurrentTeamTurn { get; private set; }
		public Ship[] PlayerCreatures => playerCreatures.GetAliveCreatures();
		public Ship[] AICreatures => aiCreatures.GetAliveCreatures();

		public override void Initialize()
		{
			playerCreatures.AllCreaturesActed += OnAllCreaturesActed;
			aiCreatures.AllCreaturesActed += OnAllCreaturesActed;
			playerCreatures.AllCreaturesDied += OnAllCreaturesDied;
			aiCreatures.AllCreaturesDied += OnAllCreaturesDied;
			UIMainCanvasController.Instance.EndTurnButtonClicked.AddListener(OnEndTurnButtonClicked);
			UIMainCanvasController.Instance.RestartButtonClicked.AddListener(OnRestartButtonClicked);

			StartNextTurn();
			GameInitialized?.Invoke();
			Time.timeScale = 1f;
		}

		private void OnDestroy()
		{
			playerCreatures.AllCreaturesActed -= OnAllCreaturesActed;
			aiCreatures.AllCreaturesActed -= OnAllCreaturesActed;
			playerCreatures.AllCreaturesDied -= OnAllCreaturesDied;
			aiCreatures.AllCreaturesDied -= OnAllCreaturesDied;
			UIMainCanvasController.Instance.EndTurnButtonClicked.RemoveListener(OnEndTurnButtonClicked);
			UIMainCanvasController.Instance.RestartButtonClicked.RemoveListener(OnRestartButtonClicked);
			TurnStarted = null;
			TurnEnded = null;
			TeamWon = null;
		}

		private void OnAllCreaturesActed(GameTeamType team)
		{
			if (team == GameTeamType.AI)
				StartNextTurn();
		}

		private void OnAllCreaturesDied(GameTeamType team)
		{
			TeamWon?.Invoke(team == GameTeamType.AI ? GameTeamType.Player : GameTeamType.AI);
			Time.timeScale = 0f;
		}

		private void OnEndTurnButtonClicked()
		{
			if (CurrentTeamTurn == GameTeamType.Player)
				StartNextTurn();
		}

		private void OnRestartButtonClicked()
		{
			SceneManager.LoadScene(0);
		}

		private void StartNextTurn()
		{
			TurnEnded?.Invoke(CurrentTeamTurn);
			CurrentTeamTurn = CurrentTeamTurn == GameTeamType.AI ? GameTeamType.Player : GameTeamType.AI;
			TurnStarted?.Invoke(CurrentTeamTurn);
		}

		private void Update()
		{
			if (!Input.anyKeyDown)
				return;

			if (Input.GetKeyDown(KeyCode.R))
				SceneManager.LoadScene(0);
			if (Input.GetKeyDown(KeyCode.Space))
				OnEndTurnButtonClicked();
			if (Input.GetKeyDown(KeyCode.Escape))
				Application.Quit();
		}
	}
}
