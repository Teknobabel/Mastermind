using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Agent Invade Base")]
public class Agent_InvadeBase : MissionBase {

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
