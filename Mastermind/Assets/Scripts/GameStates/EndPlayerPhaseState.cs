using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndPlayerPhaseState : IGameState {

	public void EnterState (){

		Debug.Log ("Starting End Player Phase Phase");

		// refill any empty henchmen for hire slots

		Organization player = GameManager.instance.game.player;

		if (player.availableHenchmen.Count < player.maxAvailableHenchmen) {

			int maxRank = GameManager.instance.game.director.m_maxStartingHenchmenLevel;
			maxRank = Mathf.Clamp (maxRank + player.currentWantedLevel, 1, 3);

			List<Henchmen> h = new List<Henchmen> ();

			foreach (Henchmen thisH in GameManager.instance.game.henchmen) {

				if (thisH.rank <= maxRank && !player.currentHenchmen.Contains (thisH) && !player.availableHenchmen.Contains (thisH)) {
					h.Add (thisH);
				}
			}

			int numToHire = player.maxAvailableHenchmen - player.availableHenchmen.Count;

			for (int i = 0; i < numToHire; i++) {
				if (h.Count > 0) {
					int rand = Random.Range (0, h.Count);
					Henchmen newH = h [rand];
					player.AddHenchmenToAvailablePool (newH);
					h.RemoveAt (rand);

					TurnResultsEntry entry = new TurnResultsEntry ();
					entry.m_resultType = GameEvent.Organization_HenchmenForHireArrived;
					entry.m_resultsText = newH.henchmenName + " is now available for hire";
					GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, entry);
				}
			}

		}

		// update infamy

		int infamyGain = GameManager.instance.game.director.m_infamyGainPerTurn;

		if (infamyGain > 0) {
			GameManager.instance.game.player.GainInfamy (infamyGain);
		}

		GameManager.instance.ChangeGameState (GameManager.instance.regionPhase);
	}

	public void UpdateState () {

	}

	public void ExitState (){
		Debug.Log ("Exiting End Player Phase Phase");
	}
}
