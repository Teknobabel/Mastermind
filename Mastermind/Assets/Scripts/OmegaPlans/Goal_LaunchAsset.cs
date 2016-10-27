using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class Goal_LaunchAsset : OPGoalBase, IObserver {

	public Asset m_asset;

	public override OPGoalBase GetObject ()
	{
		Goal_LaunchAsset g = Goal_LaunchAsset.CreateInstance<Goal_LaunchAsset> ();
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
		string s = "LAUNCH\n";

		if (m_asset != null) {
			s += m_asset.m_name.ToUpper () + "\nINTO ORBIT";
		}
			
		return s;

	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {
		case GameEvent.Mission_AssetLaunched:
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
