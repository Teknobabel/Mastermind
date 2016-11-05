using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Omega Plan Goal/Build Asset")]
public class Goal_BuildAsset : OPGoalBase, IObserver {

	public Asset m_asset;

	public override OPGoalBase GetObject ()
	{
		Goal_BuildAsset g = Goal_BuildAsset.CreateInstance<Goal_BuildAsset> ();
		g.m_asset = m_asset;
		return g;
	}

	public override void Initialize (OmegaPlan op, Organization o)
	{
		base.Initialize (op, o);

		// add observers as needed to detect goal completion

		o.AddObserver(this);

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
		switch (thisGameEvent) {
		case GameEvent.Organization_AssetGained:
			Asset a = (Asset)subject;
			if (a == m_asset) {
				// goal is met
				m_omegaPlan.GoalCompleted(this);
				GameManager.instance.game.player.RemoveObserver (this);
			}
			break;
		}
	}

}
