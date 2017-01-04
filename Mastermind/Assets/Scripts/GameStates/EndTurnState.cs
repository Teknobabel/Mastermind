using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndTurnState : IGameState {

	public void EnterState (){

		Debug.Log ("Begin End Turn Phase");

		// evaluate any event triggers

		List<EventTriggerBase> completedEvents = new List<EventTriggerBase> ();

		foreach (EventTriggerBase et in GameManager.instance.game.events) {

			if (et.EvaluateTrigger ()) {

				// event conditions met, execute event

				et.ExecuteEvent ();
				completedEvents.Add (et);
			}
		}

		while (completedEvents.Count > 0) {

			EventTriggerBase et = completedEvents [0];
			completedEvents.RemoveAt (0);

			if (GameManager.instance.game.events.Contains (et)) {
				GameManager.instance.game.events.Remove (et);
			}
		}

		// check if Intel should be spawned

		if (GameManager.instance.game.turnNumber == GameManager.instance.game.turnToSpawnNextIntel) {
			GameManager.instance.game.SpawnIntel ();
		}

		GameManager.instance.ChangeGameState (GameManager.instance.beginTurn);
	}

	public void UpdateState () {

	}

	public void ExitState (){
		Debug.Log ("Exiting End Turn Phase");
	}
}
