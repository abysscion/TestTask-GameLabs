using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Components;

namespace CoreGameplay
{
	public class UIMainCanvasController : MonoSingleton<UIMainCanvasController>
	{
		[SerializeField] private GameObject gameOverPanel;
		[SerializeField] private TMP_Text currentTurnText;
		[SerializeField] private TMP_Text winnerText;
		[SerializeField] private Button endTurnButton;
		[SerializeField] private Button restartButton;

		public Button.ButtonClickedEvent EndTurnButtonClicked => endTurnButton.onClick;
		public Button.ButtonClickedEvent RestartButtonClicked => restartButton.onClick;

		public override void Initialize()
		{
			GameController.TurnStarted += OnTurnStarted;
			GameController.TeamWon += OnAnyTeamWon;
			OnTurnStarted(GameController.Instance.CurrentTeamTurn);
		}

		private void OnDestroy()
		{
			GameController.TurnStarted -= OnTurnStarted;
		}

		private void OnTurnStarted(GameTeamType teamTurn)
		{
			endTurnButton.gameObject.SetActive(teamTurn == GameTeamType.Player);
			currentTurnText.text = $"{teamTurn}";
		}

		private void OnAnyTeamWon(GameTeamType teamTurn)
		{
			gameOverPanel.SetActive(true);
			winnerText.text = $"Winner: {teamTurn}";
		}
	}
}
