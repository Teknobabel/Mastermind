using UnityEngine;
using System.Collections;

public class MissionPhaseState : IGameState {

	public void EnterState (){

		Debug.Log ("Starting Mission Phase");

		Organization player = GameManager.instance.game.player;

		foreach (Organization.ActiveMission a in player.activeMissions) {
			
			a.m_turnsPassed++;

			TurnResultsEntry entry = new TurnResultsEntry ();
			string s = null;

			if (a.m_turnsPassed >= a.m_mission.m_numTurns) {
				a.m_mission.CompleteMission ();
				s = a.m_henchmen [0].henchmenName + " completes " + a.m_mission.m_name;
				entry.m_resultType = GameEvent.Henchmen_MissionCompleted;
			} else {
				
				s = a.m_henchmen [0].henchmenName + " continues " + a.m_mission.m_name;
				int turnsRemaining = a.m_mission.m_numTurns - a.m_turnsPassed;
				s += "\n(" + turnsRemaining.ToString () + " Turns Remaining)";
				entry.m_resultType = GameEvent.Henchmen_MissionContinued;

			}

			entry.m_resultsText = s;
			player.AddTurnResults (GameManager.instance.game.turnNumber, entry);
		}

		GameManager.instance.ChangeGameState (GameManager.instance.endPlayerPhase);
	}

	public void UpdateState () {

	}

	public void ExitState (){
		Debug.Log ("Exiting Mission Phase");
	}
}
