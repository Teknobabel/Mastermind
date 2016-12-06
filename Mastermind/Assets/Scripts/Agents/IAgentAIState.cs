using UnityEngine;
using System.Collections;

public interface IAgentAIState {

	void DoAgentTurn (AgentWrapper aw);
	void EnterState (AgentWrapper aw);
	void ExitState (AgentWrapper aw);
}
