using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Travel To Region")]
public class TravelToRegion : MissionBase {

	public override void InitializeMission (MissionWrapper a)
	{
		Debug.Log ("Initializing Mission: " + m_name);

		if (a.m_agentInFocus != null) {

			if (a.m_henchmenSlotInFocus != null) {

				a.m_region.ReserveSlot (a.m_agentInFocus.m_agent, a.m_henchmenSlotInFocus);

			} else {

				a.m_region.ReserveSlot (a.m_agentInFocus.m_agent);
			}

		} else if (a.m_henchmenInFocus != null) {

			if (a.m_henchmenSlotInFocus != null) {

				a.m_region.ReserveSlot (a.m_henchmenInFocus, a.m_henchmenSlotInFocus);

			} else {

				a.m_region.ReserveSlot (a.m_henchmenInFocus);
			}
		}
		else if (a.m_henchmen.Count > 0) {
			
			foreach (Henchmen h in a.m_henchmen) {

				a.m_region.ReserveSlot (h, a.m_henchmenSlotInFocus);
			}
		}

	}

	public override void CompleteMission (MissionWrapper a)
	{
		Debug.Log ("Completing Mission: " + m_name);

		bool ambush = false;
		TurnResultsEntry t = new TurnResultsEntry ();

		if (a.m_agentInFocus != null) {

			if (a.m_henchmenSlotInFocus.m_state == Region.HenchmenSlot.State.Occupied_Player) {

				ambush = true;
				a.m_henchmenInFocus = a.m_henchmenSlotInFocus.m_henchmen;

			} else if (a.m_henchmenSlotInFocus.m_state == Region.HenchmenSlot.State.Empty) {
				
				a.m_region.AddAgent (a.m_agentInFocus);
				t.m_resultsText = a.m_agentInFocus.m_agent.henchmenName.ToUpper () + " arrives in " + a.m_region.regionName.ToUpper();
			}

			// chance to reveal hidden agent entering region w base

			if (a.m_region.id == GameManager.instance.game.player.homeRegion.id && GameManager.instance.game.player.orgBase.chanceToRevealHiddenAgents > 0.0f) {

				if (Random.Range (0.0f, 1.0f) > GameManager.instance.game.player.orgBase.chanceToRevealHiddenAgents) {

					a.m_agentInFocus.ChangeVisibilityState (AgentWrapper.VisibilityState.Visible);

					TurnResultsEntry t2 = new TurnResultsEntry ();
					t.m_resultsText = a.m_agentInFocus.m_agent.henchmenName.ToUpper () + " has been discovered sneaking around your Lair!";
					t2.m_iconType = TurnResultsEntry.IconType.Agent;
					t2.m_resultType = GameEvent.Henchmen_ArriveInRegion;
					GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t2);
				}
			}

		} else if (a.m_henchmenInFocus != null || a.m_henchmen.Count > 0) {

			if (a.m_henchmen.Count > 0) {
				a.m_henchmenInFocus = a.m_henchmen [0];
			}

			t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);

			if (a.m_henchmenSlotInFocus.m_state == Region.HenchmenSlot.State.Occupied_Agent) {
				
				a.m_agentInFocus = a.m_henchmenSlotInFocus.m_agent;
				ambush = true;
			} else if (a.m_henchmenSlotInFocus.m_state == Region.HenchmenSlot.State.Empty) {

				a.m_region.AddHenchmen (a.m_henchmenInFocus);
				t.m_resultsText = a.m_henchmenInFocus.henchmenName.ToUpper () + " arrives in " + a.m_region.regionName.ToUpper();
			}

		}


		t.m_iconType = TurnResultsEntry.IconType.Travel;
		t.m_resultType = GameEvent.Henchmen_ArriveInRegion;
		a.m_organization.AddTurnResults (GameManager.instance.game.turnNumber, t);

		if (ambush) {

			DoAmbush (a.m_agentInFocus, a.m_henchmenInFocus, a.m_henchmenSlotInFocus);
		}
	}

	public override bool IsValid ()
	{
		if (!base.IsValid ()) { return false;}		return true;
	}

	public override void CancelMission (MissionWrapper a)
	{
		base.CancelMission (a);

		// clear any reserved slot

		if (a.m_henchmenSlotInFocus != null && ( a.m_henchmenInFocus != null || a.m_henchmen.Count > 0)) {

			if (a.m_henchmen.Count > 0 && a.m_henchmenInFocus == null) {
				a.m_henchmenInFocus = a.m_henchmen [0];
			}

			if (a.m_henchmenSlotInFocus.m_enRoute.Contains (a.m_henchmenInFocus)) {
				a.m_henchmenSlotInFocus.m_enRoute.Remove (a.m_henchmenInFocus);
			}
		}

		if (a.m_agentInFocus != null && a.m_henchmenSlotInFocus != null) {

			if (a.m_henchmenSlotInFocus.m_enRoute.Contains (a.m_agentInFocus.m_agent)) {
				a.m_henchmenSlotInFocus.m_enRoute.Remove (a.m_agentInFocus.m_agent);
			}

		}

	}

	private void DoAmbush (AgentWrapper agent, Henchmen henchmen, Region.HenchmenSlot slot)
	{
		Debug.Log ("Commencing Ambush");

		// calculate score for each, based on presence of specific traits + some randomness

		TurnResultsEntry t = new TurnResultsEntry ();
		Dictionary<Henchmen, int> ambushDict = new Dictionary<Henchmen, int> ();
		List<Henchmen> l = new List<Henchmen> ();
		Henchmen winner = null;
		Henchmen loser = null;
		int winningScore = -1;

		t.m_henchmenIDs.Add (henchmen.id);

		l.Add (henchmen);
		l.Add (agent.m_agent);

		foreach (Henchmen h in l) {

			int score = 0;

			if (h.id == agent.m_agent.id && agent.m_vizState == AgentWrapper.VisibilityState.Hidden) {

				score += 30;
				agent.ChangeVisibilityState (AgentWrapper.VisibilityState.Visible);
			}

			if (h.HasTrait (TraitData.TraitType.Agent)) { score += 30; }
			if (h.HasTrait (TraitData.TraitType.MartialArtist)) { score += 15; }
			if (h.HasTrait (TraitData.TraitType.SharpShooter)) { score += 25; }
			if (h.HasTrait (TraitData.TraitType.Assassin)) { score += 30; }
			if (h.HasTrait (TraitData.TraitType.Mercenary)) { score += 20; }
			if (h.HasTrait (TraitData.TraitType.Spy)) { score += 10; }
			if (h.HasTrait (TraitData.TraitType.Bodyguard)) { score += 15; }
			if (h.HasTrait (TraitData.TraitType.Strong)) { score += 10; }
			if (h.HasTrait (TraitData.TraitType.Tough)) { score += 10; }
			if (h.HasTrait (TraitData.TraitType.Intimidating)) { score += 15; }

			if (h.HasTrait (TraitData.TraitType.Weak)) { score -= 15; }
			if (h.HasTrait (TraitData.TraitType.Coward)) { score -= 15; }
			if (h.HasTrait (TraitData.TraitType.Injured)) { score -= 20; }
			if (h.HasTrait (TraitData.TraitType.Critical)) { score -= 35; }

			score += Random.Range (-15, 15);

			if (score > winningScore) {

				winner = h;
				winningScore = score;
			}

			ambushDict.Add (h, score);

		}

		// the winner gets the henchmen slot in the region

		if (winner == henchmen) {

			loser = agent.m_agent;

			if (slot.m_enRoute.Contains (loser)) {
				slot.m_enRoute.Remove (loser);
			}

			if (slot.m_state == Region.HenchmenSlot.State.Occupied_Agent) {
				
//				AgentWrapper a = slot.m_agent;
				Region r = slot.m_agent.m_agent.currentRegion;

				r.RemoveAgent(slot.m_agent.m_agent);
				r.AddHenchmen (henchmen);
//				slot.RemoveAgent ();
//				slot.AddHenchmen (henchmen);
			}

		} else if (winner == agent.m_agent) {

			loser = henchmen;

			if (slot.m_enRoute.Contains (loser)) {
				slot.m_enRoute.Remove (loser);
			}

			if (slot.m_state == Region.HenchmenSlot.State.Occupied_Player) {
				
//				Henchmen h = slot.m_henchmen;
//				slot.RemoveHenchmen ();
//				slot.AddAgent (agent);

				Region r = slot.m_henchmen.currentRegion;
				r.RemoveHenchmen (henchmen);
				r.AddAgent (agent);
			}
		}
			
		t.m_resultsText = "An ambush occurs between " + henchmen.henchmenName.ToUpper() + " and " + agent.m_agent.henchmenName.ToUpper() + "!";
		t.m_resultsText += "\n " + agent.m_agent.henchmenName.ToUpper () + " Score: " + ambushDict [agent.m_agent].ToString ();
		t.m_resultsText += "\n " + henchmen.henchmenName.ToUpper () + " Score: " + ambushDict [henchmen].ToString ();
		t.m_resultsText += "\n " + winner.henchmenName.ToUpper () + " wins!";
		t.m_resultType = GameEvent.Henchmen_ArriveInRegion;

		// loser has a chance to get injured (difference in scores?);

		float injuredChance = 0.4f;

		if (loser.HasTrait (TraitData.TraitType.Weak)) { injuredChance -= 0.2f; }
		if (loser.HasTrait (TraitData.TraitType.Strong)) { injuredChance += 0.2f; }

		if (Random.Range (0.0f, 1.0f) > injuredChance) {

			// loser is injured

			switch (loser.statusTrait.m_type) {
			case TraitData.TraitType.Healthy:

				loser.UpdateStatusTrait (GameManager.instance.m_statusTraits [1]);
				t.m_resultsText += "\n " + loser.henchmenName.ToUpper () + " is Injured!";

				break;
			case TraitData.TraitType.Injured:

				loser.UpdateStatusTrait (GameManager.instance.m_statusTraits [2]);
				t.m_resultsText += "\n " + loser.henchmenName.ToUpper () + " is Critically Injured!";

				break;
			case TraitData.TraitType.Critical:

				loser.UpdateStatusTrait (GameManager.instance.m_statusTraits [3]);
				t.m_resultsText += "\n " + loser.henchmenName.ToUpper () + " is Incapacitated!";

				break;
			}
		}

		// the loser gets removed from the game for a few turns

		if (loser == henchmen) {
			loser.currentRegion.RemoveHenchmen (loser);
			GameManager.instance.game.limbo.AddHenchmen (loser);
		} else if (loser == agent.m_agent) {
			loser.currentRegion.RemoveAgent (loser);
			GameManager.instance.game.limbo.AddAgent (agent);
		}

		t.m_resultsText += "\n " + loser.henchmenName.ToUpper () + "'s location is unknown!";
	
		GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
	}

}