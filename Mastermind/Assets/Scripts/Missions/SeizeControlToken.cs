using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Seize Control Token")]
public class SeizeControlToken : MissionBase {

	public ControlToken.ControlType m_type = ControlToken.ControlType.None;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		a.m_success = true;

		if (a.m_success) {

			// find the first control token of m_type not under player control

			foreach (TokenSlot ts in a.m_region.controlTokens) {

				if (ts.m_controlToken.m_controlType == m_type && ts.owner != Region.Owner.Player && ts.m_state == TokenSlot.State.Revealed) {

					// change owner to player

					ts.ChangeOwner(Region.Owner.Player);

					TurnResultsEntry t = new TurnResultsEntry ();
					t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
					t.m_resultsText += "\n" + ts.GetBaseToken().m_name.ToUpper() + " is now under " + GameManager.instance.game.player.orgName.ToUpper() + " control!";
//					t.m_resultsText += "\n" + completionChance.ToString ();
					t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
					t.m_resultType = GameEvent.Henchmen_MissionCompleted;
					GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

					break;
				}

			}


		} else {

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
//			t.m_resultsText += "\n" + completionChance.ToString ();
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
		// valid if there is a control token of m_type not under player control

		if (GameManager.instance.currentMissionWrapper.m_tokenInFocus != null)
		{
			TokenSlot t = GameManager.instance.currentMissionWrapper.m_tokenInFocus;

			if (t.m_state == TokenSlot.State.Revealed && t.m_type == TokenSlot.TokenType.Control && t.m_controlToken.m_controlType == m_type && t.owner != Region.Owner.Player) {
				return true;
			}
		}
		return false;
	}

	public override string GetNameText ()
	{
		string s = m_name + "\n";

		s += "<size=18>" + m_type.ToString().ToUpper() + "</size>";


		return s;
	}
}
