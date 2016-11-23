using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Omega Plan Goal/Aquire Asset")]
public class Goal_AcquireAsset : OPGoalBase, IObserver {

	public Asset m_asset;

	public override OPGoalBase GetObject ()
	{
		Goal_AcquireAsset g = Goal_AcquireAsset.CreateInstance<Goal_AcquireAsset> ();
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
		string s = "ACQUIRE ASSET:\n";
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

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_resultsText = "OMEGA PLAN: " + m_omegaPlan.opName.ToUpper() + " Goal Completed - Acquire Asset: " + m_asset.m_name.ToUpper();
				t.m_resultType = GameEvent.OmegaPlan_GoalCompleted;
				GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
			}
			break;
		}
	}
}
