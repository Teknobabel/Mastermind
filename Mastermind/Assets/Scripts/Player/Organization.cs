using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Organization : ScriptableObject, ISubject {

	private string m_name = "Null";

	private int m_currentInfamy = 0;
	private int m_maxInfamy = 0;
	private int m_currentWantedLevel = 0;
	private int m_maxWantedLevel = 1;
	private int m_commandPool = 0;
	private int m_currentCommandPool = 0;
	private int m_maxAvailableHenchmen = 1;
	private int m_currentIntel = 0;
	private int m_maxIntel = 1;

	private List<Henchmen> m_currentHenchmen;
	private List<Henchmen> m_availableHenchmen;
	private List<OmegaPlan> m_omegaPlans;
	private List<Asset> m_currentAssets;

	private Dictionary<int, OmegaPlan> m_omegaPlansByID = new Dictionary<int, OmegaPlan> ();
	private Dictionary<int, MenuTab> m_menuTabs;

	private Dictionary<int, List<TurnResultsEntry>> m_turnResults = new Dictionary<int, List<TurnResultsEntry>> (); // by turn number
	private Dictionary<GameEvent, List<TurnResultsEntry>> m_turnResultsByType = new Dictionary<GameEvent, List<TurnResultsEntry>> ();

	private List<IObserver>
	m_observers = new List<IObserver> ();

	private List<MissionWrapper> m_activeMissions = new List<MissionWrapper>();

	private Region m_homeRegion = null;

	private Base m_base;

	public void RefillCommandPool ()
	{
		m_currentCommandPool = m_commandPool;
		Notify (this, GameEvent.Organization_CommandPoolChanged);
	}

	public void HireHenchmen (int henchmenID)
	{
		for (int i=0; i < m_availableHenchmen.Count; i++)
		{
			Henchmen h = m_availableHenchmen [i];
			if (h.id == henchmenID) {
				UseCommandPoints (h.hireCost);
				m_currentHenchmen.Add (h);
				m_availableHenchmen.RemoveAt (i);
//				h.SetRegion (m_homeRegion);
				m_homeRegion.AddHenchmen(h);

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_resultsText = h.henchmenName + " joins " + m_name;
				t.m_resultType = GameEvent.Organization_HenchmenHired;
				AddTurnResults (GameManager.instance.game.turnNumber, t);

				Notify (this, GameEvent.Organization_HenchmenHired);
				break;
			}
		}
	}

	public void AddHenchmen (Henchmen h)
	{
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
				List<MissionWrapper> cancelledMissions = new List<MissionWrapper>();

				foreach (MissionWrapper mission in m_activeMissions) {

					if (mission.m_henchmenInFocus != null && mission.m_henchmenInFocus == h) {

						cancelledMissions.Add (mission);
					}
					else if (mission.m_henchmen.Count > 0 && mission.m_henchmen.Contains (h)) {

						mission.m_henchmen.Remove (h);

						if (mission.m_henchmen.Count == 0) {
							cancelledMissions.Add (mission);
						}
					}
				}

				while (cancelledMissions.Count > 0) {

					MissionWrapper mission = cancelledMissions [0];
					cancelledMissions.RemoveAt (0);

					activeMissions.Remove (mission);
				}

				Notify (this, GameEvent.Organization_HenchmenFired);
				break;
			}
		}
	}

	public void AddMission (MissionWrapper mr)
	{
		MissionWrapper a = new MissionWrapper ();
		a.m_mission = mr.m_mission;
		a.m_henchmen = mr.m_henchmen;
		a.m_region = mr.m_region;
		a.m_henchmenInFocus = mr.m_henchmenInFocus;
		a.m_tokenInFocus = mr.m_tokenInFocus;
		a.m_floorInFocus = mr.m_floorInFocus;
		a.m_scope = mr.m_scope;

		a.m_mission.InitializeMission (a);

		foreach (Henchmen thisH in a.m_henchmen) {
			if (thisH.currentState != Henchmen.state.OnMission) {
				thisH.ChangeState (Henchmen.state.OnMission);
			}
		}

		m_activeMissions.Add (a);
	}

	public void MissionCompleted (MissionWrapper a)
	{
		foreach (Henchmen h in a.m_henchmen) {
			h.ChangeState (Henchmen.state.Idle);
		}
		if (m_activeMissions.Contains (a)) {
			m_activeMissions.Remove (a);
		}
	}

	public MissionBase GetMission (Henchmen h)
	{
		MissionBase m = null;

		foreach (MissionWrapper a in m_activeMissions) {
			foreach (Henchmen thisH in a.m_henchmen)
			{
				if (h.id == thisH.id) {
					return a.m_mission;
				}
			}
		}

		return m;
	}

	public MissionBase GetMission (Region r)
	{
		foreach (MissionWrapper a in m_activeMissions) {
			if (a.m_region != null && r.id == a.m_region.id) {
				return a.m_mission;
			}
		}

		return null;
	}

	public void AddHenchmenToAvailablePool (Henchmen h)
	{
		m_availableHenchmen.Add (h);
	}

	public void AddTurnResults (int turn, TurnResultsEntry t)
	{
		t.m_turnNumber = turn;

		if (m_turnResults.ContainsKey (turn)) {
			List<TurnResultsEntry> tRE = m_turnResults [turn];
			tRE.Add (t);
			m_turnResults [turn] = tRE;
		} else {
			List<TurnResultsEntry> newTRE = new List<TurnResultsEntry> ();
			newTRE.Add (t);
			m_turnResults.Add (turn, newTRE);
		}

		if (m_turnResultsByType.ContainsKey (t.m_resultType)) {
			List<TurnResultsEntry> tRE = m_turnResultsByType [t.m_resultType];
			tRE.Add (t);
			m_turnResultsByType [t.m_resultType] = tRE;
		} else {
			List<TurnResultsEntry> newTRE = new List<TurnResultsEntry> ();
			newTRE.Add (t);
			m_turnResultsByType.Add (t.m_resultType, newTRE);
		}
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
			t.m_resultsText = "Wanted Level has increased to " + m_currentWantedLevel.ToString() + "!";
			t.m_resultType = GameEvent.Organization_WantedLevelIncreased;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public void GainInfamy (int amount)
	{
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

	public void AddAsset (Asset a)
	{
		m_currentAssets.Add (a);

		Notify (a, GameEvent.Organization_AssetGained);
	}

	public void RemoveAsset (Asset a)
	{
		if (m_currentAssets.Contains (a)) {
			m_currentAssets.Remove (a);

			Notify (a, GameEvent.Organization_AssetRemoved);
		}
	}

	public MissionWrapper GetMissionForHenchmen (Henchmen h)
	{
		foreach (MissionWrapper a in m_activeMissions) {
			if (a.m_henchmen.Contains (h)) {
				return (a);
			}
		}
		return null;
	}

	public void Initialize (Director d, Game g, string orgName)
	{
		m_name = orgName;
		m_currentWantedLevel = d.m_startingWantedLevel;
		m_maxWantedLevel = d.m_maxWantedLevel;
		m_maxInfamy = d.m_maxInfamy;
		m_commandPool = d.m_startingCommandPool;
		m_maxAvailableHenchmen = d.m_startingHenchmen;
		m_currentIntel = d.m_startingIntel;
		m_maxIntel = d.m_maxIntel;

		// add any starting assets

		m_currentAssets = new List<Asset> ();

		foreach (Asset a in d.m_startingAssets) {
			AddAsset (a);
		}

		// initialize base

		m_base = new Base ();
		m_base.Initialize (6);

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
			}
		}

		// set up menu tabs

		m_menuTabs = new Dictionary<int, MenuTab> ();

		MenuTab henchTab = new MenuTab ();
		henchTab.m_name = "HENCHMEN";
		henchTab.m_menuState = MenuState.State.HenchmenMenu;
		henchTab.Initialize ();
		m_menuTabs.Add (henchTab.id, henchTab);

		MenuTab lairTab = new MenuTab ();
		lairTab.m_name = "LAIR";
		lairTab.m_menuState = MenuState.State.LairMenu;
		lairTab.Initialize ();
		m_menuTabs.Add (lairTab.id, lairTab);

		MenuTab worldTab = new MenuTab ();
		worldTab.m_name = "WORLD";
		worldTab.m_menuState = MenuState.State.WorldMenu;
		worldTab.Initialize ();
		m_menuTabs.Add (worldTab.id, worldTab);

		MenuTab activityTab = new MenuTab ();
		activityTab.m_name = "ACTIVITY";
		activityTab.m_menuState = MenuState.State.ActivityMenu;
		activityTab.Initialize ();
		m_menuTabs.Add (activityTab.id, activityTab);

		foreach (OmegaPlan op in m_omegaPlans) {

			MenuTab opTab = new MenuTab ();
			opTab.m_name = "OMEGA PLAN";
			opTab.objectID = op.id;
			opTab.m_menuState = MenuState.State.OmegaPlanMenu;
			opTab.Initialize ();
			m_menuTabs.Add (opTab.id, opTab);
		}

		AddObserver (TabMenu.instance);

		Notify (this, GameEvent.Organization_Initialized);
	}

	public void AddObserver (IObserver observer)
	{
		if (!m_observers.Contains(observer))
		{
			m_observers.Add (observer);
		}
	}

	public void RemoveObserver (IObserver observer)
	{
		if (m_observers.Contains(observer))
		{
			m_observers.Remove(observer);
		}
	}

	public void Notify (ISubject subject, GameEvent thisGameEvent)
	{
		List<IObserver> observers = new List<IObserver> (m_observers);

		for (int i=0; i < observers.Count; i++)
		{
			observers[i].OnNotify(subject, thisGameEvent);
		}
	}

	private int GetCostPerTurn ()
	{
		int cost = 0;

		foreach (Henchmen h in m_currentHenchmen) {
			cost += h.costPerTurn;
		}

		return cost;
	}

	public Dictionary<int, MenuTab> menuTabs {get{return m_menuTabs; }}
	public List<OmegaPlan> omegaPlans {get{return m_omegaPlans; }}
	public Dictionary<int, OmegaPlan> omegaPlansByID {get{return m_omegaPlansByID; }}
	public List<Henchmen> availableHenchmen {get{return m_availableHenchmen; }}
	public List<Henchmen> currentHenchmen {get{return m_currentHenchmen; }}
	public int currentCommandPool {get{return m_currentCommandPool; }}
	public int commandPool {get{return m_commandPool; }}
	public int costPerTurn {get{return GetCostPerTurn();}}
	public int maxAvailableHenchmen {get{return m_maxAvailableHenchmen;}}
	public int currentWantedLevel {get{return m_currentWantedLevel; }}
	public int maxWantedLevel {get{return m_maxWantedLevel; }}
	public int currentInfamy {get{return m_currentInfamy; }}
	public int maxInfamy {get{return m_maxInfamy; }}
	public int currentIntel {get{return m_currentIntel; }}
	public int maxIntel {get{return m_maxIntel; }}
	public string orgName {get{return m_name;}}
	public List<Asset> currentAssets {get{return m_currentAssets;}}
	public List<MissionWrapper> activeMissions {get{return m_activeMissions;}}
	public Dictionary<int, List<TurnResultsEntry>> turnResults {get{return m_turnResults; }}
	public Dictionary<GameEvent, List<TurnResultsEntry>> turnResultsByType {get{return m_turnResultsByType; }}
	public Region homeRegion {get{return m_homeRegion;}}
	public Base orgBase {get{return m_base;}}

}
