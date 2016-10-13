using UnityEngine;
using System.Collections;

public class MissionPhaseState : IGameState {

	public void EnterState (){

		Debug.Log ("Starting Mission Phase");

		GameManager.instance.ChangeGameState (GameManager.instance.regionPhase);
	}

	public void UpdateState () {

	}

	public void ExitState (){
		Debug.Log ("Exiting Mission Phase");
	}
}
