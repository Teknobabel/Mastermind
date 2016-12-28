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
	public class Floor 
	{
		public int m_floorNumber = -1;
		public Asset m_installedUpgrade = null;
		public FloorState m_floorState = FloorState.None;
		public List<Henchmen> m_guards = new List<Henchmen> ();
		public int m_baseDefense = 0;

		// dependant on type of installed upgrade

		public AgentWrapper m_capturedAgent = null;
	}

	public List<Floor> m_floors = new List<Floor>();
	public List<Asset> m_currentAssets = new List<Asset> ();

	private int 
	m_maxFloors = 7;

	private float 
	m_chanceToNegateAmbushBonus = 0,
	m_chanceToRevealHiddenAgents = 0;

	public void Initialize (int numFloors)
	{
		for (int i = 0; i < numFloors; i++) {

			AddNewFloor (i + 1);
		}

		// install layer at top of base

		InstallAsset (numFloors, GameManager.instance.m_lairAsset);
	}

	public void AddNewFloor (int floorNum)
	{
		Floor f = new Floor ();
		f.m_floorState = FloorState.Empty;
		f.m_floorNumber = floorNum;
		m_floors.Add (f);
	}

	public void InstallAsset (Asset asset)
	{
		foreach (Floor f in m_floors) {

			if (f.m_floorState == FloorState.Empty) {

				f.m_floorState = FloorState.Occupied;
				f.m_installedUpgrade = asset;
				f.m_baseDefense = asset.m_defenseValue;
				m_currentAssets.Add (asset);

				return;
			}
		}
	}

	public void InstallAsset (int floorNumber, Asset asset)
	{
		foreach (Floor f in m_floors) {

			if (f.m_floorNumber == floorNumber) {

				f.m_floorState = FloorState.Occupied;
				f.m_installedUpgrade = asset;
				f.m_baseDefense = asset.m_defenseValue;
				m_currentAssets.Add (asset);
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
