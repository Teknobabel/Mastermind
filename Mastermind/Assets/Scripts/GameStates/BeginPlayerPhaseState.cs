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

		// progress turn to next phase

		// navigate to menu root if not already there to add activity menu

		if (GameManager.instance.currentMenuState != MenuState.State.TabMenu) {
			GameManager.instance.targetMenuState = MenuState.State.TabMenu;
			GameManager.instance.PopMenuState ();
		}

		foreach (KeyValuePair<int, MenuTab> pair in TabMenu.instance.menuTabs)
		{
			if (pair.Value.m_menuState == MenuState.State.ActivityMenu) {
				ActivityMenu.instance.displayTurnResults = true;
				GameManager.instance.PushMenuState (pair.Value);
				break;
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
