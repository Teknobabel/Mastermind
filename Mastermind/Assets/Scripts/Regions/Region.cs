using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class Region : ScriptableObject, ISubject {

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

		public enum Status
		{
			None,
			Normal,
			MindControlled,
			Zombified,
			Destroyed,
		}

		public enum Owner
		{
			AI,
			Player,
		}

		public State m_state = State.None;
		public TokenType m_type = TokenType.None;
		public PolicyToken m_policyToken;
		public AssetToken m_assetToken;
		public ControlToken m_controlToken;
		public Status m_status = Status.Normal;
		public Owner m_owner = Owner.AI;

		public TokenBase GetBaseToken () {
			switch (m_type) {
			case TokenType.Asset:
				return m_assetToken;
			case TokenType.Policy:
				return m_policyToken;
			case TokenType.Control:
				return m_controlToken;
			}
			return null;
		}
	}

	public class HenchmenSlot
	{
		public enum State
		{
			None,
			Empty,
			Reserved,
			Occupied,
		}

		public int m_id = -1;
		public State m_state = State.None;
		public Henchmen m_henchmen = null;
	}

	private string m_regionName = "Null";
	private RegionData.Rank m_rank = RegionData.Rank.None;
	private Sprite m_portrait;
	private RegionData.RegionGroup m_regionGroup = RegionData.RegionGroup.None;

	private List<TokenSlot> m_assetTokens;
	private List<TokenSlot> m_policyTokens;
	private List<TokenSlot> m_controlTokens;
	private List<Henchmen> m_currentHenchmen;
	private List<HenchmenSlot> m_henchmenSlots;

	private int 
	m_id = -1,
	m_numHenchmenSlots = 1;

	private List<IObserver>
	m_observers = new List<IObserver> ();

	public void Initialize (RegionData r)
	{
		m_id = GameManager.instance.newID;
		m_regionName = r.m_name.ToUpper();
		m_rank = r.m_rank;
		m_regionGroup = r.m_regionGroup;
		m_numHenchmenSlots = r.m_henchmenSlots;
		m_portrait = r.m_portrait;

		m_assetTokens = new List<TokenSlot> ();
		m_policyTokens = new List<TokenSlot> ();
		m_controlTokens = new List<TokenSlot> ();
		m_currentHenchmen = new List<Henchmen> ();
		m_henchmenSlots = new List<HenchmenSlot> ();

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

		for (int i = 0; i < m_numHenchmenSlots; i++) {
			HenchmenSlot s = new HenchmenSlot ();
			s.m_state = HenchmenSlot.State.Empty;
			s.m_id = GameManager.instance.newID;
			m_henchmenSlots.Add (s);
		}
	}

	public void AddAssetToken (AssetToken a)
	{
		bool tokenAdded = false;

		foreach (TokenSlot ts in m_assetTokens) {

			if (ts.m_state == TokenSlot.State.None) {

				ts.m_assetToken = a;
				ts.m_state = TokenSlot.State.Revealed;
				ts.m_status = TokenSlot.Status.Normal;
				tokenAdded = true;
				break;
			}
		}

		if (!tokenAdded) {
			TokenSlot t = new TokenSlot ();
			t.m_type = TokenSlot.TokenType.Asset;
			t.m_assetToken = a;
			t.m_state = TokenSlot.State.Revealed;
			t.m_status = TokenSlot.Status.Normal;

			m_assetTokens.Add (t);
		}
	}

	public void RemoveAssetToken (TokenSlot ts)
	{
		ts.m_state = TokenSlot.State.None;
		ts.m_assetToken = null;
		ts.m_status = TokenSlot.Status.None;
	}

	private void AddControlToken (ControlToken c)
	{
		TokenSlot t = new TokenSlot ();
		t.m_type = TokenSlot.TokenType.Control;
		t.m_controlToken = c;
		t.m_state = TokenSlot.State.Hidden;

		m_controlTokens.Add (t);
	}

	public void ReserveSlot (Henchmen h)
	{
		foreach (HenchmenSlot s in m_henchmenSlots) {
			if (s.m_state == HenchmenSlot.State.Empty) {
				s.m_state = HenchmenSlot.State.Reserved;
				s.m_henchmen = h;
				break;
			}
		}
	}

	private HenchmenSlot HasReservation (Henchmen h)
	{
		foreach (HenchmenSlot s in m_henchmenSlots) {
			if (s.m_state == HenchmenSlot.State.Reserved && s.m_henchmen != null && s.m_henchmen.id == h.id) {
				return s;
			}
		}

		return null;
	}

	public void RemoveHenchmen (Henchmen h)
	{
		foreach (HenchmenSlot s in m_henchmenSlots) {
			if (s.m_state == HenchmenSlot.State.Occupied && s.m_henchmen != null && s.m_henchmen == h) {
				s.m_state = HenchmenSlot.State.Empty;
				s.m_henchmen = null;

				break;
			}
		}
	}

	public void AddHenchmen (Henchmen h)
	{
		HenchmenSlot r = HasReservation (h);

		if (r != null) {
			r.m_state = HenchmenSlot.State.Occupied;
			r.m_henchmen = h;
		
		} else {
			bool henchmenPlaced = false;

			foreach (HenchmenSlot s in m_henchmenSlots) {
				if (s.m_state == HenchmenSlot.State.Empty) {
					s.m_state = HenchmenSlot.State.Occupied;
					s.m_henchmen = h;
					henchmenPlaced = true;
					break;
				}
			}

			if (!henchmenPlaced) {
				Debug.Log (m_henchmenSlots.Count);
				Debug.Log ("<color=red>No Henchmen Slot Found</color>");
			}
		}

		m_currentHenchmen.Add (h);
		h.SetRegion (this);
	} 

	public void AddObserver (IObserver observer)
	{
		if (!m_observers.Contains(observer))
		{
			m_observers.Add (observer);
		}
	}

	public void RemoveObserver (IObserver observer)
	{
		if (m_observers.Contains(observer))
		{
			m_observers.Remove(observer);
		}
	}

	public void Notify (ISubject subject, GameEvent thisGameEvent)
	{
		List<IObserver> observers = new List<IObserver> (m_observers);

		for (int i=0; i < observers.Count; i++)
		{
			observers[i].OnNotify(subject, thisGameEvent);
		}
	}

	public int id {get{return m_id;}}
	public int numHenchmenSlots {get{return m_numHenchmenSlots;}}
	public RegionData.Rank rank {get{return m_rank; }}
	public string regionName {get{return m_regionName; }}
	public RegionData.RegionGroup regionGroup {get{return m_regionGroup; }}
	public List<TokenSlot> assetTokens {get{return m_assetTokens;}}
	public List<TokenSlot> policyTokens {get{return m_policyTokens;}}
	public List<TokenSlot> controlTokens {get{return m_controlTokens;}}
	public List<Henchmen> currentHenchmen {get{return m_currentHenchmen;}}
	public List<HenchmenSlot> henchmenSlots {get{return m_henchmenSlots;}}
	public Sprite portrait {get{return m_portrait;}}
}
