using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Token/Policy - Change Asset State")]
public class Policy_ChangeAssetState : PolicyToken {

	public TokenSlot.Status m_newState = TokenSlot.Status.None;

	public override void StartPolicy (TokenSlot t)
	{
		List<TokenSlot> validTokens = new List<TokenSlot> ();

		foreach (TokenSlot ts in t.m_region.assetTokens) {

//			if (ts.m_assetToken != null) {
				validTokens.Add (ts);
//			}
		}

		if (validTokens.Count > 0) {

			int rand = Random.Range (0, validTokens.Count);

			TokenSlot ts = validTokens [rand];

			ts.m_effects.Add (m_newState);

			t.m_assetToken = ts.m_assetToken;
		}
	}

	public override void EndPolicy (TokenSlot t)
	{
		foreach (TokenSlot ts in t.m_region.assetTokens) {

			if (ts.m_assetToken == t.m_assetToken && ts.m_effects.Contains (m_newState)) {

				ts.m_effects.Remove (m_newState);
				t.m_assetToken = null;
				break;

			}
		}
	}

}
