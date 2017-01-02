using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Fire Missile")]
public class FireMissile : MissionBase {

	public Asset[] m_requiredAssets;
	public Asset[] m_requiredBaseUpgrades;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}
		t.m_resultsText = "The " + m_requiredAssets[0].m_name.ToUpper() + " takes off.";

		if (a.m_success) {

			t.m_resultsText += "The " + m_requiredAssets[0].m_name.ToUpper() + " detonates in " + a.m_regionInFocus.regionName.ToUpper() + "!";

			// chance to injur or critically injur any agents and henchmen

			foreach (Region.HenchmenSlot hs in a.m_regionInFocus.henchmenSlots) {

				if (hs.m_state == Region.HenchmenSlot.State.Occupied_Player) {

					bool statusChange = false;

					if (Random.Range (0.0f, 1.0f) > 0.6f) {

						IncurInjury (hs.m_henchmen);
						statusChange = true;
					}

					if (Random.Range (0.0f, 1.0f) > 0.75f) {

						IncurInjury (hs.m_henchmen);
						statusChange = true;
					}

					if (statusChange) {
						
						t.m_resultsText += "\n" + hs.m_henchmen.henchmenName.ToUpper () + " is " + hs.m_henchmen.statusTrait.m_name + "!";
					}

				} else if (hs.m_state == Region.HenchmenSlot.State.Occupied_Agent) {

					bool statusChange = false;

					if (Random.Range (0.0f, 1.0f) > 0.6f) {

						IncurInjury (hs.m_agent.m_agent);
						statusChange = true;
					}

					if (Random.Range (0.0f, 1.0f) > 0.75f) {

						IncurInjury (hs.m_agent.m_agent);
						statusChange = true;
					}

					if (statusChange) {

						t.m_resultsText += "\n" + hs.m_agent.m_agent.henchmenName.ToUpper () + " is " + hs.m_agent.m_agent.statusTrait.m_name + "!";
					}
				}
			}

			// chance to destroy any assets

			List<TokenSlot> destroyedAssets = new List<TokenSlot> ();

			foreach (TokenSlot ts in a.m_regionInFocus.assetTokens) {

				if (ts.m_state != TokenSlot.State.None && ts.m_assetToken != null && Random.Range (0.0f, 1.0f) > 0.6f) {

					destroyedAssets.Add (ts);
				}
			}

			while (destroyedAssets.Count > 0) {

				TokenSlot ts = destroyedAssets [0];

				t.m_resultsText += "\n" + ts.m_assetToken.m_name.ToUpper() + " has been destroyed!";

				destroyedAssets.RemoveAt (0);
				a.m_regionInFocus.RemoveAssetToken (ts);
			}

			// chance to seize any control tokens

			foreach (TokenSlot ts in a.m_regionInFocus.controlTokens) {

				if (ts.owner == Region.Owner.AI && Random.Range (0.0f, 1.0f) > 0.75f) {

					if (ts.m_state == TokenSlot.State.Hidden) {

						ts.m_state = TokenSlot.State.Revealed;
					}

					ts.ChangeOwner (Region.Owner.Player);

					t.m_resultsText += "\n" + ts.m_controlToken.m_name.ToUpper() + " is now under " + GameManager.instance.game.player.orgName.ToUpper() + " Control!";

				}
			}

		} else {

			t.m_resultsText += "The" + m_requiredAssets[0].m_name.ToUpper() + " malfunctions and fails to detonate!";


		}

		foreach (Asset asset in m_requiredAssets)
		{
			if (GameManager.instance.game.player.currentAssets.Contains (asset)) {

				t.m_resultsText += "\n" + asset.m_name.ToUpper() + " is removed.";
				GameManager.instance.game.player.currentAssets.Remove (asset);

			}
		}

		t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
		t.m_resultType = GameEvent.Henchmen_MissionCompleted;
		GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
	}

	public override string GetNameText ()
	{
		string s = m_name + "\n";

		if (GameManager.instance.currentMissionWrapper.m_regionInFocus != null) {
			s += "<size=18>" + GameManager.instance.currentMissionWrapper.m_regionInFocus.regionName.ToUpper() + "</size>";
		}

		return s;
	}

	private void IncurInjury (Henchmen h)
	{
		switch (h.statusTrait.m_type) {

		case TraitData.TraitType.Healthy:

			h.UpdateStatusTrait(GameManager.instance.m_statusTraits[1]);
			break;

		case TraitData.TraitType.Injured:

			h.UpdateStatusTrait(GameManager.instance.m_statusTraits[2]);
			break;

		case TraitData.TraitType.Critical:

			h.UpdateStatusTrait(GameManager.instance.m_statusTraits[3]);
			break;
		}
	}

	public override bool IsValid ()
	{
		if (!base.IsValid ()) { return false;}		// valid if player has Control Room, Missile Silo, and Nuclear Missile asset

		MissionWrapper mw = GameManager.instance.currentMissionWrapper;
		Debug.Log (mw.m_floorInFocus.m_installedUpgrade.m_assetType);
		if (mw.m_floorInFocus != null && mw.m_floorInFocus.m_installedUpgrade.m_assetType == Asset.AssetType.LairUpgrade_MissileSilo) {

			Base playerBase = GameManager.instance.game.player.orgBase;

			bool hasRequiredAssets = true;

			foreach (Asset a in m_requiredAssets) {
				if (!GameManager.instance.game.player.currentAssets.Contains (a)) {

					hasRequiredAssets = false;
					break;
				}
			}

			foreach (Asset a in m_requiredBaseUpgrades) {
				if (!playerBase.m_currentAssets.Contains (a)) {

					hasRequiredAssets = false;
					break;
				}
			}

			if (hasRequiredAssets) {

				return true;
			}
		}

		return false;
	}
}
