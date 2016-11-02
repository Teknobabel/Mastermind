using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IMission {

	bool IsValid ();
	void InitializeMission (Organization.ActiveMission a);
	void CompleteMission (Organization.ActiveMission a);
	int CalculateCompletionPercentage (MissionBase m, Region r, List<Henchmen> h);
	bool WasMissionSuccessful (int successChance);
}
