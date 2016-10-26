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

		// update infamy

		if (GameManager.instance.game.turnNumber > 1) {
			int infamyGain = GameManager.instance.game.director.m_infamyGainPerTurn;
			if (infamyGain > 0) {
				player.GainInfamy (infamyGain);
			}
		}

		// progress turn to next phase

		if (GameManager.instance.game.turnNumber != 1) {
			GameManager.instance.PopMenuState ();

			foreach (KeyValuePair<int, MenuTab> pair in player.menuTabs)
			{
				if (pair.Value.m_menuState == MenuState.State.ActivityMenu) {
					ActivityMenu.instance.displayTurnResults = true;
					GameManager.instance.PushMenuState (pair.Value);
					break;
				}
			}


		}
		GameManager.instance.ChangeGameState (GameManager.instance.playerPhase);

	}

	public void UpdateState () {

	}

	public void ExitState (){
		Debug.Log ("Exiting Begin Player Phase");
	}
}
