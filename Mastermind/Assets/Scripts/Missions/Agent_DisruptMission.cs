﻿using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Agent Disrupt Mission")]
public class Agent_DisruptMission : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);


		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_agentInFocus != null) {t.m_henchmenIDs.Add (a.m_agentInFocus.m_agent.id);}

		if (a.m_success) {

			MissionWrapper missionToDisrupt = null;

			foreach (MissionWrapper mw in GameManager.instance.game.player.activeMissions) {

				if (mw.m_region != null && mw.m_region.id == a.m_region.id) {

					missionToDisrupt = mw;

					break;
				}
			}

			if (missionToDisrupt != null) {

				float cancelChance = 0.75f;

				if (Random.Range (0.0f, 1.0f) < cancelChance) {

					GameManager.instance.game.player.CancelMission (missionToDisrupt);

					t.m_iconType = TurnResultsEntry.IconType.Agent;
					t.m_resultsText = a.m_agentInFocus.m_agent.henchmenName.ToUpper () + " shows up to disrupt the mission: " + missionToDisrupt.m_mission.m_name.ToUpper() + "!";
					t.m_resultsText += "\n" + a.m_agentInFocus.m_agent.henchmenName.ToUpper () + " is successful!";
					t.m_resultsText = missionToDisrupt.m_mission.m_name.ToUpper() + " is cancelled!";
					t.m_resultType = GameEvent.Henchmen_MissionDisrupted;
					GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
				} else {

					t.m_iconType = TurnResultsEntry.IconType.Agent;
					t.m_resultsText = a.m_agentInFocus.m_agent.henchmenName.ToUpper () + " shows up to disrupt the mission: " + missionToDisrupt.m_mission.m_name.ToUpper() + "!";
					t.m_resultsText += "\n" + a.m_agentInFocus.m_agent.henchmenName.ToUpper () + " fails.";
					t.m_resultType = GameEvent.Henchmen_MissionDisrupted;
					GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

				}
			}
				


		}
	}

	public override bool IsValid ()
	{
		if (!base.IsValid ()) { return false;}		return false;
	}
}
