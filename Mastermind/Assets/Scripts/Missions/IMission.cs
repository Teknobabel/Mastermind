using UnityEngine;
using System.Collections;

public interface IMission {

	bool IsValid ();
	void InitializeMission (Organization.ActiveMission a);
	void CompleteMission (Organization.ActiveMission a);
}
