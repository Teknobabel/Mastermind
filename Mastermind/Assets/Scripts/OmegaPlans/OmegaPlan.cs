using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OmegaPlan : ScriptableObject {

	public enum State
	{
		None,
		Hidden,
		Revealed,
	}

	public class Goal
	{
		public enum State
		{
			None,
			Completed,
			Active,
		}

		public OPGoalBase m_goal = null;

		public State m_state = State.Active;
	}

	private State m_state = State.None;
	private string m_name = "Null";
	private string m_shortName = "Null";
//	private OPGoalBase[] m_goals;

	private List<Goal> m_goals = new List<Goal>();

	private int m_id = -1;

	public void Initialize (OmegaPlanData op, State state)
	{
		m_id = GameManager.instance.newID;
		m_name = op.m_name;
		m_shortName = op.m_shortName;
		m_state = state;
//		m_goals = op.m_goals;

		foreach (OPGoalBase g in op.m_goals) {

			Goal goal = new Goal ();
			OPGoalBase newGoal = g.GetObject ();
			newGoal.Initialize ();
			goal.m_goal = newGoal;
			m_goals.Add (goal);
		}

	}

	public int id {get{return m_id; }}
	public string opName {get{return m_name;}}
	public string opNameShort {get{return m_shortName;}}
	public List<Goal> goals {get{return m_goals;}}
	public State state {get{return m_state;}}
}
