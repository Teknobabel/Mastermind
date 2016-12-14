using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TurnResultsEntry {

	public enum IconType
	{
		None,
		OmegaPlan,
		Mission,
		Henchmen,
		Agent,
		World,
		Travel,
		WantedLevel,
		Organization,
	}

	public string m_resultsText = "Null";
	public int m_turnNumber = -1;
	public GameEvent m_resultType = GameEvent.None;
	public IconType m_iconType = IconType.Mission;
	public List<int> m_henchmenIDs = new List<int>();
}
