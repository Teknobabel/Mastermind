using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TokenSlot : ISubject {

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
		Infected,
		AIControl,
		Protected,
		Vulnerable,
	}
		
	public State m_state = State.None;
	public TokenType m_type = TokenType.None;
	public PolicyToken m_policyToken;
	public AssetToken m_assetToken;
	public ControlToken m_controlToken;
	public List<TokenSlot> m_assetTokens = new List<TokenSlot> ();
	public List<TokenSlot> m_controlTokens = new List<TokenSlot>();
	public List<TokenSlot> m_policyTokens = new List<TokenSlot>();
	public List<Status> m_effects = new List<Status>();
	private Region.Owner m_owner = Region.Owner.AI;
	public Region m_region = null;

	private List<IObserver>
	m_observers = new List<IObserver> ();

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

	public void ChangeOwner (Region.Owner newOwner)
	{
		m_owner = newOwner;
		Notify (this, GameEvent.Region_ControlTokenOwnerChanged);
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

	public Region.Owner owner {get{return m_owner; }}
}
