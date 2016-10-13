using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Region_ListViewItem : MonoBehaviour {

	public Text m_regionName;
	public Text m_regionRank;
	public Image m_regionPortrait;

	public TokenButton[] m_policyTokens;
	public TokenButton[] m_assetTokens;
	public TokenButton[] m_controlTokens;

	public TokenButton[] m_henchmenTokens;

	// Use this for initialization
	void Start () {
	
	}

	public void Initialize (Region r)
	{
		m_regionName.text = r.regionName;

		List<PolicyToken> p = r.policyTokens;
		List<AssetToken> a = r.assetTokens;
		List<ControlToken> c = r.controlTokens;

		for (int i = 0; i < m_policyTokens.Length; i++) {
			TokenButton tB = m_policyTokens [i];
			if (i < p.Count) {
				PolicyToken pT = p[i];
				tB.Initialize(pT);
			} else {
				tB.Deactivate ();
			}
		}

		for (int i = 0; i < m_assetTokens.Length; i++) {
			TokenButton tB = m_assetTokens [i];
			if (i < a.Count) {
				AssetToken aT = a[i];
				tB.Initialize(aT);
			} else {
				tB.Deactivate ();
			}
		}

		for (int i = 0; i < m_controlTokens.Length; i++) {
			TokenButton tB = m_controlTokens [i];
			if (i < c.Count) {
				ControlToken cT = c[i];
				tB.Initialize(cT);
			} else {
				tB.Deactivate ();
			}
		}

		for (int i = 0; i < m_henchmenTokens.Length; i++) {
			TokenButton tB = m_henchmenTokens [i];
			if (i < r.henchmenSlots) {

			} else {
				tB.Deactivate ();
			}
		}

		string s = "Null";
		switch (r.rank) {
		case RegionData.Rank.One:
			s = "1";
			break;
		case RegionData.Rank.Two:
			s = "2";
			break;
		case RegionData.Rank.Three:
			s = "3";
			break;
		}

		m_regionRank.text = "R" + s;

	}

}
