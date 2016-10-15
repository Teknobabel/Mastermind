using UnityEngine;
using System.Collections;

public class MissionBase : ScriptableObject {
	
	public string m_name = "Null";
	public int m_numTurns = 1;
	public int m_maxRank = 5;
	public TraitData[] m_rank1Skills;
}
