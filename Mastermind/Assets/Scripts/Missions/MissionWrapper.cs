using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionWrapper {

	public MissionBase m_mission;
	public Region m_region;
	public Henchmen m_henchmenInFocus;
	public List<Henchmen> m_henchmen = new List<Henchmen>();
	public TokenSlot m_tokenInFocus;
	public Base.Floor m_floorInFocus;
	public int m_turnsPassed = 0;
	public MissionBase.TargetType m_scope = MissionBase.TargetType.Region;
	public bool m_success = false;

}
