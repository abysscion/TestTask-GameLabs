using System;
using CoreGameplay;
using UnityEngine;

namespace Ships
{
	public class ShipController : MonoBehaviour
	{
		//[SerializeField] private CardConfigsContainer availableCards;
		[SerializeField] private GameTeamType team;
		//[SerializeField] private List<Ship> creatures;

		//protected Dictionary<Creature, CreatureDeck> creatureToCreaturesDecksDic;
		//protected HashSet<Creature> actedCreatures;

		public event Action<GameTeamType> AllCreaturesActed;
		public event Action<GameTeamType> AllCreaturesDied;

		public GameTeamType Team => team;

		protected virtual void Start()
		{
			//creatureToCreaturesDecksDic = new Dictionary<Creature, CreatureDeck>();
			//actedCreatures = new HashSet<Creature>();

			//for (int i = 0; i < creatures.Count; i++)
			//{
			//	if (TryDisableInappropriateTeamCreature(creatures[i]))
			//	{
			//		Debug.LogWarning($"{creatures[i].name} was disabled due to mismatching team.");
			//		creatures.Remove(creatures[i]);
			//	}
			//	else
			//	{
			//		CreatureDeck creatureDeck;
			//		var creature = creatures[i];
			//		if (team == GameTeamType.Player)
			//			creatureDeck = new CreatureDeck(creature, availableCards, team);
			//		else
			//			creatureDeck = new AICreatureDeck(creature, availableCards, team);
			//		creatureToCreaturesDecksDic.Add(creature, creatureDeck);
			//		creatureDeck.CardUsed += () => OnCreatureDeckUsedCard(creatureDeck);
			//		creature.Died += () => OnCreatureDied(creature);
			//	}
			//}

			//GameController.TurnStarted += OnTurnStarted;
			//GameController.TurnEnded += OnTurnEnded;
		}

		protected virtual void OnDestroy()
		{
			GameController.TurnStarted -= OnTurnStarted;
			GameController.TurnEnded -= OnTurnEnded;
		}

		public Ship[] GetAliveCreatures()
		{
			//var result = new Creature[creatureToCreaturesDecksDic.Keys.Count];
			//creatureToCreaturesDecksDic.Keys.CopyTo(result, 0);
			//return result;
			return null;
		}

		private void OnTurnStarted(GameTeamType teamTurn)
		{
			if (teamTurn != team)
				return;
		}

		private void OnTurnEnded(GameTeamType teamTurn)
		{
			//if (teamTurn != team)
			//	return;

			//actedCreatures.Clear();
		}

		private void OnCreatureDied(Ship creature)
		{
			//creatureToCreaturesDecksDic.Remove(creature);
			//creatures.Remove(creature);
			//Destroy(creature.gameObject);
			//if (creatures.Count <= 0)
			//	AllCreaturesDied?.Invoke(team);
		}

		//private void OnCreatureDeckUsedCard(CreatureDeck creatureDeck)
		//{
		//	actedCreatures.Add(creatureDeck.Creature);
		//	if (actedCreatures.Count == creatureToCreaturesDecksDic.Keys.Count)
		//		AllCreaturesActed?.Invoke(team);
		//}
	}
}
