using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Seize Control Token")]
public class SeizeControlToken : MissionBase {

	public ControlToken.ControlType m_type = ControlToken.ControlType.None;

	public override void CompleteMission (Organization.ActiveMission a)
	{
		base.CompleteMission (a);

		int completionChance = CalculateCompletionPercentage (a.m_mission, a.m_region, a.m_henchmen);

		bool missionSuccess = WasMissionSuccessful (completionChance);

		if (missionSuccess) {

			// find the first control token of m_type not under player control

			foreach (Region.TokenSlot ts in a.m_region.controlTokens) {

				if (ts.m_controlToken.m_controlType == m_type && ts.m_owner != Region.TokenSlot.Owner.Player && ts.m_state == Region.TokenSlot.State.Revealed) {

					// change owner to player

					ts.m_owner = Region.TokenSlot.Owner.Player;

					TurnResultsEntry t = new TurnResultsEntry ();
					t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
					t.m_resultsText += "\n" + ts.GetBaseToken().m_name.ToUpper() + " is now under " + GameManager.instance.game.player.orgName.ToUpper() + " control!";
					t.m_resultsText += "\n" + completionChance.ToString ();
					t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
					t.m_resultType = GameEvent.Henchmen_MissionCompleted;
					GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

					break;
				}

			}


		} else {

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultsText += "\n" + completionChance.ToString ();
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
		// valid if there is a control token of m_type not under player control

		foreach (Region.TokenSlot t in GameManager.instance.currentMissionRequest.m_region.controlTokens) {

			if (t.m_controlToken.m_controlType == m_type && t.m_owner != Region.TokenSlot.Owner.Player && t.m_state == Region.TokenSlot.State.Revealed) {

				return true;
			}

		}
		return false;
	}
}
