using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Omega Plan Goal/Combine Assets")]
public class Goal_CombineAssets : OPGoalBase, IObserver {

	public Asset m_asset1;
	public Asset m_asset2;
	public Asset m_resultAsset;

	public override OPGoalBase GetObject ()
	{
		Goal_CombineAssets g = Goal_CombineAssets.CreateInstance<Goal_CombineAssets> ();
		g.m_asset1 = m_asset1;
		g.m_asset2 = m_asset2;
		g.m_resultAsset = m_resultAsset;
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

		if (m_asset1 != null && m_asset2 != null && m_resultAsset != null) {
			s += m_asset1.m_name.ToUpper () + " AND " + m_asset2.m_name.ToUpper();
			s += "\nT0 CREATE " + m_resultAsset.m_name.ToUpper ();
		}

		return s;
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		if (m_omegaPlan.state == OmegaPlan.State.Revealed) {
			
			switch (thisGameEvent) {
			case GameEvent.Organization_AssetGained:
			
				Asset a = (Asset)subject;

				if (a == m_resultAsset) {
				
					// goal is met

					m_omegaPlan.GoalCompleted (this);
					GameManager.instance.game.player.RemoveObserver (this);

					TurnResultsEntry t = new TurnResultsEntry ();
					t.m_iconType = TurnResultsEntry.IconType.OmegaPlan;
					t.m_resultsText = "OMEGA PLAN: " + m_omegaPlan.opName.ToUpper () + " Goal Completed - Create: " + m_resultAsset.m_name.ToUpper ();
					t.m_resultType = GameEvent.OmegaPlan_GoalCompleted;
					GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
				}
				break;
			}
		}
	}
}
