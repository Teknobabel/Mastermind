using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class Region : ScriptableObject {

	private string m_regionName = "Null";
	private RegionData.Rank m_rank = RegionData.Rank.None;
	private RegionData.RegionGroup m_regionGroup = RegionData.RegionGroup.None;

	private List<AssetToken> m_assetTokens;
	private List<PolicyToken> m_policyTokens;
	private List<ControlToken> m_controlTokens;
	private List<Henchmen> m_currentHenchmen;

	private int m_id = -1,
	m_henchmenSlots = 1;

	public void Initialize (RegionData r)
	{
		m_id = GameManager.instance.newID;
		m_regionName = r.m_name.ToUpper();
		m_rank = r.m_rank;
		m_regionGroup = r.m_regionGroup;
		m_henchmenSlots = r.m_henchmenSlots;

		m_assetTokens = new List<AssetToken> ();
		m_policyTokens = new List<PolicyToken> ();
		m_controlTokens = new List<ControlToken> ();
		m_currentHenchmen = new List<Henchmen> ();

		foreach (AssetToken a in r.m_assetTokens) {

			if (a is RandomAssetToken) {
				AssetToken aT = ((RandomAssetToken)a).GetRandomAsset ();
				AddAssetToken (aT);
			} else {
				AddAssetToken (a);
			}
		}

		foreach (ControlToken c in r.m_controlTokens) {

			if (c is RandomControlToken) {
				ControlToken cT = ((RandomControlToken)c).GetRandomToken ();
				AddControlToken (cT);
			} else {
				AddControlToken (c);
			}
		}
	}

	private void AddAssetToken (AssetToken a)
	{
		a.ChangeState (TokenBase.State.Hidden);
		m_assetTokens.Add (a);
	}

	private void AddControlToken (ControlToken c)
	{
		c.ChangeState (TokenBase.State.Hidden);
		m_controlTokens.Add (c);
	}

	public int id {get{return m_id;}}
	public int henchmenSlots {get{return m_henchmenSlots;}}
	public RegionData.Rank rank {get{return m_rank; }}
	public string regionName {get{return m_regionName; }}
	public RegionData.RegionGroup regionGroup {get{return m_regionGroup; }}
	public List<AssetToken> assetTokens {get{return m_assetTokens;}}
	public List<PolicyToken> policyTokens {get{return m_policyTokens;}}
	public List<ControlToken> controlTokens {get{return m_controlTokens;}}
}
