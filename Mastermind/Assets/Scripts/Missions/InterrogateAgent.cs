using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Interrogate Agent")]
public class InterrogateAgent : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);
//				a.m_success = true;
	
		TurnResultsEntry t = new TurnResultsEntry ();

		if (a.m_floorInFocus.m_capturedAgent == null) {

			t.m_resultsText = "There are no Agents to interrogate";
			t.m_resultType = GameEvent.Henchmen_MissionDisrupted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
			return;
		}

		AgentWrapper agent = a.m_floorInFocus.m_capturedAgent;

		if (a.m_henchmenInFocus != null) {

			t.m_resultsText = a.m_henchmenInFocus.henchmenName.ToUpper () + " begins interrogating " + agent.m_agent.henchmenName.ToUpper();

		} else {

			t.m_resultsText = "The interrogation of " + agent.m_agent.henchmenName.ToUpper() + " begins";
		}

		if (a.m_success) {

			// chance to uncover a hidden agent, hidden intel, or hidden region token

			List<AgentWrapper> hiddenAgents = new List<AgentWrapper>();
			List<TokenSlot> hiddenIntel = new List<TokenSlot> ();
			List<TokenSlot> hiddenTokens = new List<TokenSlot> ();
			bool somethingRevealed = false;

			foreach (Region r in GameManager.instance.game.regions) {

				foreach (TokenSlot ts in r.assetTokens) {

					if (ts.m_type == TokenSlot.TokenType.Asset && ts.m_assetToken == GameManager.instance.m_intel && ts.m_state == TokenSlot.State.Hidden) {

						hiddenIntel.Add (ts);

					} else if (ts.m_state == TokenSlot.State.Hidden) {

						hiddenTokens.Add (ts);
					}
				}

				foreach (TokenSlot ts in r.controlTokens) {

					if (ts.m_state == TokenSlot.State.Hidden) {

						hiddenTokens.Add (ts);
					}
				}

				foreach (TokenSlot ts in r.policyTokens) {

					if (ts.m_state == TokenSlot.State.Hidden) {

						hiddenTokens.Add (ts);
					}
				}
			}

			foreach (AgentWrapper thisAgent in GameManager.instance.game.agentOrganization.currentAgents)
			{
				if (thisAgent.m_vizState == AgentWrapper.VisibilityState.Hidden) {

					hiddenAgents.Add (thisAgent);
				}
			}

			if (hiddenAgents.Count > 0 && Random.Range (0.0f, 1.0f) > 0.4f) {

				AgentWrapper aw = hiddenAgents[Random.Range(0, hiddenAgents.Count)];
				aw.ChangeVisibilityState (AgentWrapper.VisibilityState.Visible);
				somethingRevealed = true;
				t.m_resultsText += "\n" + aw.m_agent.henchmenName.ToUpper() + " is revealed in " + aw.m_agent.currentRegion.regionName.ToUpper() + "!";

			} else if (hiddenIntel.Count > 0 && Random.Range (0.0f, 1.0f) > 0.4f) {

				TokenSlot intel = hiddenIntel [Random.Range(0, hiddenIntel.Count)];
				intel.m_state = TokenSlot.State.Revealed;
				somethingRevealed = true;
				t.m_resultsText += "\nIntel is revealed in " + intel.m_region.regionName.ToUpper() + "!";

			} else if (hiddenTokens.Count > 0) {

				TokenSlot token = hiddenTokens [Random.Range(0, hiddenTokens.Count)];
				token.m_state = TokenSlot.State.Revealed;
				somethingRevealed = true;
				TokenBase tb = token.GetBaseToken ();
				if (tb != null) {
					t.m_resultsText += "\n" + tb.m_name.ToUpper () + " is revealed in " + token.m_region.regionName.ToUpper() + "!";
				} else {
					t.m_resultsText += "\nEmpty " + token.m_type.ToString().ToUpper () + " is revealed in " + token.m_region.regionName.ToUpper() + "!";
				}
			}

			if (!somethingRevealed) {
				t.m_resultsText += "\nNo information is gained";
			}

			// chance to injur agent

			if (Random.Range (0.0f, 1.0f) > 0.75f) {

				IncurInjury (agent.m_agent);
				t.m_resultsText += "\n" + agent.m_agent.henchmenName.ToUpper() + " is " + agent.m_agent.statusTrait.m_name + "!";
			}

			t.m_resultsText += "\n+" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

		} else {

			t.m_resultsText += "\nNo information is gained";

			// chance to injur agent

			if (Random.Range (0.0f, 1.0f) > 0.75f) {

				IncurInjury (agent.m_agent);
				t.m_resultsText += "\n" + agent.m_agent.henchmenName.ToUpper() + " is " + agent.m_agent.statusTrait.m_name + "!";
			}

			t.m_resultsText += "\n+" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override string GetNameText ()
	{
		string s = m_name + "\n";

		if (GameManager.instance.currentMissionWrapper.m_floorInFocus.m_capturedAgent != null) {
			
			AgentWrapper agent = GameManager.instance.currentMissionWrapper.m_floorInFocus.m_capturedAgent;
			s += "<size=18>" + agent.m_agent.henchmenName.ToUpper () + "</size>";

		}

		return s;
	}

	public override bool IsValid ()
	{
		MissionWrapper mw = GameManager.instance.currentMissionWrapper;

		if (mw.m_scope == TargetType.Floor && mw.m_floorInFocus.m_installedUpgrade.m_assetType == Asset.AssetType.LairUpgrade_Jail &&
			mw.m_floorInFocus.m_capturedAgent != null ) {

			AgentWrapper agent = mw.m_floorInFocus.m_capturedAgent;

			if (agent.m_agent.statusTrait.m_type != TraitData.TraitType.Incapacitated) {
				return true;
			}
		}
		return false;
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
