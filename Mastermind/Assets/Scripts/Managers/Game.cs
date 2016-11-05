using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : ScriptableObject, ISubject {

	private Director m_director;

	private List<Region> m_regions = new List<Region>();
	private Dictionary<RegionData.RegionGroup, List<Region>> m_regionsByGroup = new Dictionary<RegionData.RegionGroup, List<Region>>();
	private Dictionary <int, Region> m_regionsByID = new Dictionary<int, Region>();

	private List<Henchmen> m_henchmen = new List<Henchmen>();
	private Dictionary<int, List<Henchmen>> m_henchmenByRank = new Dictionary<int, List<Henchmen>>();
	private Dictionary<int, Henchmen> m_henchmenByID = new Dictionary<int, Henchmen>();

	private int 
	m_randomSeed = 0,
	m_turnNumber = 0,
	m_turnToSpawnNextIntel = -1;

	private Organization m_player;

	private List<IObserver>
	m_observers = new List<IObserver> ();

	public void Initialize ()
	{
		m_randomSeed = (int)System.DateTime.Now.Ticks;
		Random.InitState(m_randomSeed);

		DetermineIntelSpawnTurn ();
	}

	public void AddOrganizationToGame (Organization o)
	{
		m_player = o;
	}

	public void AddDirectorToGame (Director d)
	{
		m_director = d;
	}

	public void AddRegionToGame (Region r)
	{
		m_regions.Add (r);

		if (m_regionsByGroup.ContainsKey(r.regionGroup)){
			List<Region> rList = m_regionsByGroup [r.regionGroup];
			rList.Add (r);
			m_regionsByGroup [r.regionGroup] = rList;
		} else {
			List<Region> rList = new List<Region> ();
			rList.Add (r);
			m_regionsByGroup.Add (r.regionGroup, rList);
		}

		if (!m_regionsByID.ContainsKey (r.id)) {
			m_regionsByID.Add (r.id, r);
		}
	}

	public void AddHenchmanToGame (Henchmen h)
	{
		m_henchmen.Add (h);
		if (m_henchmenByRank.ContainsKey (h.rank)) {
			List<Henchmen> l = m_henchmenByRank [h.rank];
			l.Add (h);
			m_henchmenByRank [h.rank] = l;
		} else {
			List<Henchmen> l = new List<Henchmen> ();
			l.Add (h);
			m_henchmenByRank.Add (h.rank, l);
		}

		if (!m_henchmenByID.ContainsKey (h.id)) {
			m_henchmenByID.Add (h.id, h);
		}

	}

	public List<Henchmen> GetHenchmenByRank (int rank)
	{
		List<Henchmen> l = new List<Henchmen> ();

		if (m_henchmenByRank.ContainsKey (rank)) {
			List<Henchmen> hList = m_henchmenByRank [rank];
			foreach (Henchmen h in hList) {
				l.Add (h);
			}
		}

		return l;
	}

	public Henchmen GetHenchmenByID (int id)
	{
		Henchmen h = null;

		if (m_henchmenByID.ContainsKey(id))
		{
			h = m_henchmenByID [id];
		}

		return h;
	}

	public Henchmen GetHenchmen (HenchmenData h)
	{
		foreach (Henchmen thisH in m_henchmen) {
			if (thisH.henchmenName == h.m_name) {
				return thisH;
			}
		}

		return null;
	}

	public Dictionary<RegionData.RegionGroup, List<Region>> GetAllRegionsByGroup ()
	{
		return m_regionsByGroup;
	}

	public List<Region> GetAllRegions ()
	{
		return m_regions;
	}

	public void SpawnIntel ()
	{
		Debug.Log ("<color=blue>Spawning Intel</color>");

		DetermineIntelSpawnTurn ();
	}

	public void DetermineIntelSpawnTurn ()
	{
		m_turnToSpawnNextIntel = m_turnNumber + Random.Range (m_director.m_intelSpawnLowerBounds, m_director.m_intelSpawnUpperBounds);
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

	public Organization player {get{return m_player; }}
	public Director director {get{return m_director;}}
	public List<Henchmen> henchmen {get{return m_henchmen; }}
	public int turnNumber {get{return m_turnNumber;}set{m_turnNumber = value; Notify (this, GameEvent.GameState_TurnNumberChanged); }}
	public Dictionary<int, Region> regionsByID {get{return m_regionsByID; }}
	public int turnToSpawnNextIntel {get{return m_turnToSpawnNextIntel; }}
}
