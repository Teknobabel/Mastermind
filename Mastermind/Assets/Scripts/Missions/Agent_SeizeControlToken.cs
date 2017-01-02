using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Agent Seize Control Token")]
public class Agent_SeizeControlToken : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_agentInFocus != null) {t.m_henchmenIDs.Add (a.m_agentInFocus.m_agent.id);}

		if (a.m_success) {

			List<TokenSlot> validTokens = new List<TokenSlot> ();

			// gather a list of all player owned control tokens

			foreach (TokenSlot ts in a.m_region.controlTokens) {

				if (ts.owner == Region.Owner.Player) {

					validTokens.Add (ts);
				}
			}

			if (validTokens.Count > 0) {

				TokenSlot ts = validTokens[Random.Range(0, validTokens.Count)];
				validTokens.Remove (ts);

				ts.ChangeOwner (Region.Owner.AI);

				t.m_iconType = TurnResultsEntry.IconType.Agent;
				t.m_resultsText = a.m_agentInFocus.m_agent.henchmenName.ToUpper () + " has liberated a " + ts.m_controlToken.m_controlType.ToString().ToUpper() + " Control Point in " + a.m_region.regionName.ToUpper() + "!";
				t.m_resultsText += "\n" + ts.m_controlToken.m_controlType.ToString().ToUpper() + " is no longer under your control!";
				t.m_resultType = GameEvent.Region_ControlTokenOwnerChanged;
				GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

				// update agent state if all player owned control tokens are lost

				if (validTokens.Count == 0 && a.m_agentInFocus.m_agentEvents.Contains (AgentWrapper.AgentEvents.PlayerControlTokenFound)) {

					a.m_agentInFocus.m_agentEvents.Remove (AgentWrapper.AgentEvents.PlayerControlTokenFound);
				}
			}

		} else {

		}
	}

	public override bool IsValid ()
	{
		if (!base.IsValid ()) { return false;}		return false;
	}
}
