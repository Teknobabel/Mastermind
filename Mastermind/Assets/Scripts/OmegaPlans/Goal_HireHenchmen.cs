using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Omega Plan Goal/Hire Henchmen")]
public class Goal_HireHenchmen : OPGoalBase, IObserver {

	public TraitData m_trait;

	public override OPGoalBase GetObject ()
	{
		Goal_HireHenchmen g = Goal_HireHenchmen.CreateInstance<Goal_HireHenchmen> ();
		g.m_trait = m_trait;
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
		string s = "HIRE:\n";
//		Debug.Log (m_trait);
		if (m_trait != null) {
			s += m_trait.m_name.ToUpper ();
		}

		return s;

	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		if (m_omegaPlan.state == OmegaPlan.State.Revealed) {
			
			switch (thisGameEvent) {
			case GameEvent.Organization_HenchmenHired:
				Organization o = (Organization)subject;

				foreach (Henchmen h in o.currentHenchmen) {
				
					if (h.HasTrait (m_trait)) {
					
						// goal is met

						m_omegaPlan.GoalCompleted (this);
						GameManager.instance.game.player.RemoveObserver (this);

						TurnResultsEntry t = new TurnResultsEntry ();
						t.m_iconType = TurnResultsEntry.IconType.OmegaPlan;
						t.m_resultsText = "OMEGA PLAN: " + m_omegaPlan.opName.ToUpper () + " Goal Completed - Hire: " + m_trait.m_name.ToUpper ();
						t.m_resultType = GameEvent.OmegaPlan_GoalCompleted;
						GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
					}
				}
				break;
			}
		}
	}
}
