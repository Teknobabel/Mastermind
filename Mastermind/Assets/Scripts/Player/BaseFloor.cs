using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class BaseFloor : Asset {

	[System.Serializable]
	public struct BaseFloorMissions
	{
		public int m_level;
		public bool m_extraHenchmen;
		public MissionBase[] m_availableMissions;
	}


	public BaseFloorMissions[] m_missions;
}
