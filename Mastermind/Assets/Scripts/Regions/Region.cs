using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class Region : ScriptableObject, ISubject, IObserver {
	
	public enum Owner
	{
		AI,
		Player,
	}

	public class HenchmenSlot
	{
		public enum State
		{
			None,
			Empty,
			Reserved,
			Occupied,
			Occupied_Player,
			Occupied_Agent,
		}

		public int m_id = -1;
		public State m_state = State.None;
		public Henchmen m_henchmen = null;
		public AgentWrapper m_agent = null;

		public List<Henchmen> m_enRoute = new List<Henchmen> ();

		public void AddHenchmen (Henchmen h)
		{
			if (m_enRoute.Contains (h)) {

				m_enRoute.Remove (h);
			}

			m_henchmen = h;
			m_state = State.Occupied_Player;

		}

		public void RemoveHenchmen ()
		{
			m_henchmen = null;
			m_state = State.Empty;
		}

		public void AddAgent (AgentWrapper aw)
		{
			if (m_enRoute.Contains (aw.m_agent)) {

				m_enRoute.Remove (aw.m_agent);
			}

			m_agent = aw;
			m_state = State.Occupied_Agent;

		}

		public void RemoveAgent ()
		{
			m_agent = null;
			m_state = State.Empty;
		}
	}

	private string m_regionName = "Null";
	private int m_rank = 0;
	private Sprite m_portrait;
	private RegionData.RegionGroup m_regionGroup = RegionData.RegionGroup.None;
	private Owner m_owner = Owner.AI;

	private List<TokenSlot> m_assetTokens;
	private List<TokenSlot> m_policyTokens;
	private List<TokenSlot> m_controlTokens;
	private List<TokenSlot> m_allTokens;
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
		m_allTokens = new List<TokenSlot> ();

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


		// pre populate policy tokens since policies can effect empty slots

		foreach (PolicyToken p in r.m_policyTokens) {

			AddPolicyToken (null);
		}

		foreach (PolicyToken p in r.m_policyTokens) {

			if (p is RandomPolicyToken) {
				
				if (Random.Range (0.0f, 1.0f) > ((RandomPolicyToken)p).m_emptyChance) {
					
					PolicyToken pt = ((RandomPolicyToken)p).GetRandomPolicy ();
					AddPolicyToken (pt);

				} 
			} else {
				AddPolicyToken (p);
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
				ts.m_state = TokenSlot.State.Hidden;
				tokenAdded = true;
				break;
			}
		}

		if (!tokenAdded) {
			TokenSlot t = new TokenSlot ();
			t.m_type = TokenSlot.TokenType.Asset;
			t.m_assetToken = a;
			t.m_state = TokenSlot.State.Hidden;
			t.m_region = this;

			m_assetTokens.Add (t);
			m_allTokens.Add (t);
		}
	}

	public void RemoveAssetToken (TokenSlot ts)
	{
		ts.m_state = TokenSlot.State.None;
		ts.m_assetToken = null;
		ts.m_effects.Clear();
	}

	private void AddControlToken (ControlToken c)
	{
		TokenSlot t = new TokenSlot ();

		t.m_type = TokenSlot.TokenType.Control;
		t.m_controlToken = c;
		t.m_state = TokenSlot.State.Hidden;
		t.m_region = this;
		t.AddObserver (this);
//		t.ChangeOwner(Owner.Player); // debug
		m_controlTokens.Add (t);
		m_allTokens.Add (t);
	}

	public void AddPolicytoken (PolicyToken p, TokenSlot t)
	{
//		Debug.Log ("Adding Policy Token For: " + m_regionName + " : " + p.m_name);
		if (t.m_state == TokenSlot.State.Revealed && t.m_policyToken == null) {

			t.m_policyToken = p;

			p.StartPolicy (t);
		}
	}

	private void AddPolicyToken (PolicyToken p)
	{
//		Debug.Log ("Adding Policy Token For: " + m_regionName + " : " + p);
		bool tokenAdded = false;

		foreach (TokenSlot ts in m_policyTokens) {
			Debug.Log (ts.m_state);
			if (ts.m_policyToken == null && p != null) {

				ts.m_policyToken = p;

//				ts.m_status = TokenSlot.Status.Normal;

				p.StartPolicy (ts);
//				ts.m_state = TokenSlot.State.Revealed;

				tokenAdded = true;
				break;
			}
		}

		if (!tokenAdded) {
			TokenSlot t = new TokenSlot ();
			t.m_type = TokenSlot.TokenType.Policy;
			t.m_policyToken = p;

			t.m_region = this;

			if (p != null) {
				p.StartPolicy (t);
			} 

			t.m_state = TokenSlot.State.Hidden;

			m_policyTokens.Add (t);
			m_allTokens.Add (t);
		}
	}

	public void RemovePolicyToken (TokenSlot ts)
	{
		ts.m_policyToken.EndPolicy (ts);

		ts.m_state = TokenSlot.State.None;
		ts.m_policyToken = null;
		ts.m_effects.Clear();
	}

	public void ReserveSlot (Henchmen h)
	{
		foreach (HenchmenSlot s in m_henchmenSlots) {
			
			if (s.m_state == HenchmenSlot.State.Empty) {
				
//				s.m_state = HenchmenSlot.State.Reserved;
//				s.m_henchmen = h;

				s.m_enRoute.Add (h);
				break;
			}
		}
	}

	public void ReserveSlot (Henchmen h, HenchmenSlot hs)
	{
		foreach (HenchmenSlot s in m_henchmenSlots) {
			
			if (s.m_id == hs.m_id) {
				
//				s.m_state = HenchmenSlot.State.Reserved;
//				s.m_henchmen = h;

				s.m_enRoute.Add (h);
				break;
			}
		}
	}

	private HenchmenSlot HasReservation (Henchmen h)
	{
		foreach (HenchmenSlot s in m_henchmenSlots) {
			if (s.m_enRoute.Count > 0 && s.m_enRoute.Contains(h)) {
				return s;
			}
		}

		return null;
	}

	public void RemoveAgent (Henchmen a)
	{

		foreach (HenchmenSlot s in m_henchmenSlots) {

			if (s.m_state == HenchmenSlot.State.Occupied_Agent && s.m_agent != null && s.m_agent.m_agent.id == a.id) {

				// remove from any missions

				GameManager.instance.game.agentOrganization.RemoveAgentFromMissions (s.m_agent);

				// clear location based events

				s.m_agent.m_agentEvents.Clear ();

				// remove from region

				s.RemoveAgent ();

				break;
			}
		}

		if (m_currentHenchmen.Contains (a)) {
			m_currentHenchmen.Remove (a);
		}
	}

	public void RemoveHenchmen (Henchmen h)
	{

		foreach (HenchmenSlot s in m_henchmenSlots) {

			if (s.m_state == HenchmenSlot.State.Occupied_Player && s.m_henchmen != null && s.m_henchmen.id == h.id) {

				// remove from any missions

				GameManager.instance.game.player.RemoveHenchmenFromMissions (s.m_henchmen);

				// remove from region

				s.RemoveHenchmen ();
				break;
			}
		}

		if (m_currentHenchmen.Contains (h)) {
			m_currentHenchmen.Remove (h);
		}
	}

	public void AddAgent (AgentWrapper aw)
	{
		HenchmenSlot r = HasReservation (aw.m_agent);

		if (r != null) {
			r.AddAgent (aw);

		} else {
			bool henchmenPlaced = false;

			foreach (HenchmenSlot s in m_henchmenSlots) {
				
				if (s.m_state == HenchmenSlot.State.Empty) {
					s.AddAgent (aw);
					henchmenPlaced = true;
					break;
				}
			}

			if (!henchmenPlaced) {
				Debug.Log (m_henchmenSlots.Count);
				Debug.Log ("<color=red>No Henchmen Slot Found</color>");
			}
		}

		m_currentHenchmen.Add (aw.m_agent);
		aw.m_agent.SetRegion (this);
	}

	public void AddHenchmen (Henchmen h)
	{
		HenchmenSlot r = HasReservation (h);

		if (r != null) {
			r.AddHenchmen(h);
		
		} else {
			bool henchmenPlaced = false;

			foreach (HenchmenSlot s in m_henchmenSlots) {
				if (s.m_state == HenchmenSlot.State.Empty) {
					s.AddHenchmen (h);
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

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {
		case GameEvent.Region_ControlTokenOwnerChanged:

			TokenSlot ts = (TokenSlot)subject;

			if (ts != null && ts.m_region != null) {

				bool playerOwned = true;

				foreach (TokenSlot thisTS in ts.m_region.controlTokens) {

					if (thisTS.owner == Owner.AI) {
						playerOwned = false;
						break;
					}
				}

				if (playerOwned && m_owner != Owner.Player) {

					m_owner = Owner.Player;

					TurnResultsEntry t = new TurnResultsEntry ();
					t.m_resultsText = GameManager.instance.game.player.orgName.ToUpper () + " gains control of " + m_regionName.ToUpper() + "!";
					t.m_resultsText += "\n" + GameManager.instance.game.player.orgName.ToUpper () + " gains " + m_rank.ToString() + " Command Pool!";
					t.m_resultType = GameEvent.Region_OwnerChanged;
					GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

				} else if (!playerOwned && m_owner == Owner.Player) {

					m_owner = Owner.AI;

					TurnResultsEntry t = new TurnResultsEntry ();
					t.m_resultsText = GameManager.instance.game.player.orgName.ToUpper () + " loses control of " + m_regionName.ToUpper() + "!";
					t.m_resultsText += "\n" + GameManager.instance.game.player.orgName.ToUpper () + " loses " + m_rank.ToString() + " Command Pool!";
					t.m_resultType = GameEvent.Region_OwnerChanged;
					GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
				}
			}

			break;
		}
	}

	public int id {get{return m_id;}}
	public int numHenchmenSlots {get{return m_numHenchmenSlots;}}
	public int rank {get{return m_rank; }}
	public string regionName {get{return m_regionName; }}
	public RegionData.RegionGroup regionGroup {get{return m_regionGroup; }}
	public List<TokenSlot> assetTokens {get{return m_assetTokens;}}
	public List<TokenSlot> policyTokens {get{return m_policyTokens;}}
	public List<TokenSlot> controlTokens {get{return m_controlTokens;}}
	public List<TokenSlot> allTokens {get{return m_allTokens;}}
	public List<Henchmen> currentHenchmen {get{return m_currentHenchmen;}}
	public List<HenchmenSlot> henchmenSlots {get{return m_henchmenSlots;}}
	public Sprite portrait {get{return m_portrait;}}
	public Owner owner {get{return m_owner; }}
}
