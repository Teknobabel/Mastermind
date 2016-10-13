using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeginPlayerPhaseState : IGameState {

	public void EnterState (){

		Debug.Log ("Entering Begin Player Phase");

		// update command points

		Organization player = GameManager.instance.game.player;
		player.RefillCommandPool ();
		player.UseCommandPoints (player.costPerTurn);

		// refill any empty henchmen for hire slots

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
				}
			}

		}

		// progress turn to next phase

		GameManager.instance.ChangeGameState (GameManager.instance.playerPhase);
	}

	public void UpdateState () {

	}

	public void ExitState (){
		Debug.Log ("Exiting Begin Player Phase");
	}
}
