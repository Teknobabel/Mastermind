using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Reveal Remote Tokens")]
public class RevealRemoteTokens : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		if (a.m_success) {

		} else {

			//			TurnResultsEntry t = new TurnResultsEntry ();
			//			t.m_resultsText = a.m_mission.m_name.ToUpper () + ": " + a.m_region.regionName.ToUpper() + " mission complete!";
			//			t.m_resultsText += "\n Nothing found.";
			//			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			//			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
		return false;
	}
}
