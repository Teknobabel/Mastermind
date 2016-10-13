using UnityEngine;
using System.Collections;

public class OmegaPlan : ScriptableObject {

	public enum State
	{
		None,
		Hidden,
		Revealed,
	}

	private State m_state = State.None;
	private string m_name = "Null";

	public void Initialize (OmegaPlanData op, State state)
	{
		m_name = op.m_name;
		m_state = state;
	}

}
