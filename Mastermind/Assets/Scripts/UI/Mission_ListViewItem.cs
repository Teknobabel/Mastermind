using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Mission_ListViewItem : MonoBehaviour {

	public TextMeshProUGUI m_missionName;
	public TextMeshProUGUI m_missionDescription;
	public TextMeshProUGUI m_missionCost;
	public TextMeshProUGUI m_missionDuration;
	public TextMeshProUGUI m_missionSuccessChance;
	public Image m_missionPortrait;

	public TraitButton[] m_traits;

	private MissionBase m_mission = null;

	public void Initialize (Organization.ActiveMission a)
	{
		Henchmen h = a.m_henchmen [0];
		Initialize (a.m_mission, h);

		int turnsLeft = a.m_mission.m_numTurns - a.m_turnsPassed;
		string duration = turnsLeft.ToString ();
		if (turnsLeft > 1) {
			duration += " TURNS";
		} else {
			duration += " TURN";
		}

		m_missionDuration.text = duration;
	}

	public void Initialize (MissionBase m, Henchmen h)
	{
		m_mission = m;
		m_missionName.text = m.m_name.ToUpper();
		m_missionDescription.text = m.m_description;
		m_missionCost.text = m.m_cost.ToString ();

		string duration = m.m_numTurns.ToString ();
		if (m.m_numTurns > 1) {
			duration += " TURNS";
		} else {
			duration += " TURN";
		}

		m_missionDuration.text = duration;

		int successChance = 0;

		MissionBase.MissionTrait[] traits = m.GetTraitList (1);

		for (int i = 0; i < m_traits.Length; i++) {
			TraitButton t = m_traits [i];
			if (i < traits.Length) {
				MissionBase.MissionTrait mT = traits [i];
				if (mT.m_trait != null)
				{
					bool hasTrait = h.HasTrait (mT.m_trait);
					t.Initialize (mT.m_trait, hasTrait);

					if (hasTrait) {
						successChance = Mathf.Clamp(successChance + mT.m_percentageContribution, 0, 100);
					}
				}
			} else {
				t.Deactivate ();
			}
		}

		m_missionSuccessChance.text = successChance.ToString() + "% SUCCESS";
	}

	public void StartMissionButtonPressed ()
	{
		if (m_mission != null && m_mission.m_cost <= GameManager.instance.game.player.currentCommandPool) {
			CallHenchmenMenu.instance.Startmission (m_mission);
		}
	}

}
