using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Token/Random Control Token")]
public class RandomControlToken : ControlToken {

	public ControlToken[] m_controlTokens;

	public ControlToken GetRandomToken ()
	{
		ControlToken c = null;

		if (m_controlTokens.Length > 0) {
			c = m_controlTokens[Random.Range(0, m_controlTokens.Length)];
		}

		return c;
	}
}
