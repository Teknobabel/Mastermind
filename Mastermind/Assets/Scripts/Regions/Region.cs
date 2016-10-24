using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class Region : ScriptableObject {

	public class TokenSlot
	{
		public enum State
		{
			None,
			Hidden,
			Revealed,
		}

		public enum TokenType
		{
			None,
			Asset,
			Policy,
			Control,
		}

		public State m_state = State.None;
		public TokenType m_type = TokenType.None;
		public PolicyToken m_policyToken;
		public AssetToken m_assetToken;
		public ControlToken m_controlToken;

		public TokenBase GetBaseToken () {
			switch (m_type) {
			case TokenType.Asset:
				return m_assetToken;
				break;
			case TokenType.Policy:
				return m_policyToken;
				break;
			case TokenType.Control:
				return m_controlToken;
				break;
			}
			return null;
		}
	}

	private string m_regionName = "Null";
	private RegionData.Rank m_rank = RegionData.Rank.None;
	private RegionData.RegionGroup m_regionGroup = RegionData.RegionGroup.None;

	private List<TokenSlot> m_assetTokens;
	private List<TokenSlot> m_policyTokens;
	private List<TokenSlot> m_controlTokens;
	private List<Henchmen> m_currentHenchmen;

	private int 
	m_id = -1,
	m_henchmenSlots = 1;

	public void Initialize (RegionData r)
	{
		m_id = GameManager.instance.newID;
		m_regionName = r.m_name.ToUpper();
		m_rank = r.m_rank;
		m_regionGroup = r.m_regionGroup;
		m_henchmenSlots = r.m_henchmenSlots;

		m_assetTokens = new List<TokenSlot> ();
		m_policyTokens = new List<TokenSlot> ();
		m_controlTokens = new List<TokenSlot> ();
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
		TokenSlot t = new TokenSlot ();
		t.m_type = TokenSlot.TokenType.Asset;
		t.m_assetToken = a;
		t.m_state = TokenSlot.State.Hidden;

		m_assetTokens.Add (t);
	}

	private void AddControlToken (ControlToken c)
	{
		TokenSlot t = new TokenSlot ();
		t.m_type = TokenSlot.TokenType.Control;
		t.m_controlToken = c;
		t.m_state = TokenSlot.State.Hidden;

		m_controlTokens.Add (t);
	}

	public void AddHenchmen (Henchmen h)
	{
		h.SetRegion (this);
		m_currentHenchmen.Add (h);
	} 

	public int id {get{return m_id;}}
	public int henchmenSlots {get{return m_henchmenSlots;}}
	public RegionData.Rank rank {get{return m_rank; }}
	public string regionName {get{return m_regionName; }}
	public RegionData.RegionGroup regionGroup {get{return m_regionGroup; }}
	public List<TokenSlot> assetTokens {get{return m_assetTokens;}}
	public List<TokenSlot> policyTokens {get{return m_policyTokens;}}
	public List<TokenSlot> controlTokens {get{return m_controlTokens;}}
	public List<Henchmen> currentHenchmen {get{return m_currentHenchmen;}}
}
