using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class ResearchObject : ScriptableObject {

	public string m_name = "Null";
	public MissionBase m_researchMission;
	public List<Asset> m_prerequisiteResearch = new List<Asset>();
	public List<Asset> m_researchGained = new List<Asset>();
}
