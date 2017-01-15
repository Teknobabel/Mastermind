using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrganizationBase : ScriptableObject, ISubject {

	protected List<MissionWrapper> m_activeMissions = new List<MissionWrapper>();

	protected Dictionary<int, List<TurnResultsEntry>> m_turnResults = new Dictionary<int, List<TurnResultsEntry>> (); // by turn number
	protected Dictionary<GameEvent, List<TurnResultsEntry>> m_turnResultsByType = new Dictionary<GameEvent, List<TurnResultsEntry>> ();

	private List<IObserver>
	m_observers = new List<IObserver> ();

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

	public virtual void Initialize (string orgName) 
	{
		
	}

	public virtual void AddMission (MissionWrapper mw)
	{
		mw.m_mission.InitializeMission(mw);

		foreach (Henchmen thisH in mw.m_henchmen) {
			if (thisH.currentState != Henchmen.state.OnMission) {
				thisH.ChangeState (Henchmen.state.OnMission);
			}
		}

		if (mw.m_henchmenInFocus != null && mw.m_henchmenInFocus.currentState != Henchmen.state.OnMission) {

			mw.m_henchmenInFocus.ChangeState(Henchmen.state.OnMission);
		}

		m_activeMissions.Add (mw);
	}

	public virtual void MissionCompleted (MissionWrapper a)
	{
		if (a.m_henchmenInFocus != null) {
			a.m_henchmenInFocus.ChangeState (Henchmen.state.Idle);
		}

		foreach (Henchmen h in a.m_henchmen) {
			h.ChangeState (Henchmen.state.Idle);
		}

		if (m_activeMissions.Contains (a)) {
			m_activeMissions.Remove (a);
		}
	}

	public virtual void CancelMission (MissionWrapper mw)
	{
		for (int i = 0; i < m_activeMissions.Count; i++) {

			MissionWrapper activeMission = m_activeMissions [i];

			if (activeMission == mw) {

				m_activeMissions.RemoveAt (i);
				mw.m_mission.CancelMission (mw);
			}
		}
	}

	public MissionWrapper GetMission (Henchmen h)
	{

		foreach (MissionWrapper a in m_activeMissions) {

			if ((a.m_henchmenInFocus != null && a.m_henchmenInFocus == h) || (a.m_agentInFocus != null && a.m_agentInFocus.m_agent == h)) {
				return a;
			}

			foreach (Henchmen thisH in a.m_henchmen)
			{
				if (h.id == thisH.id) {
					return a;
				}
			}

			foreach (AgentWrapper thisAW in a.m_agents)
			{
				if (h.id == thisAW.m_agent.id) {
					return a;
				}
			}
		}

		return null;
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

	public List<MissionWrapper> activeMissions {get{return m_activeMissions;}}
	public Dictionary<int, List<TurnResultsEntry>> turnResults {get{return m_turnResults; }}
	public Dictionary<GameEvent, List<TurnResultsEntry>> turnResultsByType {get{return m_turnResultsByType; }}
}
