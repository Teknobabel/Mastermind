using UnityEngine;
using System.Collections;

public interface IGameState {

	void EnterState ();

	void UpdateState ();

	void ExitState ();

}
