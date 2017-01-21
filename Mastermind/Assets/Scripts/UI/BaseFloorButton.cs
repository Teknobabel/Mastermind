using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BaseFloorButton : MonoBehaviour {

	public TextMeshProUGUI 
	m_text,
	m_currentMissionText;

	public RawImage m_floorButtonBG;

	public GameObject
	m_henchmenButton;

	public Transform m_henchmenSlotPanel;

	private Floor m_floor;

	public void Initialize (Floor floor)
	{
		m_floor = floor;

//		string s = "<size=18>FLOOR " + floor.m_floorNumber.ToString () + "</size>\n";

		string s = "";

		if (floor.m_floorState == Base.FloorState.Empty) {
			
			s += "BUILD NEW FLOOR";
			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Base_Floor_Empty_Text);
			m_floorButtonBG.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Base_Floor_Empty_BG);

		} else if (floor.m_floorState == Base.FloorState.Occupied) {
			
			s += floor.m_installedUpgrade.m_name.ToUpper () + "(" + floor.m_level.ToString() + ")";

			if (floor.m_installedUpgrade.m_assetType == Asset.AssetType.Lair) {

				m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Base_Floor_Lair_Text);
				m_floorButtonBG.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Base_Floor_Lair_BG);

			} else {

				m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Base_Floor_Occupied_Text);
				m_floorButtonBG.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Base_Floor_Occupied_BG);
			}

		} else if (floor.m_floorState == Base.FloorState.UpgradeInProgress) {
			
			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Base_Floor_Empty_Text);
			m_floorButtonBG.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Base_Floor_Empty_BG);

			s += "CONSTRUCTION IN PROGRESS\n (";

			foreach (MissionWrapper mw in GameManager.instance.game.player.activeMissions) {

				if (mw.m_scope == MissionBase.TargetType.BaseUpgrade && mw.m_floorInFocus != null && mw.m_floorInFocus.m_floorNumber == m_floor.m_floorNumber) {

					s += mw.m_mission.m_name.ToUpper () + ")";
				}
			}
		}

		// get # of henchmen slots

		int numSlots = 1;

		if (floor.m_installedUpgrade != null) {

			foreach (BaseFloor.BaseFloorMissions bfm in floor.m_installedUpgrade.m_missions) {

				if (bfm.m_level <= floor.m_level) {

					if (bfm.m_extraHenchmen) {

						numSlots++;
					}
				}
			}
		}

		for (int i=0; i < numSlots; i++)
		{
			Region r = floor.m_region;

			GameObject h = (GameObject)(Instantiate (m_henchmenButton, m_henchmenSlotPanel));
			h.transform.localScale = Vector3.one;
			RegionHenchmenButton rhb = (RegionHenchmenButton)h.GetComponent<RegionHenchmenButton> ();
			Debug.Log (rhb);
			Region.HenchmenSlot hSlot = r.henchmenSlots[i];
			rhb.Initialize (hSlot);

			// set up onclick event

			Button b = h.GetComponent<Button> ();
			b.onClick = new Button.ButtonClickedEvent ();
			b.onClick.AddListener (() => {
				HenchmenButtonClicked (hSlot);
			});

		}

		m_text.text = s;

		bool missionUnderway = false;
		foreach (MissionWrapper mw in GameManager.instance.game.player.activeMissions) {

			if (mw.m_floorInFocus != null && mw.m_floorInFocus.m_floorNumber == m_floor.m_floorNumber) {

				missionUnderway = true;
				m_currentMissionText.text = "MISSION: \n" + mw.m_mission.m_name.ToUpper ();
				int turnsRemaining = mw.m_mission.m_numTurns - mw.m_turnsPassed;
				m_currentMissionText.text += "\n" + turnsRemaining.ToString() + " TURNS";
			}
		}

		if (!missionUnderway) {
			m_currentMissionText.text = "MISSION: \nNONE";
		}
	}

	public void ButtonClicked ()
	{
//		if (m_floor.m_floorState == Base.FloorState.Empty) {
//			Debug.Log ("Empty Foor Button Clicked");
//			LairMenu.instance.SelectUpgradeForFloor (m_floor);
//		} else if (m_floor.m_floorState == Base.FloorState.Occupied && m_floor.m_installedUpgrade.m_assetType != Asset.AssetType.TrapRoom) {
			LairMenu.instance.SelectMissionForFloor (m_floor);
//		}
	}

	public void HenchmenButtonClicked (Region.HenchmenSlot hs)
	{
		
		if (hs.m_state == Region.HenchmenSlot.State.Empty) {

			// player can move any henchmen at the lair or in the base to this slot

			LairMenu.instance.SelectHenchmenForFloor(m_floor);
		}
	}
}
