using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Agent Escape Jail")]
public class Agent_EscapeJail : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		Debug.Log ("Completing Mission: Escape Jail");
		base.CompleteMission (a);

		AgentWrapper aw = a.m_floorInFocus.m_capturedAgent;

		TurnResultsEntry t = new TurnResultsEntry ();
		t.m_henchmenIDs.Add (aw.m_agent.id);

		t.m_iconType = TurnResultsEntry.IconType.Agent;
		t.m_resultsText = aw.m_agent.henchmenName.ToUpper () + " attempts to escape!";

		// base escape chance

		float escapeChance = 0.25f;

		// apply modifiers based on traits

		if (aw.m_agent.HasTrait (TraitData.TraitType.Charismatic)) { escapeChance += 0.1f; }
		if (aw.m_agent.HasTrait (TraitData.TraitType.Spy)) { escapeChance += 0.075f; }
		if (aw.m_agent.HasTrait (TraitData.TraitType.MartialArtist)) { escapeChance += 0.075f; }
		if (aw.m_agent.HasTrait (TraitData.TraitType.Intimidating)) { escapeChance += 0.1f; }
		if (aw.m_agent.HasTrait (TraitData.TraitType.Violent)) { escapeChance += 0.1f; }

		if (aw.m_agent.HasTrait (TraitData.TraitType.Weak)) { escapeChance -= 0.075f; }
		if (aw.m_agent.HasTrait (TraitData.TraitType.Coward)) { escapeChance -= 0.075f; }

		if (aw.m_agent.statusTrait.m_type == TraitData.TraitType.Injured) { escapeChance -= 0.1f; }
		if (aw.m_agent.statusTrait.m_type == TraitData.TraitType.Critical) { escapeChance -= 0.2f; }

		// roll for escape

		bool didEscape = false;

		if (Random.Range (0.0f, 1.0f) <= escapeChance) {

			// agent escaped

			didEscape = true;
		}

		// chance for injury

		float injuryChace = 0.25f;

		if (aw.m_agent.HasTrait (TraitData.TraitType.Strong)) { injuryChace -= 0.1f; }
		if (aw.m_agent.HasTrait (TraitData.TraitType.Tough)) { injuryChace -= 0.1f; }

		if (aw.m_agent.HasTrait (TraitData.TraitType.Weak)) { injuryChace += 0.075f; }
		if (aw.m_agent.HasTrait (TraitData.TraitType.Coward)) { injuryChace += 0.075f; }

		if (Random.Range (0.0f, 1.0f) <= injuryChace) {

			// agent injured

			IncurInjury (aw.m_agent);

			t.m_resultsText += "\n" + aw.m_agent.henchmenName.ToUpper() + " becomes " + aw.m_agent.statusTrait.m_name + "!";
		}

		if (aw.m_agent.statusTrait.m_type == TraitData.TraitType.Incapacitated) {

			// remove from jail and send to limbo

			GameManager.instance.game.player.orgBase.RemoveCapturedAgent (aw);

			aw.m_agent.ChangeState (Henchmen.state.Idle);

		} else if (didEscape) {

			// if escaped and not incapacitated, turn invisible and go to random region

			GameManager.instance.game.player.orgBase.RemoveCapturedAgent (aw);

			aw.m_agent.ChangeState (Henchmen.state.Idle);
			aw.ChangeVisibilityState (AgentWrapper.VisibilityState.Hidden);

			Region destination = GameManager.instance.game.GetRandomRegion (true);

			destination.AddAgent (aw);

			t.m_resultsText += "\n" + aw.m_agent.henchmenName.ToUpper () + " escapes!";
		} else if (!didEscape) {

			t.m_resultsText += "\n" + aw.m_agent.henchmenName.ToUpper () + " fails to escape!";
		}

		t.m_resultType = GameEvent.Henchmen_MissionCompleted;
		GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

	}

	public override bool IsValid ()
	{
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
