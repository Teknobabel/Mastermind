using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Agent Hide")]
public class Agent_Hide : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		Debug.Log ("Completing Mission: Hide Agent");
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_agentInFocus != null) {t.m_henchmenIDs.Add (a.m_agentInFocus.m_agent.id);}

		if (a.m_success) {
			
			if (a.m_agentInFocus.m_vizState == AgentWrapper.VisibilityState.Visible) {

				a.m_agentInFocus.ChangeVisibilityState (AgentWrapper.VisibilityState.Hidden);

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
