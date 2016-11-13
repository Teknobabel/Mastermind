using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Token/Random Policy Token")]
public class RandomPolicyToken : PolicyToken {

	public float m_emptyChance = 0.5f;

	public PolicyToken[] m_policies;

	public PolicyToken GetRandomPolicy ()
	{
		PolicyToken p = null;

		if (m_policies.Length > 0) {
			p = m_policies[Random.Range(0, m_policies.Length)];
		}

		return p;
	}
}
