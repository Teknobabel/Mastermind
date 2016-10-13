using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Organization : ScriptableObject, ISubject {

	private string m_name = "Null";

	private int m_currentInfamy = 0;
	private int m_maxInfamy = 0;
	private int m_currentWantedLevel = 0;
	private int m_commandPool = 0;
	private int m_currentCommandPool = 0;
	private int m_maxAvailableHenchmen = 1;

	private List<Henchmen> m_currentHenchmen;
	private List<Henchmen> m_availableHenchmen;
	private List<OmegaPlan> m_omegaPlans;
	private Dictionary<MenuState.State, MenuTab> m_menuTabs;

	private List<IObserver>
	m_observers = new List<IObserver> ();

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
				Notify (this, GameEvent.Organization_HenchmenHired);
				break;
			}
		}
	}

	public void AddHenchmenToAvailablePool (Henchmen h)
	{
		m_availableHenchmen.Add (h);
	}

	public void UseCommandPoints (int points)
	{
		m_currentCommandPool = Mathf.Clamp (m_currentCommandPool - points, 0, 99);
		Notify (this, GameEvent.Organization_CommandPoolChanged);
	}

	public void Initialize (Director d, Game g)
	{
		m_currentWantedLevel = d.m_startingWantedLevel;
		m_maxInfamy = d.m_maxInfamy;
		m_commandPool = d.m_startingCommandPool;
		m_maxAvailableHenchmen = d.m_startingHenchmen;

		// select Omega Plans

		List<OmegaPlanData> ops = new List<OmegaPlanData> ();
		m_omegaPlans = new List<OmegaPlan> ();
		foreach (OmegaPlanData o in d.m_omegaPlanBank) {
			ops.Add (o);
		}

		int revealed = 0;
		for (int i = 0; i < d.m_startingOmegaPlans; i++) {
			if (ops.Count > 0) {
				int rand = Random.Range (0, ops.Count);
				OmegaPlanData newOPData = ops [rand];
				ops.RemoveAt (rand);

				OmegaPlan newOP = OmegaPlan.CreateInstance<OmegaPlan> ();

				if (revealed < d.m_startingRevealedOmegaPlans) {
					newOP.Initialize (newOPData, OmegaPlan.State.Revealed);
					revealed++;
				} else {
					newOP.Initialize (newOPData, OmegaPlan.State.Hidden);
				}

				m_omegaPlans.Add (newOP);
			}
		}

		// select starting Henchmen

		m_currentHenchmen = new List<Henchmen> ();
		m_availableHenchmen = new List<Henchmen> ();

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

		for (int i = 0; i < d.m_startingHenchmen; i++) {
			if (bank.Count > 0) {
				int rand = Random.Range (0, bank.Count);
				Henchmen newH = bank [rand];
				m_availableHenchmen.Add (newH);
				bank.RemoveAt (rand);
			}
		}

		// set up menu tabs

		m_menuTabs = new Dictionary<MenuState.State, MenuTab> ();
		MenuTab henchTab = new MenuTab ();
		henchTab.m_name = "HENCHMEN";
		henchTab.m_menuState = MenuState.State.HenchmenMenu;
		m_menuTabs.Add (henchTab.m_menuState, henchTab);

		MenuTab lairTab = new MenuTab ();
		lairTab.m_name = "LAIR";
		lairTab.m_menuState = MenuState.State.LairMenu;
		m_menuTabs.Add (lairTab.m_menuState, lairTab);

		MenuTab worldTab = new MenuTab ();
		worldTab.m_name = "WORLD";
		worldTab.m_menuState = MenuState.State.WorldMenu;
		m_menuTabs.Add (worldTab.m_menuState, worldTab);

		MenuTab activityTab = new MenuTab ();
		activityTab.m_name = "ACTIVITY";
		activityTab.m_menuState = MenuState.State.ActivityMenu;
		m_menuTabs.Add (activityTab.m_menuState, activityTab);

//		foreach (OmegaPlan op in m_omegaPlans) {

			MenuTab opTab = new MenuTab ();
			opTab.m_name = "OMEGA PLAN";
			opTab.m_menuState = MenuState.State.OmegaPlanMenu;
			m_menuTabs.Add (opTab.m_menuState, opTab);
//		}

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

	public Dictionary<MenuState.State, MenuTab> menuTabs {get{return m_menuTabs; }}
	public List<Henchmen> availableHenchmen {get{return m_availableHenchmen; }}
	public List<Henchmen> currentHenchmen {get{return m_currentHenchmen; }}
	public int currentCommandPool {get{return m_currentCommandPool; }}
	public int commandPool {get{return m_commandPool; }}
	public int costPerTurn {get{return GetCostPerTurn();}}
	public int maxAvailableHenchmen {get{return m_maxAvailableHenchmen;}}
	public int currentWantedLevel {get{return m_currentWantedLevel; }}
}
