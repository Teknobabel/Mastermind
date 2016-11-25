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

	private List<Henchmen> m_agents = new List<Henchmen>();

	private int 
	m_turnNumber = 0,
	m_turnToSpawnNextIntel = -1;

	private Organization m_player;
	private AgentOrganization m_agentOrg;
	private Region m_limbo;

	private List<IObserver>
	m_observers = new List<IObserver> ();

	private List<AssetToken> m_intelInPlay = new List<AssetToken>();

	public void Initialize ()
	{
		m_limbo = Region.CreateInstance<Region> ();
		m_limbo.Initialize (GameManager.instance.m_limboRegion);

		DetermineIntelSpawnTurn ();

		GameManager.instance.game = this;
	}

	public void AddAgentOrganizationToGame (AgentOrganization o)
	{
		m_agentOrg = o;
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

	public void AddAgentToGame (Henchmen a)
	{
		m_agents.Add (a);
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

	public Henchmen GetAgent (HenchmenData a)
	{
		foreach (Henchmen thisA in m_agents) {
			if (thisA.henchmenName == a.m_name) {
				return thisA;
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

	public Region GetRandomRegion (bool onlyEmptyRegions)
	{
		Region r = null;

		List<Region> validRegions = new List<Region> ();

		foreach (Region region in GameManager.instance.game.regions) {

			if (region.currentHenchmen.Count == 0 && onlyEmptyRegions) {

				validRegions.Add (region);
			} else if (region.currentHenchmen.Count < region.henchmenSlots.Count) {

				validRegions.Add (region);
			}
		}

		if (validRegions.Count > 0) {

			r = validRegions[Random.Range(0, validRegions.Count)];
		}

		return r;
	}

	public void IntelCaptured ()
	{
		if (m_intelInPlay.Count > 0) {
			m_intelInPlay.RemoveAt (0);
		}

		Notify (this, GameEvent.Organization_IntelCaptured);
	}

	public void SpawnIntel ()
	{
		Debug.Log ("<color=blue>Spawning Intel</color>");

		// don't spawn new intel if we are at max # in world at the same time

		if (m_intelInPlay.Count == GameManager.instance.game.director.m_maxIntelInWorld) {

			DetermineIntelSpawnTurn ();
			return;
		}

		// gather list of regions with room to spawn intel

		List<Region> r = new List<Region> ();

		foreach (Region thisR in GameManager.instance.game.m_regions) {

			if (thisR.id != GameManager.instance.game.player.homeRegion.id) {
				
				if (thisR.assetTokens.Count < GameManager.instance.game.m_director.maxTokenSlotsInRegion && thisR.id != GameManager.instance.game.player.homeRegion.id) {

					r.Add (thisR);

				} else {

					// check for any open slots
					foreach (TokenSlot ts in thisR.assetTokens) {
						if (ts.m_state == TokenSlot.State.None) {
							r.Add (thisR);
							break;
						}
					}
				}
			}
		}

		if (r.Count > 0) {

			int rand = Random.Range (0, r.Count);

			Region region = r[rand];

			region.AddAssetToken (GameManager.instance.m_intel);

			m_intelInPlay.Add (GameManager.instance.m_intel);

			Notify (this, GameEvent.Organization_IntelSpawned);

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = "Intel has appeared somewhere in the world!";
			t.m_resultType = GameEvent.Organization_IntelSpawned;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);



		}

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
	public AgentOrganization agentOrganization {get{return m_agentOrg;}}
	public Director director {get{return m_director;}}
	public List<Henchmen> henchmen {get{return m_henchmen; }}
	public int turnNumber {get{return m_turnNumber;}set{m_turnNumber = value; Notify (this, GameEvent.GameState_TurnNumberChanged); }}
	public Dictionary<int, Region> regionsByID {get{return m_regionsByID; }}
	public int turnToSpawnNextIntel {get{return m_turnToSpawnNextIntel; }}
	public List<AssetToken> intelInPlay {get{return m_intelInPlay; }}
	public List<Region> regions {get{ return m_regions; }}
	public List<Henchmen> agents {get{return m_agents;}}
	public Region limbo {get{return m_limbo; }}
}
