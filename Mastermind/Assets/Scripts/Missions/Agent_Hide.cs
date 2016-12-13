using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Agent Hide")]
public class Agent_Hide : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		Debug.Log ("Completing Mission: Hide Agent");
		base.CompleteMission (a);

		if (a.m_success) {
			
			if (a.m_agentInFocus.m_vizState == AgentWrapper.VisibilityState.Visible) {

				a.m_agentInFocus.ChangeVisibilityState (AgentWrapper.VisibilityState.Hidden);

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_iconType = TurnResultsEntry.IconType.Agent;
				t.m_resultsText = a.m_agentInFocus.m_agent.henchmenName.ToUpper () + " disappears!";
				t.m_resultType = GameEvent.Agent_BecameVisible;
				GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
			}
		}
	}

	public override bool IsValid ()
	{
		return false;
	}
}
