﻿using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Build Asset: Warehouse")]
public class BuildAsset_Warehouse : MissionBase {

	public Asset m_asset;
	public Asset m_requiredResearch;

	public int m_numAssetSlots = 6;

	public override void InitializeMission (MissionWrapper a)
	{
		if (a.m_floorInFocus != null) {
			a.m_floorInFocus.m_floorState = Base.FloorState.UpgradeInProgress;
		}
	}

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);
				a.m_success = true;

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}

		if (a.m_success) {

			// add asset to player's inventory

			GameManager.instance.game.player.AddAsset (m_asset);
			GameManager.instance.game.player.maxAssets += m_numAssetSlots;
			GameManager.instance.game.player.orgBase.InstallAsset (a.m_floorInFocus.m_floorNumber, m_asset);

			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
			t.m_resultsText += "\n" + GameManager.instance.game.player.orgName.ToUpper() + " GAINS A " + m_asset.m_name.ToUpper() + ".";
			t.m_resultsText += "\n" + GameManager.instance.game.player.orgName.ToUpper() + " can hold " + m_numAssetSlots.ToString() + " more Assets.";
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

		} else {

			a.m_floorInFocus.m_floorState = Base.FloorState.Empty;

			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null &&
			GameManager.instance.currentMissionWrapper.m_scope == m_targetType && (m_requiredResearch == null || GameManager.instance.game.player.currentResearch.Contains(m_requiredResearch))) {

			// player can have up to 2 warehouses built

			int numWarehouses = 0;

			foreach (Asset a in GameManager.instance.game.player.orgBase.m_currentAssets) {

				if (a.m_assetType == Asset.AssetType.LairUpgrade_Warehouse) {

					numWarehouses++;

					if (numWarehouses > 1) {

						return false;
					}
				}
			}



			Region r = GameManager.instance.currentMissionWrapper.m_region;
			if (r == GameManager.instance.game.player.homeRegion && !GameManager.instance.game.player.currentAssets.Contains(m_asset)) {
				return true;
			}
		}
		return false;
	}
}
