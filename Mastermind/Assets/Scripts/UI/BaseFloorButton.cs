using UnityEngine;
using TMPro;
using System.Collections;

public class BaseFloorButton : MonoBehaviour {

	public TextMeshProUGUI m_text;

	private Base.Floor m_floor;

	public void Initialize (Base.Floor floor)
	{
		m_floor = floor;

		string s = "FLOOR " + floor.m_floorNumber.ToString () + ": ";

		if (floor.m_floorState == Base.FloorState.Empty) {
			
			s += "EMPTY";

		} else if (floor.m_floorState == Base.FloorState.Occupied) {
			
			s += floor.m_installedUpgrade.m_name.ToUpper ();

		} else if (floor.m_floorState == Base.FloorState.UpgradeInProgress) {
			
			s += "CONSTRUCTION IN PROGRESS\n (";

			foreach (MissionWrapper mw in GameManager.instance.game.player.activeMissions) {

				if (mw.m_scope == MissionBase.TargetType.BaseUpgrade && mw.m_floorInFocus != null && mw.m_floorInFocus.m_floorNumber == m_floor.m_floorNumber) {

					s += mw.m_mission.m_name.ToUpper () + ")";
				}
			}
		}

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
}
