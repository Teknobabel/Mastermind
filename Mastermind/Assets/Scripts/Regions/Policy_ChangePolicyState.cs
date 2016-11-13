using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Token/Policy - Change Policy State")]
public class Policy_ChangePolicyState : PolicyToken {

	public Region.TokenSlot.Status m_newState = Region.TokenSlot.Status.None;

	public override void StartPolicy (Region.TokenSlot t)
	{
		List<Region.TokenSlot> validTokens = new List<Region.TokenSlot> ();

		foreach (Region.TokenSlot ts in t.m_region.policyTokens) {

//			if (ts.m_policyToken != null) {
				validTokens.Add (ts);
//			}
		}

		if (validTokens.Count > 0) {

			int rand = Random.Range (0, validTokens.Count);

			Region.TokenSlot ts = validTokens [rand];

			ts.m_effects.Add (m_newState);

			t.m_policyToken = ts.m_policyToken;
		}
	}

	public override void EndPolicy (Region.TokenSlot t)
	{
		foreach (Region.TokenSlot ts in t.m_region.policyTokens) {

			if (ts.m_policyToken == t.m_policyToken && ts.m_effects.Contains (m_newState)) {

				ts.m_effects.Remove (m_newState);
				break;

			}
		}
	}
}
