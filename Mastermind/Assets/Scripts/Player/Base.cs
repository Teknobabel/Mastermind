using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base {

	public enum FloorState {
		None,
		Empty,
		Occupied,
		UpgradeInProgress,
	}

	public Floor m_lair;
	public List<Floor> m_floors = new List<Floor>();
	public List<Asset> m_currentAssets = new List<Asset> ();


	private int 
	m_maxFloors = 7;

	private float 
	m_chanceToNegateAmbushBonus = 0,
	m_chanceToRevealHiddenAgents = 0;

	public void Initialize (Director d, OrganizationBase org)
	{
		// add any starting base upgrades

		AddNewFloor (m_floors.Count + 1);

		if (d.m_startingBaseUpgrades.Length > 0) {

			for (int i = 0; i < d.m_startingBaseUpgrades.Length; i++) {

				BaseFloor a = d.m_startingBaseUpgrades [i];
				InstallAsset (a);

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_iconType = TurnResultsEntry.IconType.Organization;
				t.m_resultsText = org.orgName.ToUpper() + " gains Base Upgrade: " + a.m_name.ToUpper();
				//				t.m_resultType = GameEvent.Organization_OmegaPlanRevealed;

				org.AddTurnResults (0, t);
			}
		}

//		for (int i = 0; i < numFloors; i++) {

//			AddNewFloor (i + 1);
//		}

		// install layer at top of base

//		InstallAsset (numFloors, GameManager.instance.m_lairAsset);
		m_lair = new Floor();
		m_lair.m_floorState = FloorState.Occupied;
		m_lair.m_installedUpgrade = GameManager.instance.m_lairAsset;
		m_lair.m_floorNumber = m_floors.Count+1;
		m_lair.m_baseDefense = m_lair.m_installedUpgrade.m_defenseValue;
		m_lair.Initialize ();

	}

	public void AddNewFloor (int floorNum)
	{
		Floor f = new Floor ();
		f.m_floorState = FloorState.Empty;
		f.m_floorNumber = floorNum;
		f.Initialize ();
		m_floors.Add (f);

		if (m_lair != null) {
			m_lair.m_floorNumber = m_floors.Count+1;
		}
	}

	public void InstallAsset (BaseFloor asset)
	{
		foreach (Floor f in m_floors) {

			if (f.m_floorState == FloorState.UpgradeInProgress || f.m_floorState == FloorState.Empty) {

				f.m_floorState = FloorState.Occupied;
				f.m_installedUpgrade = asset;
				f.m_baseDefense = asset.m_defenseValue;

				m_currentAssets.Add (asset);

				if (GameManager.instance.game != null && GameManager.instance.game.player != null) {
					GameManager.instance.game.player.Notify (asset, GameEvent.Organization_AssetGained);
				}

				if (m_floors.Count < m_maxFloors) {
					AddNewFloor (m_floors.Count + 1);
				}

				return;
			}
		}
	}

	public void InstallAsset (int floorNumber, BaseFloor asset)
	{
		foreach (Floor f in m_floors) {

			if (f.m_floorNumber == floorNumber) {

				f.m_floorState = FloorState.Occupied;
				f.m_installedUpgrade = asset;
				f.m_baseDefense = asset.m_defenseValue;
				m_currentAssets.Add (asset);

				if (GameManager.instance.game != null) {
					GameManager.instance.game.player.Notify (asset, GameEvent.Organization_AssetGained);
				}
				return;
			}
		}
	}

	public void RemoveAsset (int floorNumber)
	{
		foreach (Floor f in m_floors) {

			if (f.m_floorNumber == floorNumber) {

				f.m_floorState = FloorState.Empty;
				f.m_baseDefense = 0;
				m_currentAssets.Remove (f.m_installedUpgrade);
				f.m_installedUpgrade = null;

				return;
			}
		}
	}

	public bool CanCaptureAgent ()
	{
		foreach (Floor f in m_floors) {

			if (f.m_installedUpgrade != null && f.m_installedUpgrade.m_assetType == Asset.AssetType.LairUpgrade_Jail && f.m_capturedAgent == null) {

				return true;
			}
		}

		return false;
	}

	public void RemoveCapturedAgent (AgentWrapper aw)
	{
		foreach (Floor f in m_floors) {

			if (f.m_installedUpgrade != null && f.m_installedUpgrade.m_assetType == Asset.AssetType.LairUpgrade_Jail && f.m_capturedAgent != null
				&& f.m_capturedAgent == aw) {

				GameManager.instance.game.limbo.RemoveAgent (aw.m_agent);
				f.m_capturedAgent = null;
				break;
			}
		}
	}

	public void AgentCaptured (AgentWrapper aw)
	{
		GameManager.instance.game.agentOrganization.RemoveAgentFromMissions (aw);
		aw.m_agentEvents.Clear ();

		aw.m_agent.currentRegion.RemoveAgent (aw.m_agent);
		GameManager.instance.game.limbo.AddAgent (aw);
		aw.m_agent.ChangeState (Henchmen.state.Captured);

		foreach (Floor f in m_floors) {

			if (f.m_installedUpgrade != null && f.m_installedUpgrade.m_assetType == Asset.AssetType.LairUpgrade_Jail && f.m_capturedAgent == null) {

				f.m_capturedAgent = aw;
				break;
			}
		}
	}

	public float chanceToNegateAmbushBonus {get { return m_chanceToNegateAmbushBonus; }set{ m_chanceToNegateAmbushBonus = value; }}
	public float chanceToRevealHiddenAgents {get{ return m_chanceToRevealHiddenAgents; }set{ m_chanceToRevealHiddenAgents = value; }}
	public int maxFloors {get{ return m_maxFloors; }}
}
