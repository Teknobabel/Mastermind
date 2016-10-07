using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class TraitData : ScriptableObject {

	public enum TraitType
	{
		None = 0,
		Hacker = 1,
		Mercenary = 2,
		Thief = 3,
		Bureaucrat = 4,
		Spy = 5,
		Assassin = 6,
		Scientist = 7,
		Engineer = 8,
		Bodyguard = 9,
		Wealthy = 10,
		Connections = 11,
		Lab = 12,
		Soldiers = 13,
		HackerCollective = 14,
		Gang = 15,
		Tanks = 16,
		Jets = 17,
		CuttingEdgeTech = 18,
		Blackmail = 19,
		Charismatic = 20,
		TechWhiz = 21,
		CodeBreaker = 22,
		Genius = 23,
		MartialArtist = 24,
		Leader = 25,
		Strong = 26,
		Tough = 27,
		Intimidating = 28,
		SharpShooter = 29,
		Coward = 30,
		Weak = 31,
		ShortTempered = 32,
		Wanted = 33,
		Arrogant = 34,
		Paranoid = 35,
		Stubborn = 36,
		Violent = 37,
		Chaotic = 38,
		Greed = 39,
		Injured = 40,
		Incapacitated = 41,
		Captured = 42,
		Zombified = 43,
	}

	public enum TraitClass
	{
		None = 0,
		Skill = 1,
		Resource = 2,
		Gift = 3,
		Flaw = 4,
		Status = 5,
	}

	public string m_name = "Null";
	public TraitType m_type = TraitType.None;
	public TraitClass m_class = TraitClass.None;

	// Use this for initialization
	void OnEnable () {
	
	}

}
