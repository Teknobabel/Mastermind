using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Token/Policy - Change All Policy State")]
public class Policy_ChangeAllPolicyState : PolicyToken {

	public Region.TokenSlot.Status m_newState = Region.TokenSlot.Status.None;

	public override void StartPolicy (Region.TokenSlot t)
	{
		foreach (Region.TokenSlot ts in t.m_region.policyTokens) {

			ts.m_effects.Add (m_newState);
			t.m_policyTokens.Add (ts);
		}
	}

	public override void EndPolicy (Region.TokenSlot t)
	{
		while (t.m_policyTokens.Count > 0) {

			Region.TokenSlot ts = t.m_policyTokens [0];
			t.m_policyTokens.RemoveAt (0);

			if (ts.m_effects.Contains (m_newState)) {
				ts.m_effects.Remove (m_newState);
			}
		}
	}
}
