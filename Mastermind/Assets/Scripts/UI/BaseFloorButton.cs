using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BaseFloorButton : MonoBehaviour {

	public TextMeshProUGUI m_text;

	public RawImage m_floorButtonBG;

	public RegionHenchmenButton m_henchmenButton;

	private Floor m_floor;

	public void Initialize (Floor floor)
	{
		m_floor = floor;

		string s = "FLOOR " + floor.m_floorNumber.ToString () + ": \n";

		if (floor.m_floorState == Base.FloorState.Empty) {
			
			s += "EMPTY";
			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Base_Floor_Empty_Text);
			m_floorButtonBG.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Base_Floor_Empty_BG);

		} else if (floor.m_floorState == Base.FloorState.Occupied) {
			
			s += floor.m_installedUpgrade.m_name.ToUpper ();

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
//		Debug.Log(floor.m_henchmenSlot);
		m_henchmenButton.Initialize (floor.m_henchmenSlot);

		m_text.text = s;
	}

	public void ButtonClicked ()
	{
		if (m_floor.m_floorState == Base.FloorState.Empty) {
			Debug.Log ("Empty Foor Button Clicked");
			LairMenu.instance.SelectUpgradeForFloor (m_floor);
		} else if (m_floor.m_floorState == Base.FloorState.Occupied && m_floor.m_installedUpgrade.m_assetType != Asset.AssetType.TrapRoom) {
			LairMenu.instance.SelectMissionForFloor (m_floor);
		}
	}

	public void HenchmenButtonClicked ()
	{
		
		if (m_floor.m_henchmenSlot.m_state == Region.HenchmenSlot.State.Empty) {

			// player can move any henchmen at the lair or in the base to this slot

			LairMenu.instance.SelectHenchmenForFloor(m_floor);
		}
	}
}
