using UnityEngine;
using System.Collections;

public class BeginTurnState : IGameState {

	public void EnterState (){

		GameManager.instance.game.turnNumber++;

		Debug.Log ("Begin New Turn: " + GameManager.instance.game.turnNumber);

//		// check if Intel should be spawned
//		if (GameManager.instance.game.turnNumber == GameManager.instance.game.turnToSpawnNextIntel) {
//			GameManager.instance.game.SpawnIntel ();
//		}

		GameManager.instance.ChangeGameState (GameManager.instance.beginPlayerPhase);
	}

	public void UpdateState () {

	}

	public void ExitState (){
		Debug.Log ("Exiting Begin New Turn Phase");
	}
}
