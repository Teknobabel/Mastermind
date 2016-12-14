using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class Asset : ScriptableObject, ISubject {

	public enum Rank
	{
		None,
		One,
		Two,
		Three,
		Four,
	}

	public enum AssetType
	{
		None,
		TradeGoods,
		Soldiers,
		Workers,
		Money,
		Scientists,
		Tanks,
		Data,
		Weapons,
		Drugs,
		Art,
		Missile,
		Jets,
		Blackmail,
		Satellite,
		Rocket,
		NuclearMissile,
		SpySatellite,
		Random,
		Virus,
		MegaLaser,
		MegaMagnet,
		Technology,
		MindControlDevice,
		LairUpgrade_Lab,
		LairUpgrade_Workshop,
		LairUpgrade_ControlRoom,
		LairUpgrade_MissileSilo,
		DrillingRig,
		LaserMagnet,
		MindControlSatellite,
		OrbitalLaserCannon,
		WeatherDominator,
		ZVirus,
		Zombies,
		AI,
		SuperVirus,
		Relic,
		Sacrifice,
		Portal,
		LairUpgrade_Dojo,
		LairUpgrade_Library,
		LairUpgrade_DangerRoom,
		LairUpgrade_Infirmary,
		LairUpgrade_Jail,
		LairUpgrade_DarkNet,
		LairUpgrade_Vault,
		LairUpgrade_Nexus,
		LairUpgrade_Barracks,
		LairUpgrade_Armory,
		LairUpgrade_Runway,
		LairUpgrade_BackDoor,
		LairUpgrade_BlackMarket,
		Intel,
		TrapRoom,
		CommandCenter,
		LairUpgrade_Cloak,
		LairUpgrade_SecurityStation,
		LairUpgrade_Warehouse,
	}

	private List<IObserver>
	m_observers = new List<IObserver> ();

	public void AddObserver (IObserver observer)
	{
		if (!m_observers.Contains(observer))
		{
			m_observers.Add (observer);
		}
	}

	public void RemoveObserver (IObserver observer)
	{
		if (m_observers.Contains(observer))
		{
			m_observers.Remove(observer);
		}
	}

	public void Notify (ISubject subject, GameEvent thisGameEvent)
	{
		List<IObserver> observers = new List<IObserver> (m_observers);

		for (int i=0; i < observers.Count; i++)
		{
			observers[i].OnNotify(subject, thisGameEvent);
		}
	}

	public string m_name = "Null";
	public AssetType m_assetType = AssetType.None;
	public Rank m_rank = Rank.None;
	public int m_defenseValue = 0;
}
