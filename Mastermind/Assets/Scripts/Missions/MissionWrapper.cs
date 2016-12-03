using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionWrapper {

	public MissionBase m_mission;
	public Region m_region;
	public Henchmen m_henchmenInFocus;
	public AgentWrapper m_agentInFocus;
	public Region m_regionInFocus;
	public List<Henchmen> m_henchmen = new List<Henchmen>();
	public List<AgentWrapper> m_agents = new List<AgentWrapper>();
	public TokenSlot m_tokenInFocus;
	public Asset m_assetInFocus;
	public Base.Floor m_floorInFocus;
	public Region.HenchmenSlot m_henchmenSlotInFocus;
	public int m_turnsPassed = 0;
	public MissionBase.TargetType m_scope = MissionBase.TargetType.Region;
	public IOrganization m_organization;
	public bool m_success = false;

}
