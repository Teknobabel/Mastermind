using UnityEngine;
using System.Collections;

public class DynamicTrait : TraitData {

	public enum LinkType
	{
		None,
		Ally,
		Rival,
		Wanted,
		Loves,
		Hates,
		Experienced,
		Fear,
		Citizen,
	}

	public LinkType m_linkType = LinkType.None;
	public Henchmen m_henchmen = null;
	public Region m_region = null;
	public MissionBase m_mission = null;

	public override string GetName ()
	{
		string s = "";

		switch (m_linkType) {

		case LinkType.Ally:
			s = "ALLY OF:";
			s += "\n" + m_henchmen.henchmenName.ToUpper ();
			break;
		case LinkType.Rival:
			s = "RIVAL OF:";
			s += "\n" + m_henchmen.henchmenName.ToUpper ();
			break;
		case LinkType.Wanted:
			s = "WANTED IN:";
			s += "\n" + m_region.regionName.ToUpper ();
			break;
		case LinkType.Loves:
			
			break;
		case LinkType.Hates:

			break;
		case LinkType.Experienced:

			break;
		case LinkType.Fear:

			break;
		case LinkType.Citizen:
			s = "CITIZEN OF:";
			s += "\n" + m_region.regionName.ToUpper ();
			break;
		}

		return s;
	}
}
