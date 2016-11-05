using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Omega Plan Goal/Use Asset on all Tokens of Type in Region")]
public class Goal_UseAssetOnAllTokenTypeInRegion : OPGoalBase, IObserver {

	public Asset m_asset;
	public Region.TokenSlot.Status m_status;
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

		o.AddObserver(this);

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
		switch (thisGameEvent) {
		case GameEvent.Region_TokenStatusChanged:
			Region r = (Region)subject;
			if (r == m_region) {

				// check if all tokens of type have the correct status
				bool allTokensOfTypeAffected = true;
				foreach (Region.TokenSlot t in r.controlTokens) {

					if (t.m_type == Region.TokenSlot.TokenType.Control && t.m_controlToken == ((TokenBase)m_tokenType) && t.m_status != m_status) {
						allTokensOfTypeAffected = false;
					}
				}
					
				if (allTokensOfTypeAffected) {
					// goal is met
					m_omegaPlan.GoalCompleted (this);
					GameManager.instance.game.player.RemoveObserver (this);
				}
			}
			break;
		}
	}
}
