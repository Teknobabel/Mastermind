using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Agent Search Region")]
public class Agent_SearchRegion : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		a.m_agentInFocus.m_agentEvents.Add (AgentWrapper.AgentEvents.RegionSearched);

		if (a.m_success) {

			// add events for any relevant objects found in this region
			bool baseFound = false;
			bool henchmenFound = false;
			bool controlTokensFound = false;
			bool intelFound = false;

			// base

			if (a.m_region.id == GameManager.instance.game.player.homeRegion.id) {

				// check for cloak

				bool hasCloak = false;
				foreach (Asset asset in GameManager.instance.game.player.orgBase.m_currentAssets)
				{
					if (asset.m_assetType == Asset.AssetType.LairUpgrade_Cloak) {

						hasCloak = true;
						break;
					}
				}

				if (!hasCloak || (hasCloak && Random.Range(0.0f, 1.0f) > 0.4f))
				{
					a.m_agentInFocus.m_agentEvents.Add (AgentWrapper.AgentEvents.BaseFound);
					baseFound = true;
				}
			}

			// henchmen
			foreach (Region.HenchmenSlot hs in a.m_region.henchmenSlots) {

				if (hs.m_state == Region.HenchmenSlot.State.Occupied_Player) {

					a.m_agentInFocus.m_agentEvents.Add (AgentWrapper.AgentEvents.HenchmenFound);
					henchmenFound = true;
					break;
				}
			}

			// player owned control points
			foreach (TokenSlot ts in a.m_region.controlTokens) {

				if (ts.owner == Region.Owner.Player) {

					a.m_agentInFocus.m_agentEvents.Add (AgentWrapper.AgentEvents.PlayerControlTokenFound);
					controlTokensFound = true;
					break;
				}
			}

			// intel
			foreach (TokenSlot ts in a.m_region.assetTokens) {

				if (ts.m_assetToken != null && ts.m_assetToken == GameManager.instance.m_intel) {

					a.m_agentInFocus.m_agentEvents.Add (AgentWrapper.AgentEvents.IntelFound);
					intelFound = true;
					break;
				}
			}

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + ": " + a.m_region.regionName.ToUpper() + " mission complete!";
			if (baseFound) {
				t.m_resultsText += "\nBase found!";
			}
			if (henchmenFound) {
				t.m_resultsText += "\nHenchmen found!";
			}
			if (controlTokensFound) {
				t.m_resultsText += "\nPlayer owned Control Tokens found!";
			}
			if (intelFound) {
				t.m_resultsText += "\nIntel found!";
			}

			if (!baseFound && !henchmenFound && !controlTokensFound && !intelFound) {
				t.m_resultsText += "\nNothing found.";
			}

			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

		} else {

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + ": " + a.m_region.regionName.ToUpper() + " mission complete!";
			t.m_resultsText += "\n Nothing found.";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
		return false;
	}
}
