using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Agent Capture Intel")]
public class Agent_CaptureIntel : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_agentInFocus != null) {t.m_henchmenIDs.Add (a.m_agentInFocus.m_agent.id);}

		foreach (TokenSlot ts in a.m_region.assetTokens) {

			if (ts.m_assetToken != null && ts.m_assetToken == GameManager.instance.m_intel) {

				// intel found

				GameManager.instance.game.IntelCaptured ();
				a.m_region.RemoveAssetToken (ts);
				break;
			}
		}

		if (a.m_agentInFocus.m_agentEvents.Contains (AgentWrapper.AgentEvents.IntelFound)) {

			a.m_agentInFocus.m_agentEvents.Remove (AgentWrapper.AgentEvents.IntelFound);
		}

		foreach (AgentWrapper aw in a.m_agents) {

			if (aw.m_agentEvents.Contains (AgentWrapper.AgentEvents.IntelFound)) {
				aw.m_agentEvents.Remove (AgentWrapper.AgentEvents.IntelFound);
			}
		}

		t.m_iconType = TurnResultsEntry.IconType.Agent;
		t.m_resultsText = a.m_agentInFocus.m_agent.henchmenName.ToUpper() + " uncovers the Intel!";
		t.m_resultType = GameEvent.Henchmen_MissionCompleted;
		GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
	}

	public override bool IsValid ()
	{

		return false;
	}
}
