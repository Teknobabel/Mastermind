using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Agent Invade Base")]
public class Agent_InvadeBase : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);
		a.m_success = true;

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_agentInFocus != null) {t.m_henchmenIDs.Add (a.m_agentInFocus.m_agent.id);}

		if (a.m_success) {

			t.m_iconType = TurnResultsEntry.IconType.Agent;
			t.m_resultsText = GameManager.instance.game.player.orgName.ToUpper () + "'s base is being invaded!\n";

			// gather henchmen and agents

			List<AgentWrapper> agents = a.m_agents;
			List<TraitData> agentCombinedTraits = new List<TraitData> ();
			List<Henchmen> henchmen = new List<Henchmen> ();
			bool didWin = true;
			int sneakAttackBonus = 0;
//			List<TraitData> henchmenCombinedTraits = new List<TraitData> ();

			Base b = GameManager.instance.game.player.orgBase;

			foreach (AgentWrapper aw in agents) {

				if (aw.m_vizState == AgentWrapper.VisibilityState.Hidden) {

					// security center has a chance to cancel sneak attack bonus

					if (Random.Range (0.0f, 1.0f) >= b.chanceToNegateAmbushBonus) {

						sneakAttackBonus += 30;
					} else {

						t.m_resultsText += "\n" + aw.m_agent.henchmenName.ToUpper() + " is detected by security!";
					}

					aw.ChangeVisibilityState (AgentWrapper.VisibilityState.Visible);
				}

				agentCombinedTraits.AddRange(aw.m_agent.GetAllTraits());
				agentCombinedTraits.Add (aw.m_agent.statusTrait);
			}

			foreach (Region.HenchmenSlot hs in a.m_region.henchmenSlots) {
				
				if (hs.m_state == Region.HenchmenSlot.State.Occupied_Player && hs.m_henchmen.statusTrait.m_type != TraitData.TraitType.Incapacitated) {
					
					henchmen.Add (hs.m_henchmen);
				}
			}

			t.m_resultsText += "\nBase is being invaded by: ";

			foreach (AgentWrapper aw in agents) {
				t.m_resultsText += aw.m_agent.name.ToUpper() + ", ";
			}

			t.m_resultsText += "\n\nBase is being defended by: ";

			// place henchmen in base

			foreach (Henchmen h in henchmen) {
				
				t.m_resultsText += h.name.ToUpper() + ", ";
				Base.Floor randFloor = b.m_floors[Random.Range(0, b.m_floors.Count)];
				randFloor.m_guards.Add (h);

			}

			// go through each floor while there are agents left

			foreach (Base.Floor f in b.m_floors) {

				t.m_resultsText += "\n\nThe Agents reach Floor " + f.m_floorNumber.ToString() + ": ";
				if (f.m_floorState == Base.FloorState.Empty) {

					t.m_resultsText += "Empty";
				} else {
					t.m_resultsText += f.m_installedUpgrade.m_name.ToUpper();
				}

				int agentScore = sneakAttackBonus;
				int baseScore = f.m_baseDefense;

				List<TraitData> henchmenCombinedTraits = new List<TraitData> ();


				t.m_resultsText += "\nThis Floor is being ivaded by: ";

				foreach (AgentWrapper aw in agents) {
					t.m_resultsText += aw.m_agent.name.ToUpper() + ", ";
				}

				t.m_resultsText += "\nThis Floor is being defended by: ";

				foreach (Henchmen h in f.m_guards) {

					t.m_resultsText += h.henchmenName.ToUpper() + ", ";
					henchmenCombinedTraits.AddRange (h.GetAllTraits ());
					henchmenCombinedTraits.Add (h.statusTrait);
				}

				// calculate aggregate agent score

				foreach (TraitData td in agentCombinedTraits) {

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

				foreach (TraitData td in henchmenCombinedTraits) {

					switch (td.m_type) {

					case TraitData.TraitType.Bodyguard:
						baseScore += 20;
						break;
					case TraitData.TraitType.MartialArtist:
						baseScore += 10;
						break;
					case TraitData.TraitType.SharpShooter:
						baseScore += 15;
						break;
					case TraitData.TraitType.Tough:
						baseScore += 10;
						break;
					case TraitData.TraitType.Strong:
						baseScore += 10;
						break;
					case TraitData.TraitType.Mercenary:
						baseScore += 15;
						break;
					case TraitData.TraitType.Assassin:
						baseScore += 20;
						break;
					case TraitData.TraitType.Spy:
						baseScore += 10;
						break;
					case TraitData.TraitType.Thief:
						baseScore += 10;
						break;
					case TraitData.TraitType.Intimidating:
						baseScore += 15;
						break;
					case TraitData.TraitType.Injured:
						baseScore -= 10;
						break;
					case TraitData.TraitType.Critical:
						baseScore -= 20;
						break;
					case TraitData.TraitType.Weak:
						baseScore -= 15;
						break;
					case TraitData.TraitType.Coward:
						baseScore -= 20;
						break;
					}
				}

				t.m_resultsText += "\nAgent's score: " + agentScore.ToString();
				t.m_resultsText += "\nBase's score: " + baseScore.ToString();

				// any losers have a chance to get injured and / or flee

				List<Henchmen> losers = new List<Henchmen> ();

				if (agentScore >= baseScore) {

					losers.AddRange (f.m_guards);
				} else {
					foreach (AgentWrapper aw in agents) {

						losers.Add (aw.m_agent);
					}
				}

				foreach (Henchmen h in losers) {

					float injuredChance = 0.4f;

					if (h.HasTrait (TraitData.TraitType.Weak)) { injuredChance -= 0.2f; }
					if (h.HasTrait (TraitData.TraitType.Strong)) { injuredChance += 0.2f; }

					if (Random.Range (0.0f, 1.0f) > injuredChance) {

						// loser is injured

						switch (h.statusTrait.m_type) {
						case TraitData.TraitType.Healthy:

							h.UpdateStatusTrait (GameManager.instance.m_statusTraits [1]);
							t.m_resultsText += "\n<color=red>" + h.henchmenName.ToUpper () + " is Injured!</color>";

							break;
						case TraitData.TraitType.Injured:

							h.UpdateStatusTrait (GameManager.instance.m_statusTraits [2]);
							t.m_resultsText += "\n<color=red>" + h.henchmenName.ToUpper () + " is Critically Injured!</color>";

							break;
						case TraitData.TraitType.Critical:

							h.UpdateStatusTrait (GameManager.instance.m_statusTraits [3]);
							t.m_resultsText += "\n<color=red>" + h.henchmenName.ToUpper () + " is Incapacitated!</color>";

							break;
						}
					}
				}

				for (int i = 0; i < f.m_guards.Count; i++) {

					Henchmen h = henchmen [i];

					if (h.statusTrait.m_type != TraitData.TraitType.Healthy) {

						float fleeChance = 0.75f;

						if (h.statusTrait.m_type == TraitData.TraitType.Incapacitated) {

							fleeChance = 0.0f;
						}
						else if (h.statusTrait.m_type == TraitData.TraitType.Critical) {

							fleeChance = 0.5f;
						}

						if (h.HasTrait (TraitData.TraitType.Coward)) {

							fleeChance -= 0.1f;
						}

						if (Random.Range (0.0f, 1.0f) > fleeChance) {
							
							// send to limbo
							h.currentRegion.RemoveHenchmen (h);
							henchmen.Remove (h);
							GameManager.instance.game.limbo.AddHenchmen (h);

							t.m_resultsText += "\n<color=yellow>" + h.henchmenName.ToUpper () + " flees!</color>";

//							i = 0;
						}
					}
				}

				for (int i = 0; i < agents.Count; i++) {

					AgentWrapper aw = agents [i];

					if (aw.m_agent.statusTrait.m_type != TraitData.TraitType.Healthy) {

						float fleeChance = 0.75f;

						if (aw.m_agent.statusTrait.m_type == TraitData.TraitType.Incapacitated) {

							fleeChance = 0.0f;
						}
						else if (aw.m_agent.statusTrait.m_type == TraitData.TraitType.Critical) {

							fleeChance = 0.5f;
						}

						if (aw.m_agent.HasTrait (TraitData.TraitType.Coward)) {

							fleeChance -= 0.1f;
						}

						if (Random.Range (0.0f, 1.0f) > fleeChance) {
							
							// send to limbo

							aw.ChangeAIState (GameManager.instance.game.agentOrganization.agentAIState_Idle);
							aw.m_agent.currentRegion.RemoveAgent (aw.m_agent);
							GameManager.instance.game.limbo.AddAgent (aw);
							agents.RemoveAt (i);
							t.m_resultsText += "\n<color=yellow>" + aw.m_agent.henchmenName.ToUpper () + " flees!</color>";

							i = 0;
						}
					}
				}


				// if there are any agents left, proceed to the next level

				if (agents.Count == 0) {

					t.m_resultsText += "\n\nThe invasion is repelled";
					didWin = false;
					break;
				}

			}

			if (didWin) {
				
				t.m_resultsText += "\n\nYou have been apprehended by the Agents!";
				t.m_resultsText += "\nThe game is over!";

			} else {

				// Remove all guards from base

				foreach (Base.Floor f in b.m_floors) {

					f.m_guards.Clear ();
				}
			}


			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
//			Debug.Log (t.m_resultsText);
		} else {

		}

	}

	public override bool IsValid ()
	{
		return false;
	}
}
