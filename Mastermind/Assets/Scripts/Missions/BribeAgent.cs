using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Bribe Agent")]
public class BribeAgent : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);
//		a.m_success = true;

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}

		if (a.m_success) {

			// send agent to Limbo if successful
			a.m_agentInFocus.m_agent.currentRegion.RemoveAgent (a.m_agentInFocus.m_agent);
			GameManager.instance.game.limbo.AddAgent (a.m_agentInFocus);

			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
			t.m_resultsText += "\n" + a.m_agentInFocus.m_agent.henchmenName.ToUpper() + " disappears.";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

		} else {

			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
		// valid if agent in focus isn't hidden

		AgentWrapper aw = GameManager.instance.currentMissionWrapper.m_agentInFocus;

		if (aw.m_vizState != AgentWrapper.VisibilityState.Hidden && aw.m_agent.statusTrait.m_type != TraitData.TraitType.Incapacitated) {

			return true;
		}

		return false;
	}

	public override string GetNameText ()
	{
		string s = m_name + "\n";

		if (GameManager.instance.currentMissionWrapper.m_agentInFocus != null) {
			s += "<size=18>" + GameManager.instance.currentMissionWrapper.m_agentInFocus.m_agent.henchmenName + "</size>";
		}

		return s;
	}
}
