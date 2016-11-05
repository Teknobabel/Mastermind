using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Token/Random Asset Token")]
public class RandomAssetToken : AssetToken {

	public AssetToken[] m_assets;

	public AssetToken GetRandomAsset ()
	{
		AssetToken a = null;

		if (m_assets.Length > 0) {
			a = m_assets[Random.Range(0, m_assets.Length)];
		}

		return a;
	}
}
