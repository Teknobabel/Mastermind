using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Agent Engage Henchmen")]
public class Agents_EngageHenchmen : MissionBase {

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
