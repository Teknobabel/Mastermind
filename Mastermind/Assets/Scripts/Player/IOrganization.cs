using UnityEngine;
using System.Collections;

public interface IOrganization {

	void AddTurnResults (int turn, TurnResultsEntry t);
	void Initialize (string orgName);
	void AddMission (MissionWrapper mw);
	void MissionCompleted (MissionWrapper a);
	void CancelMission (MissionWrapper mw);
}
