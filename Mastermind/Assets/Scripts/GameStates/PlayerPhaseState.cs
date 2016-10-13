using UnityEngine;
using System.Collections;

public class PlayerPhaseState : IGameState {

	public void EnterState (){

		Debug.Log ("Entering Player Phase");

//		GameManager.instance.ChangeGameState (GameManager.instance.missionPhase);
	}

	public void UpdateState () {

	}

	public void ExitState (){

		Debug.Log ("Exiting Player Phase");
	}
}
