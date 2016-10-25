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
	m_maxIntel = 5;

	public Asset[] m_startingAssets;
	public OmegaPlanData[] m_startingOmegaPlanData;

	// TODO: starting henchmen

	// TODO: starting player upgrades

}
