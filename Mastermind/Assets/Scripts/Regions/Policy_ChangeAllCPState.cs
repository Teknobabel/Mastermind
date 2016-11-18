using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Token/Policy - Change All CP State")]
public class Policy_ChangeAllCPState : PolicyToken {

	public TokenSlot.Status m_newState = TokenSlot.Status.None;

	public override void StartPolicy (TokenSlot t)
	{
		foreach (TokenSlot ts in t.m_region.controlTokens) {

			ts.m_effects.Add (m_newState);
			t.m_controlTokens.Add (ts);

		}
	}

	public override void EndPolicy (TokenSlot t)
	{
		while (t.m_controlTokens.Count > 0) {

			TokenSlot ts = t.m_controlTokens [0];
			t.m_controlTokens.RemoveAt (0);

			if (ts.m_effects.Contains (m_newState)) {
				ts.m_effects.Remove (m_newState);
			}
		}
	}
}
