using UnityEngine;
using System.Collections;

public interface IMission {

	bool IsValid ();
	void CompleteMission (Organization.ActiveMission a);
}
