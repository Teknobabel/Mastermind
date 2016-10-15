using UnityEngine;
using System.Collections;

public interface IMission {

	bool IsValid ();
	void ProcessTurn ();
}
