using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Reveal Asset")]
public class RevealAsset : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();

		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}

		foreach (Henchmen h in a.m_henchmen) {

			t.m_henchmenIDs.Add (h.id);
		}

		if (a.m_success) {

			// gather all hidden tokens

			Region r = a.m_region;

			List<TokenSlot> hiddenTokens = new List<TokenSlot> ();

			foreach (TokenSlot ts in r.assetTokens) {

				if (ts.m_state == TokenSlot.State.Hidden) {
					hiddenTokens.Add (ts);
				}
			}

			if (hiddenTokens.Count > 0) {
				TokenSlot tB = hiddenTokens [Random.Range (0, hiddenTokens.Count)];
//				tB.ChangeState (TokenSlot.State.Revealed);
				tB.m_state = TokenSlot.State.Revealed;

				t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
				t.m_resultsText += "\n" + tB.m_assetToken.m_name.ToUpper() + " is revealed!";
				t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
				t.m_resultType = GameEvent.Henchmen_MissionCompleted;
				GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);


				// debug

				if (a.m_henchmen.Count > 1)
				{
					
					Henchmen h1 = a.m_henchmen[0];
					Henchmen h2 = a.m_henchmen[1];

					DynamicTrait d = DynamicTrait.CreateInstance<DynamicTrait>();
					d.m_class = TraitData.TraitClass.Dynamic;
					d.m_linkType = DynamicTrait.LinkType.Rival;
					d.m_henchmen = h2;
					h1.AddTrait (d);

					DynamicTrait d2 = DynamicTrait.CreateInstance<DynamicTrait>();
					d2.m_class = TraitData.TraitClass.Dynamic;
					d2.m_linkType = DynamicTrait.LinkType.Wanted;
					d2.m_region = a.m_region;
					h1.AddTrait (d2);

					DynamicTrait d4 = DynamicTrait.CreateInstance<DynamicTrait>();
					d4.m_class = TraitData.TraitClass.Dynamic;
					d4.m_linkType = DynamicTrait.LinkType.Wanted;
					d4.m_region = a.m_region;
					h2.AddTrait (d4);

					DynamicTrait d3 = DynamicTrait.CreateInstance<DynamicTrait>();
					d3.m_class = TraitData.TraitClass.Dynamic;
					d3.m_linkType = DynamicTrait.LinkType.Rival;
					d3.m_henchmen = h1;
					h2.AddTrait (d3);
				}
			}
		} else {

			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

		}

	}

	public override bool IsValid ()
	{
		bool hasPreRequisites = base.IsValid ();
			
		// valid if there are any hidden tokens in the region

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;

			foreach (TokenSlot t in r.assetTokens) {

				if (t.m_state == TokenSlot.State.Hidden && hasPreRequisites) {
					return true;
				}
			}
		}

		return false;
	}
}
