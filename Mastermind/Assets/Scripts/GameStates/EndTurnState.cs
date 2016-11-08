using UnityEngine;
using System.Collections;

public class EndTurnState : IGameState {

	public void EnterState (){

		Debug.Log ("Begin End Turn Phase");

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
