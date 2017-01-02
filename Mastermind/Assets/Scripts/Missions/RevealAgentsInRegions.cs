using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Reveal Agents in Region")]
public class RevealAgentsInRegions : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();

		if (a.m_henchmenInFocus != null) {
			t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);
			t.m_resultsText = a.m_henchmenInFocus.henchmenName.ToUpper () + " begins sweeping " + a.m_regionInFocus.regionName.ToUpper ();
		} else {
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission begins in " + a.m_regionInFocus.regionName.ToUpper ();
		}

		if (a.m_success) {

			bool agentsFound = false;

			foreach (Region.HenchmenSlot hs in a.m_regionInFocus.henchmenSlots) {

				if (hs.m_state == Region.HenchmenSlot.State.Occupied_Agent && hs.m_agent.m_vizState == AgentWrapper.VisibilityState.Hidden) {

					hs.m_agent.ChangeVisibilityState (AgentWrapper.VisibilityState.Visible);
					t.m_resultsText += "\n" + hs.m_agent.m_agent.henchmenName.ToUpper() + " revealed!";
					agentsFound = true;
				}
			}

			if (!agentsFound) {

				t.m_resultsText += "\nNothing found.";
			}

			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

		} else {
			
			t.m_resultsText += "\nNothing found.";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override string GetNameText ()
	{
		string s = m_name + "\n";

		if (GameManager.instance.currentMissionWrapper.m_regionInFocus != null) {
			s += "<size=18>" + GameManager.instance.currentMissionWrapper.m_regionInFocus.regionName.ToUpper() + "</size>";
		}

		return s;
	}

	public override bool IsValid ()
	{
		if (!base.IsValid ()) { return false;}		MissionWrapper mw = GameManager.instance.currentMissionWrapper;

		if (mw.m_scope == TargetType.Floor && mw.m_floorInFocus.m_installedUpgrade.m_assetType == Asset.AssetType.LairUpgrade_ControlRoom)
		{

			return true;
		}

		return false;
	}
}
