using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentOrganization : ScriptableObject, IOrganization, ISubject, IObserver {

	private Dictionary<int, List<TurnResultsEntry>> m_turnResults = new Dictionary<int, List<TurnResultsEntry>> (); // by turn number
	private Dictionary<GameEvent, List<TurnResultsEntry>> m_turnResultsByType = new Dictionary<GameEvent, List<TurnResultsEntry>> ();

	private List<AgentWrapper> m_currentAgents = new List<AgentWrapper>();

	private List<MissionWrapper> m_activeMissions = new List<MissionWrapper>();

	private int m_agentsToSpawn = 0;

	private List<IObserver>
	m_observers = new List<IObserver> ();

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

			// get a random empty region

			Region region = GameManager.instance.game.GetRandomRegion (true);

			if (region == null) {

				// if no empty regions exist, get a random region

				region = GameManager.instance.game.GetRandomRegion (false);
			}

			if (region != null) {

				Henchmen agent = null;

				if (henchmen == null) { // if a specific agent isn't supplied, choose a random agent
					
					int r = Random.Range (0, agentBank.Count);
					agent = agentBank [r];
				} else {

					agent = henchmen;
				}

				AgentWrapper aw = new AgentWrapper ();
				aw.m_agent = agent;
				aw.m_currentAIState = GameManager.instance.agentState_Idle;
				aw.AddObserver (this);
				m_currentAgents.Add (aw);

				region.AddAgent (aw);
				Debug.Log ("Agent: " + agent.henchmenName + " spawning in region: " + region.regionName);
			}
		}
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {

		case GameEvent.Agent_BecameVisible:
			Notify (subject, GameEvent.Agent_BecameVisible);
			break;
		}
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

	public List<AgentWrapper> currentAgents {get{ return m_currentAgents;}}
	public List<MissionWrapper> activeMissions {get{return m_activeMissions;}}
	public int agentsToSpawn {get{ return m_agentsToSpawn;} set{m_agentsToSpawn = value;}}
}
