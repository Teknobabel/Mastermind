using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OmegaPlan : ScriptableObject, ISubject {

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

	private List<Goal> m_goals = new List<Goal>();
	private List<AssetToken> m_mandatoryAssets = new List<AssetToken> ();

	private int m_id = -1;

	private List<IObserver>
	m_observers = new List<IObserver> ();

	public void Initialize (OmegaPlanData op, State state, Organization o)
	{
		m_id = GameManager.instance.newID;
		m_name = op.m_name;
		m_shortName = op.m_shortName;
		m_state = state;

		foreach (OPGoalBase g in op.m_goals) {

			Goal goal = new Goal ();
			OPGoalBase newGoal = g.GetObject ();
			newGoal.Initialize (this, o);
			goal.m_goal = newGoal;
			m_goals.Add (goal);
		}

		foreach (AssetToken a in op.m_mandatoryAssets) {
			m_mandatoryAssets.Add (a);
		}

	}

	public void ChangeState ( State newState)
	{
		m_state = newState;

		switch (newState) {
		case State.Revealed:
			
			Notify (this, GameEvent.Organization_OmegaPlanRevealed);
			break;
		}
	}

	public void GoalCompleted (OPGoalBase goal)
	{
		foreach (Goal g in m_goals) {
			if (g.m_state == Goal.State.Active && goal.id == g.m_goal.id) {
				g.m_state = Goal.State.Completed;
			}
		}
	}

	public int GetNumGoalsCompleted ()
	{
		int completed = 0;

		foreach (Goal g in m_goals) {
			if (g.m_state == Goal.State.Completed) {

				completed++;
			}
		}

		return completed;
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

	public int id {get{return m_id; }}
	public string opName {get{return m_name;}}
	public string opNameShort {get{return m_shortName;}}
	public List<Goal> goals {get{return m_goals;}}
	public State state {get{return m_state;}}
	public List<AssetToken> mandatoryAssets {get{return m_mandatoryAssets;}}
}
