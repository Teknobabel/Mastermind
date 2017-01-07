using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class TechTree : ScriptableObject {

	public enum BranchType
	{
		None,
		Influence,
		Tech,
		Force,
		Lair,
	}

	[System.Serializable]
	public struct ResearchBranch
	{
		public string m_branchName;
		public BranchType m_branchType;
		public List<ResearchObject> m_researchObjects;
	}

	public ResearchBranch[] m_branches;
}
