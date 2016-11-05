using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Omega Plan Goal/Use Asset")]
public class Goal_UseAsset : OPGoalBase, IObserver {

	public Asset m_asset;

	public override OPGoalBase GetObject ()
	{
		Goal_UseAsset g = Goal_UseAsset.CreateInstance<Goal_UseAsset> ();
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
		string s = "USE:\n";
		Debug.Log (m_asset);
		if (m_asset != null) {
			s += m_asset.m_name.ToUpper ();
		}

		return s;
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {
		case GameEvent.Mission_AssetUsed:
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
