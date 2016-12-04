using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Threaten The World")]
public class ThreatenTheWorld : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();
		t.m_resultsText = "You threaten the world with total annihilation!";
		t.m_resultsText += "The world submits to your authority!";
		t.m_resultsText += "The game is over, you win!";
		t.m_resultType = GameEvent.GameState_GameOver;
		GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

	}

	public override bool IsValid ()
	{
		// valid if all other omega plan goals are complete

		if (GameManager.instance.currentMissionWrapper.m_region.id == GameManager.instance.game.player.homeRegion.id) {
			
			foreach (OmegaPlan op in GameManager.instance.game.player.omegaPlans) {

				int goalsCompleted = 0;

				foreach (OmegaPlan.Goal opGoal in op.goals) {

					if (opGoal.m_state == OmegaPlan.Goal.State.Completed) {

						goalsCompleted++;
					}
				}

				if (goalsCompleted == op.goals.Count - 1) {

					return true;
				}
			}
		}

		return false;
	}
}
