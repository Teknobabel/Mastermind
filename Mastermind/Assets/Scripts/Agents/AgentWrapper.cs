using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentWrapper : ISubject {

	public enum AgentEvents
	{
		RegionSearched,
		BaseFound,
		HenchmenFound,
		PlayerControlTokenFound,
		IntelFound,
	}

	public enum State
	{
		Idle,
		Fleeing,
		Attacking,
		Invading,
		Stalking,
	}

	public enum VisibilityState
	{
		Hidden,
		Visible,
		Tracked,
	}

	public Henchmen m_agent = null;
	public State m_state = State.Idle;
	public VisibilityState m_vizState = VisibilityState.Hidden;
	public bool m_hasBeenRevealed = false;
	public List<AgentEvents> m_agentEvents = new List<AgentEvents>();
	public IAgentAIState m_currentAIState;
	public int m_turnEnteredState = 0;

	private List<IObserver>
	m_observers = new List<IObserver> ();

	public void ChangeVisibilityState (VisibilityState newState)
	{
		m_vizState = newState;

		switch (newState) {

		case VisibilityState.Visible:

			if (!m_hasBeenRevealed) {

				m_hasBeenRevealed = true;
				Notify (this, GameEvent.Agent_BecameVisible);
			}
			break;
		}
	}

	public void ChangeAIState (IAgentAIState newState)
	{

		if (!GameManager.instance.game.agentOrganization.agentsByState.ContainsKey (newState)) {

			List<AgentWrapper> awList = new List<AgentWrapper> ();
			awList.Add (this);
			GameManager.instance.game.agentOrganization.agentsByState.Add (newState, awList);

		} else {
			
			GameManager.instance.game.agentOrganization.agentsByState [newState].Add (this);

		}

		if (m_currentAIState != null) {
			m_currentAIState.ExitState (this);
		}

		m_currentAIState = newState;

		m_currentAIState.EnterState (this);
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
}
