using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Add Henchmen Trait")]
public class AddHenchmenTrait : MissionBase {

	public TraitData m_newTrait;
	public Asset m_requiredUpgrade;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		int completionChance = CalculateCompletionPercentage (a);

		bool success = WasMissionSuccessful (completionChance);

		if (success) {

			// add trait to henchmen

			a.m_henchmenInFocus.AddTrait (m_newTrait);

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
			t.m_resultsText += "\n" + a.m_henchmenInFocus.henchmenName.ToUpper () + " GAINS " + m_newTrait.m_name.ToUpper () + " TRAIT.";
			t.m_resultsText += "\n" + completionChance.ToString ();
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);


		} else {

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultsText += "\n" + completionChance.ToString ();
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override string GetNameText ()
	{
		string s = m_name + " - ";

		if (GameManager.instance.currentMissionWrapper.m_henchmenInFocus != null) {
			s += GameManager.instance.currentMissionWrapper.m_henchmenInFocus.henchmenName;
		}

		return s;
	}

	public override bool IsValid ()
	{
		// Valid if henchmen in focus doesn't currently have newTrait

		if (m_requiredUpgrade == null || (m_requiredUpgrade != null && GameManager.instance.game.player.currentAssets.Contains(m_requiredUpgrade)) && 
			GameManager.instance.currentMissionWrapper.m_region == GameManager.instance.game.player.homeRegion)
		{
			if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_henchmenInFocus != null)
			{
				if (!GameManager.instance.currentMissionWrapper.m_henchmenInFocus.HasTrait (m_newTrait)) {
					return true;
				} else {
					return false;
				}
			}
		}
		return false;
	}
}
