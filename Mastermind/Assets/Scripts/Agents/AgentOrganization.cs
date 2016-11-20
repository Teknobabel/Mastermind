using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentOrganization : ScriptableObject, IOrganization {

	private Dictionary<int, List<TurnResultsEntry>> m_turnResults = new Dictionary<int, List<TurnResultsEntry>> (); // by turn number
	private Dictionary<GameEvent, List<TurnResultsEntry>> m_turnResultsByType = new Dictionary<GameEvent, List<TurnResultsEntry>> ();

	private List<AgentWrapper> m_currentAgents = new List<AgentWrapper>();

	private List<MissionWrapper> m_activeMissions = new List<MissionWrapper>();

	private int m_agentsToSpawn = 0;

	public void Initialize (string orgName)
	{
		// spawn any starting agent as per director

		Director d = GameManager.instance.game.director;
		Game g = GameManager.instance.game;

		if (GameManager.instance.game.director.m_startingAgentData.Length > 0) {

			for (int i=0; i < d.m_startingAgentData.Length; i++)
			{
				Henchmen h = g.GetAgent (d.m_startingAgentData [i]);

				SpawnAgentInWorld (h);
			}
		}
	}

	public void AddMission (MissionWrapper mw)
	{
		mw.m_mission.InitializeMission(mw);

		foreach (Henchmen thisH in mw.m_henchmen) {
			if (thisH.currentState != Henchmen.state.OnMission) {
				thisH.ChangeState (Henchmen.state.OnMission);
			}
		}

		m_activeMissions.Add (mw);
	}

	public void PlayerWantedLevelIncreased ()
	{
		m_agentsToSpawn++;
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

	public void SpawnAgentInWorld (Henchmen henchmen)
	{
		Debug.Log ("Spawning Agent");

		List<Henchmen> agentBank = new List<Henchmen> ();
		List<Henchmen> currentHenchmen = new List<Henchmen>();

		// sort out currently active henchmen from bank list

		foreach (AgentWrapper aw in m_currentAgents) {
			currentHenchmen.Add (aw.m_agent);
		}

		foreach (Henchmen a in GameManager.instance.game.agents) {

			if (!currentHenchmen.Contains (a)) {

				agentBank.Add (a);
			}
		}

		if (agentBank.Count > 0) {

			// select a region to place agent in

			List<Region> emptyRegions = new List<Region> ();
			List<Region> validRegions = new List<Region> ();

			foreach (Region region in GameManager.instance.game.regions) {

				if (region.id != GameManager.instance.game.player.homeRegion.id) {

					if (region.currentHenchmen.Count == 0) {

						emptyRegions.Add (region);
					} else if (region.currentHenchmen.Count < region.henchmenSlots.Count) {

						validRegions.Add (region);
					}
				}
			}

			Region randRegion = null;

			if (emptyRegions.Count > 0) {

				randRegion = emptyRegions[Random.Range(0, emptyRegions.Count)];

			} else if (validRegions.Count > 0) {

				randRegion = validRegions[Random.Range(0, validRegions.Count)];
			}

			if (randRegion != null) {

				Henchmen agent = null;

				if (henchmen == null) {
					
					int r = Random.Range (0, agentBank.Count);
					agent = agentBank [r];
				} else {

					agent = henchmen;
				}

				AgentWrapper aw = new AgentWrapper ();
				aw.m_agent = agent;
				m_currentAgents.Add (aw);

				randRegion.AddHenchmen (agent);
				Debug.Log ("Agent: " + agent.henchmenName + " spawning in region: " + randRegion.regionName);
			}
		}
	}

	public List<AgentWrapper> currentAgents {get{ return m_currentAgents;}}
	public List<MissionWrapper> activeMissions {get{return m_activeMissions;}}
	public int agentsToSpawn {get{ return m_agentsToSpawn;} set{m_agentsToSpawn = value;}}
}
