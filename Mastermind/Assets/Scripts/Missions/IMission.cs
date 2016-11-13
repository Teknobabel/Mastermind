using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IMission {

	bool IsValid ();
	void InitializeMission (MissionWrapper a);
	void CompleteMission (MissionWrapper a);
	int CalculateCompletionPercentage (MissionWrapper mw);
	bool WasMissionSuccessful (int successChance);
}
