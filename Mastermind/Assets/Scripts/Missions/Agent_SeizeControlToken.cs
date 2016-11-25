using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Agent Seize Control Token")]
public class Agent_SeizeControlToken : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		if (a.m_success) {

		} else {

		}
	}

	public override bool IsValid ()
	{
		return false;
	}
}
