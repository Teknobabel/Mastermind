using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class Director : ScriptableObject {

	public OmegaPlanData[] m_omegaPlanBank;

	public int 
	m_maxInfamy = 100,
	m_infamyGainPerTurn = 5,
	m_startingWantedLevel = 0,
	m_maxWantedLevel = 5,
	m_startingOmegaPlans = 3,
	m_startingRevealedOmegaPlans = 1,
	m_startingHenchmen = 3,
	m_maxStartingHenchmenLevel = 1,
	m_startingCommandPool = 10,
	m_startingIntel = 0,
	m_maxIntel = 5,
	m_intelSpawnLowerBounds = 5,
	m_intelSpawnUpperBounds = 20,
	m_maxIntelInWorld = 3,
	m_injuredStatusPenalty = 10,
	m_criticalStatusPenalty = 20;

	public float
	m_agentStayInRegionChance = 0.65f;

	private int
		m_maxTokenSlotsInRegion = 6;

	public Asset[] m_startingAssets;
	public Asset[] m_startingBaseUpgrades;
	public OmegaPlanData[] m_startingOmegaPlanData;
	public HenchmenData[] m_startingHenchmenData;
	public HenchmenData[] m_startingAgentData;

	public EventTriggerBase[] m_events;

	public int maxTokenSlotsInRegion {get{return m_maxTokenSlotsInRegion;}}
}
