using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class Director : ScriptableObject {

	public OmegaPlanData[] m_omegaPlanBank;

	public int 
	m_maxInfamy = 100,
	m_startingWantedLevel = 0,
	m_startingOmegaPlans = 3,
	m_startingRevealedOmegaPlans = 1,
	m_startingHenchmen = 3,
	m_maxStartingHenchmenLevel = 1,
	m_startingCommandPool = 10;

}
