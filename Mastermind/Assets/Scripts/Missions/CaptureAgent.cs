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

			TurnResultsEntry t = new TurnResultsEntry ();
			if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission underway!";
			t.m_resultsText += "\n";

			foreach (Henchmen h in a.m_henchmen) {

				t.m_resultsText += h.henchmenName.ToUpper() + ", ";
			}

			t.m_resultsText += " attempt to locate " + a.m_agentInFocus.m_agent.henchmenName.ToUpper ();

			t.m_resultsText += "\n" + a.m_agentInFocus.m_agent.henchmenName.ToUpper() + " located!";

			int agentScore = 0;
			int henchmenScore = 500;

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

			int scoreDifference = henchmenScore - agentScore;



			if (scoreDifference < 0) {

				// if agent wins, henchmen have chance to get injured

				t.m_resultsText += "\n" + a.m_agentInFocus.m_agent.henchmenName.ToUpper () + " gets the upper hand!";

				foreach (Henchmen h in a.m_henchmen) {

					float injuryChance = 0.6f;

					if (h.HasTrait (TraitData.TraitType.Weak)) {

						injuryChance -= 0.2f;
					}

					if (Random.Range (0.0f, 1.0f) > injuryChance) {

						IncurInjury (h);

						t.m_resultsText += "\n" + h.henchmenName.ToUpper () + " is " + h.statusTrait.m_name + "!";
					}
				}

			} else if (scoreDifference >= 0 && scoreDifference < 30 && Random.Range (0.0f, 1.0f) > 0.45f) {

				// if score difference is minimal, agent has chance to escape

				t.m_resultsText += "\n" + a.m_agentInFocus.m_agent.henchmenName.ToUpper () + " manages to escape!";

			} else if (scoreDifference >= 30 && scoreDifference < 60) {
				
				// if score difference is moderate, agent is captured

				GameManager.instance.game.player.orgBase.AgentCaptured (a.m_agentInFocus);
				t.m_resultsText += "\n" + a.m_agentInFocus.m_agent.henchmenName.ToUpper () + " is captured!";

			} else if (scoreDifference >= 60) {

				// if score difference is large, agent has chance to be injured

				if (a.m_agentInFocus.m_agent.statusTrait.m_type != TraitData.TraitType.Critical && Random.Range (0.0f, 1.0f) > 0.65f) {

					IncurInjury (a.m_agentInFocus.m_agent);
				}

				GameManager.instance.game.player.orgBase.AgentCaptured (a.m_agentInFocus);
				t.m_resultsText += "\n" + a.m_agentInFocus.m_agent.henchmenName.ToUpper () + " is captured!";
			}

			// send anyone that is incapacitated to limbo

			foreach (Henchmen h in a.m_henchmen) {

				if (h.statusTrait.m_type == TraitData.TraitType.Incapacitated) {

					h.currentRegion.RemoveHenchmen (h);
					GameManager.instance.game.limbo.AddHenchmen (h);
				}
			}


			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

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
		if (!base.IsValid ()) { return false;}		// valid if there is a non-incapacitated Agent in the region

		AgentWrapper aw = GameManager.instance.currentMissionWrapper.m_agentInFocus;

		if (GameManager.instance.game.player.orgBase.m_currentAssets.Contains(m_requiredAsset) && aw.m_vizState != AgentWrapper.VisibilityState.Hidden && aw.m_agent.statusTrait.m_type != TraitData.TraitType.Incapacitated && 
			GameManager.instance.game.player.orgBase.CanCaptureAgent()) {

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

	private void IncurInjury (Henchmen h)
	{
		switch (h.statusTrait.m_type) {

		case TraitData.TraitType.Healthy:

			h.UpdateStatusTrait(GameManager.instance.m_statusTraits[1]);
			break;

		case TraitData.TraitType.Injured:

			h.UpdateStatusTrait(GameManager.instance.m_statusTraits[2]);
			break;

		case TraitData.TraitType.Critical:

			h.UpdateStatusTrait(GameManager.instance.m_statusTraits[3]);
			break;
		}
	}
}
