using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Token/Policy - Change All Asset State")]
public class Policy_ChangeAllAssetState : PolicyToken {

	public TokenSlot.Status m_newState = TokenSlot.Status.None;

	public override void StartPolicy (TokenSlot t)
	{
		foreach (TokenSlot ts in t.m_region.assetTokens) {

			ts.m_effects.Add (m_newState);
			t.m_assetTokens.Add (ts);

		}
	}

	public override void EndPolicy (TokenSlot t)
	{
		while (t.m_assetTokens.Count > 0) {

			TokenSlot ts = t.m_assetTokens [0];
			t.m_assetTokens.RemoveAt (0);

			if (ts.m_effects.Contains (m_newState)) {
				ts.m_effects.Remove (m_newState);
			}
		}
	}
}
