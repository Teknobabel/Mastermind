using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class Goal_UseAssetOnAllTokenTypeInRegion : OPGoalBase, IObserver {

	public Asset m_asset;
	public TokenBase m_tokenType;
	public RegionData m_region;

	public override OPGoalBase GetObject ()
	{
		Goal_UseAssetOnAllTokenTypeInRegion g = Goal_UseAssetOnAllTokenTypeInRegion.CreateInstance<Goal_UseAssetOnAllTokenTypeInRegion> ();
		g.m_region = m_region;
		g.m_tokenType = m_tokenType;
		g.m_asset = m_asset;
		return g;
	}

	public override void Initialize (OmegaPlan op, Organization o)
	{
		base.Initialize (op, o);

		// add observers as needed to detect goal completion

	}

	public override string GetText ()
	{
		string s = "USE ";

		if (m_asset != null) {
			s += m_asset.m_name.ToUpper () + " ON ALL\n";
		}

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
