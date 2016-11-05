using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Omega Plan Goal/Seize All Tokens of Type in Region")]
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

	public override void Initialize (OmegaPlan op, Organization o)
	{
		base.Initialize (op, o);

		// add observers as needed to detect goal completion

		o.AddObserver(this);

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
		switch (thisGameEvent) {
		case GameEvent.Region_ControlTokenSeized:
			Region r = (Region)subject;
			if (r == m_region) {

				// check if all control tokens of type in region belong to player

				bool seizedAllTokens = true;
				foreach (Region.TokenSlot t in r.controlTokens) {

					if (t.m_type == Region.TokenSlot.TokenType.Control && t.m_controlToken == ((TokenBase)m_tokenType) && t.m_owner == Region.TokenSlot.Owner.AI) {
						seizedAllTokens = false;
						break;
					}
				}

				if (seizedAllTokens)
				{
//				// goal is met
				m_omegaPlan.GoalCompleted(this);
				GameManager.instance.game.player.RemoveObserver (this);
				}
			}
			break;
		}
	}
}
