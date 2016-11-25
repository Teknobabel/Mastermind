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
