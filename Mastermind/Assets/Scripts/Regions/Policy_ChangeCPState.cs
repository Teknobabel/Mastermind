using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Token/Policy - Change CP State")]
public class Policy_ChangeCPState : PolicyToken {

	public TokenSlot.Status m_newState = TokenSlot.Status.None;

	public override void StartPolicy (TokenSlot t)
	{
		List<TokenSlot> validTokens = new List<TokenSlot> ();

		foreach (TokenSlot ts in t.m_region.controlTokens) {

//			if (ts.m_controlToken != null) {
				validTokens.Add (ts);
//			}
		}

		if (validTokens.Count > 0) {

			int rand = Random.Range (0, validTokens.Count);

			TokenSlot ts = validTokens [rand];

			ts.m_effects.Add (m_newState);

			t.m_controlToken = ts.m_controlToken;
		}
	}

	public override void EndPolicy (TokenSlot t)
	{
		foreach (TokenSlot ts in t.m_region.controlTokens) {

			if (ts.m_controlToken == t.m_controlToken && ts.m_effects.Contains (m_newState)) {

				ts.m_effects.Remove (m_newState);
				t.m_controlToken = null;
				break;

			}
		}
	}
}
