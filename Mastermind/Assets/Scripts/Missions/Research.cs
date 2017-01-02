using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Base_Research")]
public class Research : MissionBase {

	public Asset m_research;
//	public Asset m_unlockedBaseUpgrade;
	//public Asset m_prerequisiteResearch;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		if (a.m_success) {

			GameManager.instance.game.player.AddResearch (m_research);

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
			t.m_resultsText += "\nResearch Completed: " + m_research.m_name.ToUpper ();

//			if (m_unlockedBaseUpgrade != null) {
//				t.m_resultsText += "\nNew floor type: " + m_unlockedBaseUpgrade.m_name.ToUpper () + " can now be built in your Lair!";
//			}

			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

		} else {

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
		if (!base.IsValid ()) { return false;}		ResearchButton rb = GameManager.instance.currentMissionWrapper.m_researchButtonInFocus;

		if ( rb != null && rb.m_researchGranted == m_research && rb.researchState == ResearchButton.ResearchState.Available)
		{
			return true;
		}

		return false;
	}
}
