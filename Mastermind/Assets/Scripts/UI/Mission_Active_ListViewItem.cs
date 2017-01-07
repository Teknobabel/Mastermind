using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class Mission_Active_ListViewItem : MonoBehaviour {

	public TextMeshProUGUI m_missionName;
	public TextMeshProUGUI m_missionDescription;
	public TextMeshProUGUI m_missionDuration;
	public TextMeshProUGUI m_missionSuccessChance;
	public Image m_missionPortrait;

	public Transform m_traitPanel;
	public Transform m_henchmenSlotPanel;

	public GameObject m_traitButton;
	public GameObject m_henchmenSlotButton;

	private List<TraitButton> m_traitButtons = new List<TraitButton>();

	private MissionWrapper m_missionWrapper = null;

	public void Initialize (MissionWrapper a)
	{
		m_missionWrapper = a;

		m_missionName.text = a.m_mission.m_name.ToUpper();
		m_missionDescription.text = a.m_mission.m_description;
		m_missionSuccessChance.text = a.m_mission.CalculateCompletionPercentage(a).ToString() + "% SUCCESS";

		int turnsLeft = a.m_mission.m_numTurns - a.m_turnsPassed;
		string duration = turnsLeft.ToString ();
		if (turnsLeft > 1) {
			duration += " TURNS";
		} else {
			duration += " TURN";
		}

		m_missionDuration.text = duration;

		DrawTraits(a);

		Region r = a.m_region;

		for (int i = 0; i < r.henchmenSlots.Count; i++) {

			Region.HenchmenSlot hSlot = r.henchmenSlots[i];

			if (hSlot.m_state == Region.HenchmenSlot.State.Occupied_Player) {
				GameObject h = (GameObject)(Instantiate (m_henchmenSlotButton, m_henchmenSlotPanel));
				h.transform.localScale = Vector3.one;
				RegionHenchmenButton rhb = (RegionHenchmenButton)h.GetComponent<RegionHenchmenButton> ();
				rhb.Initialize (hSlot);
			}
		}
	}

	private void DrawTraits (MissionWrapper mw)
	{
		MissionBase.MissionTrait[] traits = mw.m_mission.GetTraitList (mw.m_mission.GetMissionRank(mw));
		List<TraitData> combinedTraitList = mw.m_mission.GetCombinedTraitList (mw);




		for (int i = 0; i < 6; i++) {

			GameObject thisT = (GameObject)(Instantiate (m_traitButton, m_traitPanel));
			thisT.transform.localScale = Vector3.one;
			TraitButton tb = (TraitButton)thisT.GetComponent<TraitButton> ();

			m_traitButtons.Add (tb);

		}

		for (int i = 0; i < m_traitButtons.Count; i++) {

			TraitButton t = m_traitButtons [i];

			if (i < traits.Length) {

				MissionBase.MissionTrait mT = traits [i];
				bool hasTrait = false;
				bool hasAsset = false;

				if (mT.m_trait != null) {
					hasTrait = combinedTraitList.Contains (mT.m_trait);
					t.Initialize (mT.m_trait, hasTrait);
				}

				if (!hasTrait && mT.m_asset != null) {
					hasAsset = GameManager.instance.game.player.currentAssets.Contains (mT.m_asset);
					t.Initialize (mT.m_asset, hasAsset);
				}

			} else {
				t.Deactivate ();
			}
		}









//		for (int i = 0; i < m_traits.Length; i++) {
//
//			TraitButton t = m_traits [i];
//
//			if (i < traits.Length) {
//
//				MissionBase.MissionTrait mT = traits [i];
//				bool hasTrait = false;
//				bool hasAsset = false;
//
//				if (mT.m_trait != null) {
//					hasTrait = combinedTraitList.Contains (mT.m_trait);
//					t.Initialize (mT.m_trait, hasTrait);
//				}
//
//				if (!hasTrait && mT.m_asset != null) {
//					hasAsset = GameManager.instance.game.player.currentAssets.Contains (mT.m_asset);
//					t.Initialize (mT.m_asset, hasAsset);
//				}
//
//			} else {
//				
//				t.Deactivate ();
//			}
//		}
	}

	public void CancelMission ()
	{
		GameManager.instance.game.player.CancelMission (m_missionWrapper);

		if (GameManager.instance.currentMenuState == MenuState.State.MissionMenu) {

			MissionMenu.instance.UpdateMissionMenu ();
		} else if (GameManager.instance.currentMenuState == MenuState.State.HenchmenDetailMenu) {

			HenchmenDetailMenu.instance.UpdateListView ();
		}
	}
}
