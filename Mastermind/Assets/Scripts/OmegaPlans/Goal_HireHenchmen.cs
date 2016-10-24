using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class Goal_HireHenchmen : OPGoalBase, IObserver {

	public TraitData m_trait;

	public override OPGoalBase GetObject ()
	{
		Goal_HireHenchmen g = Goal_HireHenchmen.CreateInstance<Goal_HireHenchmen> ();
		g.m_trait = m_trait;
		return g;
	}

	public override void Initialize ()
	{

		// add observers as needed to detect goal completion

	}

	public override string GetText ()
	{
		string s = "HIRE:\n";
		Debug.Log (m_trait);
		if (m_trait != null) {
			s += m_trait.m_name.ToUpper ();
		}

		return s;

	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		//		switch (thisGameEvent) {
		//		case GameEvent.Organization_HenchmenDismissed:
		//		case GameEvent.Organization_HenchmenHired:
		//
		//			break;
		//		}
	}
}
