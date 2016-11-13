using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Token/Policy - Change Asset State")]
public class Policy_ChangeAssetState : PolicyToken {

	public Region.TokenSlot.Status m_newState = Region.TokenSlot.Status.None;

	public override void StartPolicy (Region.TokenSlot t)
	{
		List<Region.TokenSlot> validTokens = new List<Region.TokenSlot> ();

		foreach (Region.TokenSlot ts in t.m_region.assetTokens) {

			if (ts.m_assetToken != null) {
				validTokens.Add (ts);
			}
		}

		if (validTokens.Count > 0) {

			int rand = Random.Range (0, validTokens.Count);

			Region.TokenSlot ts = validTokens [rand];

			ts.m_effects.Add (m_newState);

			t.m_assetToken = ts.m_assetToken;
		}
	}

	public override void EndPolicy (Region.TokenSlot t)
	{
		foreach (Region.TokenSlot ts in t.m_region.assetTokens) {

			if (ts.m_assetToken == t.m_assetToken && ts.m_effects.Contains (m_newState)) {

				ts.m_effects.Remove (m_newState);
				break;

			}
		}
	}

}
