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
	private OPGoal[] m_goals;

	private int m_id = -1;

	public void Initialize (OmegaPlanData op, State state)
	{
		m_id = GameManager.instance.newID;
		m_name = op.m_name;
		m_state = state;
		m_goals = op.m_goals;
	}

	public int id {get{return m_id; }}
	public string opName {get{return m_name;}}
	public OPGoal[] goals {get{return m_goals;}}
}
