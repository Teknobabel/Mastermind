using UnityEngine;
using System.Collections;

public class AgentPhaseState : IGameState {

	public void EnterState (){

		Debug.Log ("Starting Agent Phase");

		GameManager.instance.ChangeGameState (GameManager.instance.endTurnPhase);
	}

	public void UpdateState () {

	}

	public void ExitState (){
		Debug.Log ("Exiting Agent Phase");
	}
}
