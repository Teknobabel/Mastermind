using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class Goal_SeizeAllTokenTypeInRegion : OPGoalBase, IObserver {

	public TokenBase m_tokenType;
	public RegionData m_region;

	public override OPGoalBase GetObject ()
	{
		Goal_SeizeAllTokenTypeInRegion g = Goal_SeizeAllTokenTypeInRegion.CreateInstance<Goal_SeizeAllTokenTypeInRegion> ();
		g.m_region = m_region;
		g.m_tokenType = m_tokenType;
		return g;
	}

	public override void Initialize ()
	{

		// add observers as needed to detect goal completion

	}

	public override string GetText ()
	{
		string s = "SEIZE CONTROL OF ALL:\n";

		if (m_tokenType != null) {
			s += m_tokenType.m_name.ToUpper () + "S\n";
		}

		if (m_region != null) {
			s += " IN " + m_region.m_name.ToUpper ();
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
