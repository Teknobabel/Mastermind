using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MissionWrapper : ICloneable {

	public MissionBase m_mission;
	public Region m_region;
	public Henchmen m_henchmenInFocus;
	public AgentWrapper m_agentInFocus;
	public Region m_regionInFocus;
	public List<Henchmen> m_henchmen = new List<Henchmen>();
	public List<AgentWrapper> m_agents = new List<AgentWrapper>();
	public TokenSlot m_tokenInFocus;
	public Asset m_assetInFocus;
	public Floor m_floorInFocus;
	public ResearchButton m_researchButtonInFocus;
	public Region.HenchmenSlot m_henchmenSlotInFocus;
	public int m_turnsPassed = 0;
	public MissionBase.TargetType m_scope = MissionBase.TargetType.Region;
	public OrganizationBase m_organization;
	public bool m_success = false;

	public object Clone ()
	{
		return new MissionWrapper
		{
			m_mission = this.m_mission,
			m_region = this.m_region,
			m_henchmenInFocus = this.m_henchmenInFocus,
			m_agentInFocus = this.m_agentInFocus,
			m_regionInFocus = this.m_regionInFocus,
			m_henchmen = this.m_henchmen,
			m_agents = this.m_agents,
			m_tokenInFocus = this.m_tokenInFocus,
			m_assetInFocus = this.m_assetInFocus,
			m_floorInFocus = this.m_floorInFocus,
			m_researchButtonInFocus = this.m_researchButtonInFocus,
			m_henchmenSlotInFocus = this.m_henchmenSlotInFocus,
			m_turnsPassed = this.m_turnsPassed,
			m_scope = this.m_scope,
			m_organization = this.m_organization,
			m_success = this.m_success,
		};


	}

}
