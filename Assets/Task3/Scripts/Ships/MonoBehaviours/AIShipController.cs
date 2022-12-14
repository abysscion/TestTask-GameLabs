using System.Collections;
using CoreGameplay;

namespace Ships
{
	public class AIShipController : ShipController
	{
		protected override void Start()
		{
			base.Start();

			GameController.GameInitialized += OnGameInitialized;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			GameController.GameInitialized -= OnGameInitialized;
			GameController.TurnStarted -= OnTurnStarted;
			GameController.TurnEnded -= OnTurnEnded;
		}

		private void OnGameInitialized()
		{
			GameController.TurnStarted += OnTurnStarted;
			GameController.TurnEnded += OnTurnEnded;
			if (GameController.Instance.CurrentTeamTurn == Team)
				OnTurnStarted(Team);
		}

		private void OnTurnStarted(GameTeamType teamTurn)
		{
			if (teamTurn == Team)
				StartCoroutine(AIAttackCoroutine());
		}

		private void OnTurnEnded(GameTeamType teamTurn)
		{
			if (teamTurn == Team)
				StopAllCoroutines();
		}

		private IEnumerator AIAttackCoroutine()
		{
			yield return null;
		}
	}
}
