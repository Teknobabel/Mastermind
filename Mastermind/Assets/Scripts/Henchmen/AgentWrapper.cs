using UnityEngine;
using System.Collections;

public class AgentWrapper {

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
}
