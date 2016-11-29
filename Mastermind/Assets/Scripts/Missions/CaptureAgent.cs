using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Capture Agent")]
public class CaptureAgent : MissionBase {

	public Asset m_requiredAsset;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		if (a.m_success) {

			int agentScore = 0;
			int henchmenScore = 0;

			List<TraitData> agentTraits = new List<TraitData> ();
			List<TraitData> henchmenTraits = new List<TraitData> ();

			foreach (Region.HenchmenSlot hs in a.m_region.henchmenSlots) {

				if (hs.m_state == Region.HenchmenSlot.State.Occupied_Player) {

					henchmenTraits.AddRange (hs.m_henchmen.GetAllTraits());
					henchmenTraits.Add (hs.m_henchmen.statusTrait);

				} else if (hs.m_state == Region.HenchmenSlot.State.Occupied_Agent && hs.m_agent.m_agent.id == a.m_agentInFocus.m_agent.id) {

					agentTraits.AddRange (hs.m_agent.m_agent.GetAllTraits());
					agentTraits.Add (hs.m_agent.m_agent.statusTrait);
				}
			}

			foreach (TraitData td in agentTraits) {

				switch (td.m_type) {

				case TraitData.TraitType.Agent:
					agentScore += 30;
					break;
				case TraitData.TraitType.MartialArtist:
					agentScore += 10;
					break;
				case TraitData.TraitType.SharpShooter:
					agentScore += 15;
					break;
				case TraitData.TraitType.Tough:
					agentScore += 10;
					break;
				case TraitData.TraitType.Strong:
					agentScore += 10;
					break;
				case TraitData.TraitType.Mercenary:
					agentScore += 15;
					break;
				case TraitData.TraitType.Assassin:
					agentScore += 20;
					break;
				case TraitData.TraitType.Spy:
					agentScore += 10;
					break;
				case TraitData.TraitType.Thief:
					agentScore += 10;
					break;
				case TraitData.TraitType.Intimidating:
					agentScore += 15;
					break;
				case TraitData.TraitType.Injured:
					agentScore -= 10;
					break;
				case TraitData.TraitType.Critical:
					agentScore -= 20;
					break;
				case TraitData.TraitType.Weak:
					agentScore -= 15;
					break;
				case TraitData.TraitType.Coward:
					agentScore -= 20;
					break;
				}
			}

			// calculate aggregate henchmen score

			foreach (TraitData td in henchmenTraits) {

				switch (td.m_type) {

				case TraitData.TraitType.Bodyguard:
					henchmenScore += 20;
					break;
				case TraitData.TraitType.MartialArtist:
					henchmenScore += 10;
					break;
				case TraitData.TraitType.SharpShooter:
					henchmenScore += 15;
					break;
				case TraitData.TraitType.Tough:
					henchmenScore += 10;
					break;
				case TraitData.TraitType.Strong:
					henchmenScore += 10;
					break;
				case TraitData.TraitType.Mercenary:
					henchmenScore += 15;
					break;
				case TraitData.TraitType.Assassin:
					henchmenScore += 20;
					break;
				case TraitData.TraitType.Spy:
					henchmenScore += 10;
					break;
				case TraitData.TraitType.Thief:
					henchmenScore += 10;
					break;
				case TraitData.TraitType.Intimidating:
					henchmenScore += 15;
					break;
				case TraitData.TraitType.Injured:
					henchmenScore -= 10;
					break;
				case TraitData.TraitType.Critical:
					henchmenScore -= 20;
					break;
				case TraitData.TraitType.Weak:
					henchmenScore -= 15;
					break;
				case TraitData.TraitType.Coward:
					henchmenScore -= 20;
					break;
				}
			}

			//			TurnResultsEntry t = new TurnResultsEntry ();
			//			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission complete!";
			//			t.m_resultsText += "\n" + a.m_assetInFocus.m_name.ToUpper() + " is now in orbit.";
			//			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			//			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

		} else {

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultsText += a.m_agentInFocus.m_agent.henchmenName.ToUpper() + " eludes the henchmen.";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
		// valid if there is a non-incapacitated Agent in the region

		AgentWrapper aw = GameManager.instance.currentMissionWrapper.m_agentInFocus;

		if (GameManager.instance.game.player.orgBase.m_currentAssets.Contains(m_requiredAsset) && aw.m_vizState != AgentWrapper.VisibilityState.Hidden && aw.m_agent.statusTrait.m_type != TraitData.TraitType.Incapacitated) {

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
