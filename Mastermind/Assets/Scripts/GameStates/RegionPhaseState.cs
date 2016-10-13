using UnityEngine;
using System.Collections;

public class RegionPhaseState : IGameState {

	public void EnterState (){

		Debug.Log ("Entering Region Phase");

		GameManager.instance.ChangeGameState (GameManager.instance.agentPhase);
	}

	public void UpdateState () {

	}

	public void ExitState (){
		Debug.Log ("Exiting Region Phase");
	}
}
