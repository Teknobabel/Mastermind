using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class Goal_BuildAsset : OPGoalBase, IObserver {

	public Asset m_asset;

	public override OPGoalBase GetObject ()
	{
		Goal_BuildAsset g = Goal_BuildAsset.CreateInstance<Goal_BuildAsset> ();
		g.m_asset = m_asset;
		return g;
	}

	public override void Initialize ()
	{

		// add observers as needed to detect goal completion

	}

	public override string GetText ()
	{
		string s = "BUILD:\n";
		Debug.Log (m_asset);
		if (m_asset != null) {
			s += m_asset.m_name.ToUpper ();
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
