using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Organization : OrganizationBase {

	private int m_currentInfamy = 0;
	private int m_maxInfamy = 0;
	private int m_currentWantedLevel = 0;
	private int m_maxWantedLevel = 1;
	private int m_commandPool = 0;
	private int m_currentCommandPool = 0;
	private int m_maxAvailableHenchmen = 1;
	private int m_currentIntel = 0;
	private int m_maxIntel = 1;
	private int m_maxAssets = 99;

	private List<Henchmen> m_currentHenchmen;
	private List<Henchmen> m_availableHenchmen;
	private List<OmegaPlan> m_omegaPlans;
	private List<Asset> m_currentAssets;
	private List<Asset> m_currentResearch;
	private List<Asset> m_assetsInOrbit;

	private Dictionary<int, OmegaPlan> m_omegaPlansByID = new Dictionary<int, OmegaPlan> ();
//	private Dictionary<int, MenuTab> m_menuTabs;

	private Region m_homeRegion = null;

	private Base m_base;

	public void RefillCommandPool ()
	{
		m_currentCommandPool = GetCommandPool();
		Notify (this, GameEvent.Organization_CommandPoolChanged);
	}

	public void HireHenchmen (int henchmenID)
	{
		for (int i=0; i < m_availableHenchmen.Count; i++)
		{
			Henchmen h = m_availableHenchmen [i];
			if (h.id == henchmenID) {
				
				UseCommandPoints (h.hireCost);
				h.SetOwner (Region.Owner.Player);
				m_currentHenchmen.Add (h);
				m_availableHenchmen.RemoveAt (i);
//				h.SetRegion (m_homeRegion);
				m_homeRegion.AddHenchmen(h);

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_iconType = TurnResultsEntry.IconType.Henchmen;
				t.m_resultsText = h.henchmenName + " joins " + m_name;
				t.m_resultType = GameEvent.Organization_HenchmenHired;
				t.m_henchmenIDs.Add (h.id);
				AddTurnResults (GameManager.instance.game.turnNumber, t);

				Notify (this, GameEvent.Organization_HenchmenHired);
				break;
			}
		}
	}

	public void AddHenchmen (Henchmen h)
	{
		h.SetOwner (Region.Owner.Player);
		m_currentHenchmen.Add (h);
		m_homeRegion.AddHenchmen(h);
	}

	public void DismissHenchmen (int henchmenID)
	{
		for (int i=0; i < m_availableHenchmen.Count; i++)
		{
			Henchmen h = m_availableHenchmen [i];

			if (h.id == henchmenID) {
				
				m_availableHenchmen.RemoveAt (i);

				Notify (this, GameEvent.Organization_HenchmenDismissed);
				break;
			}
		}
	}

	public int GetNumOwnedRegions ()
	{
		int owned = 0;

		foreach (Region r in GameManager.instance.game.regions) {

			bool playerOwned = true;

			foreach (TokenSlot ts in r.controlTokens) {

				if (ts.owner == Region.Owner.AI) {

					playerOwned = false;
					break;
				}
			}

			if (playerOwned) {

				owned++;
			}
		}

		return owned;
	}

	public void FireHenchmen (int henchmenID)
	{
		for (int i=0; i < m_currentHenchmen.Count; i++)
		{
			Henchmen h = m_currentHenchmen [i];

			if (h.id == henchmenID) {

				m_currentHenchmen.RemoveAt (i);

				// remove from current region
				h.SetRegion(null);

				// remove from current missions

				RemoveHenchmenFromMissions (h);

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_iconType = TurnResultsEntry.IconType.Henchmen;
				t.m_resultsText = h.henchmenName + " is dismissed from " + m_name;
				t.m_resultType = GameEvent.Organization_HenchmenFired;
				t.m_henchmenIDs.Add (h.id);
				AddTurnResults (GameManager.instance.game.turnNumber, t);


				Notify (this, GameEvent.Organization_HenchmenFired);
				break;
			}
		}
	}

	public override void AddMission (MissionWrapper mw)
	{
		base.AddMission (mw);

		if (mw.m_mission.m_cost > 0) {
			UseCommandPoints (mw.m_mission.m_cost);
		}

	}

	public override void MissionCompleted (MissionWrapper a)
	{
		base.MissionCompleted (a);

		if (a.m_success && a.m_mission.m_infamyGain > 0) {

			GainInfamy (a.m_mission.m_infamyGain);

		} else if (!a.m_success && a.m_mission.m_missionFailInfamyGain > 0) {
			
			GainInfamy (a.m_mission.m_missionFailInfamyGain);
		}
	}

	public void AddHenchmenToAvailablePool (Henchmen h)
	{
		m_availableHenchmen.Add (h);
	}

	public void GainCommandPoints (int points)
	{
		m_currentCommandPool = Mathf.Clamp (m_currentCommandPool + points, 0, 99);
		Notify (this, GameEvent.Organization_CommandPoolChanged);
	}

	public void UseCommandPoints (int points)
	{
		m_currentCommandPool = Mathf.Clamp (m_currentCommandPool - points, 0, 99);
		Notify (this, GameEvent.Organization_CommandPoolChanged);
	}

	private void AddOmegaPlan (OmegaPlan op)
	{
		m_omegaPlans.Add (op);
		m_omegaPlansByID.Add (op.id, op);
	}

	public void GainWantedLevel (int amount)
	{
		Debug.Log ("Wanted Level Increased");

		if (m_currentWantedLevel < m_maxWantedLevel) {
			m_currentWantedLevel++;

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_iconType = TurnResultsEntry.IconType.WantedLevel;
			t.m_resultsText = "Wanted Level has increased to " + m_currentWantedLevel.ToString() + "!";
			t.m_resultType = GameEvent.Organization_WantedLevelIncreased;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

			GameManager.instance.game.agentOrganization.PlayerWantedLevelIncreased ();

			Notify (this, GameEvent.Organization_WantedLevelIncreased);
		}
	}

	public void GainInfamy (int amount)
	{
		Debug.Log ("Gaining Infamy: " + amount.ToString ());

		if (m_currentWantedLevel < m_maxWantedLevel)
		{
			m_currentInfamy += amount;

			if (m_currentInfamy >= m_maxInfamy) {
				m_currentInfamy -= m_maxInfamy;
				GainWantedLevel (1);
			}
		}
		Notify (this, GameEvent.Organization_InfamyChanged);

	}

	public void AddResearch (Asset r)
	{
		m_currentResearch.Add (r);
	}

	public void AddAsset (Asset a)
	{
		m_currentAssets.Add (a);

		Notify (a, GameEvent.Organization_AssetGained);
	}

	public void LaunchAssetIntoOrbit (Asset a)
	{
		m_assetsInOrbit.Add (a);

		Notify (a, GameEvent.Mission_AssetLaunched);
	}

	public void RemoveAsset (Asset a)
	{
		if (m_currentAssets.Contains (a)) {
			m_currentAssets.Remove (a);

			Notify (a, GameEvent.Organization_AssetRemoved);
		}
	}
		
	public override void CancelMission (MissionWrapper mw)
	{
		base.CancelMission (mw);

		foreach (Henchmen h in mw.m_henchmen) {

			if (h.currentState == Henchmen.state.OnMission) {

				h.ChangeState (Henchmen.state.Idle);
			}
		}

		if (mw.m_henchmenInFocus != null && mw.m_henchmenInFocus.currentState == Henchmen.state.OnMission) {

			mw.m_henchmenInFocus.ChangeState (Henchmen.state.Idle);
		}
	}

	public void RemoveHenchmenFromMissions (Henchmen h)
	{
		h.ChangeState (Henchmen.state.Idle);

		List<MissionWrapper> cancelledMissions = new List<MissionWrapper> ();

		foreach (MissionWrapper mw in activeMissions) {

			if (mw != GameManager.instance.currentlyExecutingMission) {
				
				if (mw.m_henchmenInFocus != null && mw.m_henchmenInFocus.id == h.id) {

					mw.m_henchmenInFocus = null;
				}

				if (mw.m_henchmen.Contains (h)) {

					mw.m_henchmen.Remove (h);
				}

				if (mw.m_henchmenInFocus == null && mw.m_henchmen.Count == 0) {

					cancelledMissions.Add (mw);
				}
			}
		}

		foreach (MissionWrapper mw in cancelledMissions) {

			if (activeMissions.Contains (mw)) {

				mw.m_mission.CancelMission (mw);
				activeMissions.Remove (mw);

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_resultsText = mw.m_mission.m_name.ToUpper() + " Mission is cancelled!";
				t.m_resultType = GameEvent.Henchmen_MissionDisrupted;
				GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
			}
		}
	}

	public override void Initialize (string orgName)
	{
		Game g = GameManager.instance.game;
		Director d = g.director;

		m_name = orgName;
		m_currentWantedLevel = d.m_startingWantedLevel;
		m_maxWantedLevel = d.m_maxWantedLevel;
		m_maxInfamy = d.m_maxInfamy;
		m_commandPool = d.m_startingCommandPool;
		m_maxAvailableHenchmen = d.m_startingHenchmen;
		m_currentIntel = d.m_startingIntel;
		m_maxIntel = d.m_maxIntel;
		m_maxAssets = 6;

		TurnResultsEntry t2 = new TurnResultsEntry ();
		t2.m_iconType = TurnResultsEntry.IconType.Organization;
		t2.m_resultsText = m_name.ToUpper() + " is formed!";
		t2.m_resultType = GameEvent.Organization_OmegaPlanRevealed;
		AddTurnResults (0, t2);

		// add any starting assets

		m_currentAssets = new List<Asset> ();
		m_currentResearch = new List<Asset> ();
		m_assetsInOrbit = new List<Asset> ();

		foreach (Asset a in d.m_startingAssets) {
			
			AddAsset (a);

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_iconType = TurnResultsEntry.IconType.Organization;
			t.m_resultsText = m_name.ToUpper() + " gains Asset: " + a.m_name.ToUpper();
//			t.m_resultType = GameEvent.Organization_OmegaPlanRevealed;
			AddTurnResults (0, t);
		}

		// initialize base

		m_base = new Base ();
		m_base.Initialize (d, this);


		// add any starting research

		if (d.m_startingResearch.Length > 0) {

			for (int i = 0; i < d.m_startingResearch.Length; i++) {

				Asset a = d.m_startingResearch [i];
				m_currentResearch.Add (a);

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_iconType = TurnResultsEntry.IconType.Organization;
				t.m_resultsText = m_name.ToUpper() + " gains Research: " + a.m_name.ToUpper();
//				t.m_resultType = GameEvent.Organization_OmegaPlanRevealed;
				AddTurnResults (0, t);
			}
		}

		// select Omega Plans

		List<OmegaPlanData> ops = new List<OmegaPlanData> ();
		m_omegaPlans = new List<OmegaPlan> ();
		List<AssetToken> mandatoryAssets = new List<AssetToken> ();

		foreach (OmegaPlanData o in d.m_omegaPlanBank) {

			bool alreadyPresent = false;
			foreach (OmegaPlanData startingO in d.m_startingOmegaPlanData)
			{
				if (startingO == o) {
					alreadyPresent = true;
				}
			}

			if (!alreadyPresent) {
				ops.Add (o);
			}
		}

		int revealed = 0;
		for (int i = 0; i < d.m_startingOmegaPlans; i++) {

			// pull first from starting omega plans as per Director

			if (i < d.m_startingOmegaPlanData.Length) {
				OmegaPlanData newOPData = d.m_startingOmegaPlanData [i];

				OmegaPlan newOP = OmegaPlan.CreateInstance<OmegaPlan> ();

				if (revealed < d.m_startingRevealedOmegaPlans) {
					
					newOP.Initialize (newOPData, OmegaPlan.State.Revealed, this);
					revealed++;

					TurnResultsEntry t = new TurnResultsEntry ();
					t.m_iconType = TurnResultsEntry.IconType.OmegaPlan;
					t.m_resultsText = "OMEGA PLAN: " + newOP.opName.ToUpper() + " is now unlocked!";
					t.m_resultType = GameEvent.Organization_OmegaPlanRevealed;
					AddTurnResults (0, t);

				} else {
					newOP.Initialize (newOPData, OmegaPlan.State.Hidden, this);
				}

				mandatoryAssets.AddRange (newOP.mandatoryAssets);

				AddOmegaPlan (newOP);

			} else if (ops.Count > 0) {
				int rand = Random.Range (0, ops.Count);
				OmegaPlanData newOPData = ops [rand];
				ops.RemoveAt (rand);

				OmegaPlan newOP = OmegaPlan.CreateInstance<OmegaPlan> ();

				if (revealed < d.m_startingRevealedOmegaPlans) {
					newOP.Initialize (newOPData, OmegaPlan.State.Revealed, this);
					revealed++;

					TurnResultsEntry t = new TurnResultsEntry ();
					t.m_iconType = TurnResultsEntry.IconType.OmegaPlan;
					t.m_resultsText = "OMEGA PLAN: " + newOP.opName.ToUpper() + " is now unlocked!";
					t.m_resultType = GameEvent.Organization_OmegaPlanRevealed;
					AddTurnResults (0, t);

				} else {
					newOP.Initialize (newOPData, OmegaPlan.State.Hidden, this);
				}

				mandatoryAssets.AddRange (newOP.mandatoryAssets);

				AddOmegaPlan (newOP);
			}
		}

		// disperse any mandatory assets needed to complete OP's

		List<Region> allRegions = g.GetAllRegions();

		while (mandatoryAssets.Count > 0) {
			
			AssetToken thisAsset = mandatoryAssets [0];
			mandatoryAssets.RemoveAt (0);

			// get list of regions with less than max # assets

			List<Region> validRegions = new List<Region>();

			foreach (Region r in allRegions) {

				if (r.assetTokens.Count < d.maxTokenSlotsInRegion) {
					validRegions.Add (r);
				}
			}

			// pull a random region

			if (validRegions.Count > 0) {

				int rand = Random.Range (0, validRegions.Count);

				Region thisRegion = validRegions[rand];

				// add Asset

				thisRegion.AddAssetToken (thisAsset);
			}
		}

		// initialize home region (Lair)

		Region newRegion = Region.CreateInstance<Region> ();
		newRegion.Initialize (GameManager.instance.m_lairRegion);
		m_homeRegion = newRegion;
		g.AddRegionToGame (newRegion);

		// select starting Henchmen

		m_currentHenchmen = new List<Henchmen> ();
		m_availableHenchmen = new List<Henchmen> ();

		// get any starting henchmen from the director

		for (int i=0; i < d.m_startingHenchmenData.Length; i++)
		{
			Henchmen h = g.GetHenchmen (d.m_startingHenchmenData [i]);

			if (h != null) {
				
				AddHenchmen (h);

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_iconType = TurnResultsEntry.IconType.Henchmen;
				t.m_resultsText = h.henchmenName + " joins " + m_name;
				t.m_resultType = GameEvent.Organization_HenchmenHired;
				t.m_henchmenIDs.Add (h.id);
				AddTurnResults (GameManager.instance.game.turnNumber, t);
			}
		}

		List<Henchmen> bank = new List<Henchmen> ();

		switch (d.m_maxStartingHenchmenLevel) {
		case 1:
			bank.AddRange (g.GetHenchmenByRank(1));
			break;
		case 2:
			bank.AddRange (g.GetHenchmenByRank(1));
			bank.AddRange (g.GetHenchmenByRank(2));
			break;
		case 3:
			bank.AddRange (g.GetHenchmenByRank(1));
			bank.AddRange (g.GetHenchmenByRank(2));
			bank.AddRange (g.GetHenchmenByRank(3));
			break;
		}

		foreach (Henchmen h in m_currentHenchmen) {
			if (bank.Contains (h)) {
				bank.Remove (h);
			}
		}

		for (int i = 0; i < d.m_startingHenchmen; i++) {
			if (bank.Count > 0) {
				
				int rand = Random.Range (0, bank.Count);
				Henchmen newH = bank [rand];
				m_availableHenchmen.Add (newH);
				bank.RemoveAt (rand);

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_iconType = TurnResultsEntry.IconType.Henchmen;
				t.m_resultsText = newH.henchmenName + " is available for hire";
				t.m_resultType = GameEvent.Organization_HenchmenHired;
				AddTurnResults (GameManager.instance.game.turnNumber, t);
			}
		}

		AddObserver (TabMenu.instance);

		Notify (this, GameEvent.Organization_Initialized);
	}

	private int GetCostPerTurn ()
	{
		int cost = 0;

		// each employed henchmen has a cost per turn variable

		foreach (Henchmen h in m_currentHenchmen) {
			cost += h.costPerTurn;
		}

		return cost;
	}
		
	private int GetCommandPool ()
	{
		int totalCP = m_commandPool;

		// check for any Command Centers installed in the player's base

		foreach (Asset a in m_currentAssets) {

			if (a.m_assetType == Asset.AssetType.CommandCenter) {
				totalCP += 2;
			}
		}

		// check for regions controlled by the player

		foreach (Region r in GameManager.instance.game.regions) {

			if (r.owner == Region.Owner.Player) {

				totalCP += r.rank;
			}
		}

		return totalCP;
	}
		
	public List<OmegaPlan> omegaPlans {get{return m_omegaPlans; }}
	public Dictionary<int, OmegaPlan> omegaPlansByID {get{return m_omegaPlansByID; }}
	public List<Henchmen> availableHenchmen {get{return m_availableHenchmen; }}
	public List<Henchmen> currentHenchmen {get{return m_currentHenchmen; }}
	public int currentCommandPool {get{return m_currentCommandPool; }}
	public int commandPool {get{return GetCommandPool(); }}
	public int costPerTurn {get{return GetCostPerTurn();}}
	public int maxAvailableHenchmen {get{return m_maxAvailableHenchmen;}}
	public int currentWantedLevel {get{return m_currentWantedLevel; }}
	public int maxWantedLevel {get{return m_maxWantedLevel; }}
	public int currentInfamy {get{return m_currentInfamy; }}
	public int maxInfamy {get{return m_maxInfamy; }}
	public int currentIntel {get{return m_currentIntel; } set{ m_currentIntel = value; }}
	public int maxIntel {get{return m_maxIntel; }}
	public List<Asset> currentAssets {get{return m_currentAssets;}}
	public List<Asset> currentResearch {get{return m_currentResearch;}}
	public List<Asset> assetsInOrbit {get{return m_assetsInOrbit;}}
	public Region homeRegion {get{return m_homeRegion;}}
	public Base orgBase {get{return m_base;}}
	public int maxAssets {get{return m_maxAssets;} set { m_maxAssets = value; }}

}
