using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Track Agent")]
public class TrackAgent : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);
		a.m_success = true;
		if (a.m_success) {

			if (a.m_agentInFocus != null && a.m_agentInFocus.m_vizState == AgentWrapper.VisibilityState.Visible) {

				a.m_agentInFocus.ChangeVisibilityState (AgentWrapper.VisibilityState.Tracked);

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_resultsText = a.m_mission.m_name.ToUpper () + ": " + a.m_agentInFocus.m_agent.henchmenName.ToUpper() + " mission complete!";
				t.m_resultsText += "\nTracking device successfully placed on " + a.m_agentInFocus.m_agent.henchmenName.ToUpper() + ".";
				t.m_resultType = GameEvent.Henchmen_MissionCompleted;
				GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
			}

		} else {

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + ": " + a.m_region.regionName.ToUpper() + " mission failed";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override string GetNameText ()
	{
		string s = m_name + "\n";

		if (GameManager.instance.currentMissionWrapper.m_agentInFocus != null) {
			s += "<size=18>" + GameManager.instance.currentMissionWrapper.m_agentInFocus.m_agent.henchmenName.ToUpper() + "</size>";
		}

		return s;
	}

	public override bool IsValid ()
	{
//		foreach (Region.HenchmenSlot hs in GameManager.instance.currentMissionWrapper.m_region.henchmenSlots) {
		Debug.Log(GameManager.instance.currentMissionWrapper.m_agentInFocus);
		if (GameManager.instance.currentMissionWrapper.m_agentInFocus.m_vizState == AgentWrapper.VisibilityState.Visible) {

				return true;
//			}
		}

		return false;
	}
}
