using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor {

	public int m_floorNumber = -1;
	public Asset m_installedUpgrade = null;
	public Base.FloorState m_floorState = Base.FloorState.None;
	public List<Henchmen> m_guards = new List<Henchmen> ();
	public int m_baseDefense = 0;

	public Region.HenchmenSlot m_henchmenSlot;
	public Region m_region;

	// dependant on type of installed upgrade

	public AgentWrapper m_capturedAgent = null;

	public void Initialize ()
	{
		m_region = Region.CreateInstance<Region> ();
		m_region.regionName = "FLOOR " + m_floorNumber.ToString ();
		m_region.id = GameManager.instance.newID;
		m_region.henchmenSlots = new List<Region.HenchmenSlot> ();
		m_region.currentHenchmen = new List<Henchmen> ();

		m_henchmenSlot = new Region.HenchmenSlot ();
		m_henchmenSlot.m_state = Region.HenchmenSlot.State.Empty;
		m_henchmenSlot.m_id = GameManager.instance.newID;

		m_region.henchmenSlots.Add (m_henchmenSlot);
	}
}
