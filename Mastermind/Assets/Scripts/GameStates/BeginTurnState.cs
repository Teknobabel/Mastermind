using UnityEngine;
using System.Collections;

public class BeginTurnState : IGameState {

	public void EnterState (){

		Debug.Log ("Begin New Turn");
		GameManager.instance.game.turnNumber++;

		GameManager.instance.ChangeGameState (GameManager.instance.beginPlayerPhase);
	}

	public void UpdateState () {

	}

	public void ExitState (){
		Debug.Log ("Exiting Begin New Turn Phase");
	}
}
