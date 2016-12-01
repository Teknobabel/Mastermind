using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Agent Engage Henchmen")]
public class Agents_EngageHenchmen : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		TurnResultsEntry t = new TurnResultsEntry ();
		t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission underway!";
		t.m_resultsText += "\n";

		foreach (AgentWrapper agent in a.m_agents) {

			t.m_resultsText += agent.m_agent.henchmenName.ToUpper() + ", ";
		}

		t.m_resultsText += " attempt to engage ";

		List<Henchmen> bribeableHenchmen = new List<Henchmen> ();

		foreach (Region.HenchmenSlot hs in a.m_region.henchmenSlots) {

			if (hs.m_state == Region.HenchmenSlot.State.Occupied_Player) {

				a.m_henchmen.Add (hs.m_henchmen);

				if (hs.m_henchmen.HasTrait (TraitData.TraitType.Greed) || hs.m_henchmen.HasTrait (TraitData.TraitType.Coward) || hs.m_henchmen.HasTrait (TraitData.TraitType.Paranoid) || hs.m_henchmen.HasTrait (TraitData.TraitType.Chaotic)) {

					bribeableHenchmen.Add (hs.m_henchmen);
				}
			}
		}

		foreach (Henchmen h in a.m_henchmen) {

			t.m_resultsText += h.henchmenName.ToUpper() + ", ";
		}

		int agentScore = 0;
		int henchmenScore = 0;

		List<TraitData> agentTraits = new List<TraitData> ();
		List<TraitData> henchmenTraits = new List<TraitData> ();

		// bribe agents to leave the region

		while (bribeableHenchmen.Count > 0) {

			Henchmen h = bribeableHenchmen [0];
			bribeableHenchmen.RemoveAt (0);

			float bribeChance = 0.3f;

			if (Random.Range (0.0f, 1.0f) < bribeChance) {

				// bribe is successful, move henchmen to limbo

				a.m_henchmen.Remove (h);
				h.currentRegion.RemoveHenchmen (h);
				GameManager.instance.game.limbo.AddHenchmen (h);

				t.m_resultsText += "\nThe agents successfully bribe " + h.henchmenName.ToUpper() + "!";
				t.m_resultsText += "\n" + h.henchmenName.ToUpper() + " flees!";
			}
		}

		// gather trait list for any remaining henchmen

		foreach (Region.HenchmenSlot hs in a.m_region.henchmenSlots) {

			if (hs.m_state == Region.HenchmenSlot.State.Occupied_Player) {

				henchmenTraits.AddRange (hs.m_henchmen.GetAllTraits());
				henchmenTraits.Add (hs.m_henchmen.statusTrait);

			} else if (hs.m_state == Region.HenchmenSlot.State.Occupied_Agent) {

				agentTraits.AddRange (hs.m_agent.m_agent.GetAllTraits());
				agentTraits.Add (hs.m_agent.m_agent.statusTrait);
			}
		}

		// check if any henchmen remain, in case all present were successfully bribed

		if (henchmenTraits.Count == 0) {

			t.m_resultsText += "\nNo henchmen remain, the agents are successful!";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
			return;
		}

		// calculate scores based on traits

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

		int scoreDifference = agentScore - henchmenScore;

		if (scoreDifference < 0) {

			// agents have injury chance

			foreach (AgentWrapper agent in a.m_agents) {

				float injuryChance = 0.6f;

				if (agent.m_agent.HasTrait (TraitData.TraitType.Weak)) {

					injuryChance -= 0.2f;
				}

				if (Random.Range (0.0f, 1.0f) > injuryChance) {

					IncurInjury (agent.m_agent);

					t.m_resultsText += "\n" + agent.m_agent.henchmenName.ToUpper() + " is " + agent.m_agent.statusTrait.m_name + "!";
				}
			}

		} else if (scoreDifference >= 0 && scoreDifference < 30) {

			// henchmen have a chance to receive injury

			foreach (Henchmen h in a.m_henchmen) {

				float injuryChance = 0.6f;

				if (h.HasTrait (TraitData.TraitType.Weak)) {

					injuryChance -= 0.2f;
				}

				if (Random.Range (0.0f, 1.0f) > injuryChance) {

					IncurInjury (h);

					t.m_resultsText += "\n" + h.henchmenName.ToUpper() + " is " + h.statusTrait.m_name + "!";
				}
			}

		} else if (scoreDifference >= 30 && scoreDifference < 60) {

			// henchmen receive injury

			foreach (Henchmen h in a.m_henchmen) {

				IncurInjury (h);
				t.m_resultsText += "\n" + h.henchmenName.ToUpper() + " is " + h.statusTrait.m_name + "!";
				
			}

		} else if (scoreDifference >= 60) {

			// henchmen receive critical injury
			foreach (Henchmen h in a.m_henchmen) {

				IncurInjury (h);
				IncurInjury (h);
				t.m_resultsText += "\n" + h.henchmenName.ToUpper() + " is " + h.statusTrait.m_name + "!";

			}
		}

		// send anyone that is incapacitated to limbo

		foreach (Henchmen h in a.m_henchmen) {

			if (h.statusTrait.m_type == TraitData.TraitType.Incapacitated) {

				a.m_henchmen.Remove (h);
				h.currentRegion.RemoveHenchmen (h);
				GameManager.instance.game.limbo.AddHenchmen (h);

				t.m_resultsText += "\n" + h.henchmenName.ToUpper() + " disappears!";
			}
		}

		foreach (AgentWrapper agent in a.m_agents) {

			if (agent.m_agent.statusTrait.m_type == TraitData.TraitType.Incapacitated) {

				a.m_agents.Remove (agent);
				agent.m_agent.currentRegion.RemoveAgent (agent.m_agent);
				GameManager.instance.game.limbo.AddAgent (agent);

				t.m_resultsText += "\n" + agent.m_agent.henchmenName.ToUpper() + " disappears!";
			}
		}

		// if all henchmen are gone, remove henchmen found event from remaining agents

		if (a.m_henchmen.Count == 0 && a.m_agents.Count > 0) {

			foreach (AgentWrapper aw in a.m_agents) {

				if (aw.m_agentEvents.Contains (AgentWrapper.AgentEvents.HenchmenFound)) {

					aw.m_agentEvents.Remove (AgentWrapper.AgentEvents.HenchmenFound);
				}
			}
		}

		t.m_resultType = GameEvent.Henchmen_MissionCompleted;
		GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
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

	public override bool IsValid ()
	{
		return false;
	}
}
