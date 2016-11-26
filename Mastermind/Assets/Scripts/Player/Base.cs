using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base {

	public enum FloorState {
		None,
		Empty,
		Occupied,
		UpgradeInProgress,
	}
	public class Floor 
	{
		public int m_floorNumber = -1;
		public Asset m_installedUpgrade = null;
		public FloorState m_floorState = FloorState.None;
		public List<Henchmen> m_guards = new List<Henchmen> ();
		public int m_baseDefense = 0;
	}

	public List<Floor> m_floors = new List<Floor>();

	public List<Asset> m_currentAssets = new List<Asset> ();

	public void Initialize (int numFloors)
	{
		for (int i = 0; i < numFloors; i++) {

			Floor f = new Floor ();
			f.m_floorState = FloorState.Empty;
			f.m_floorNumber = i+1;
			m_floors.Add (f);
		}
	}

	public void InstallAsset (Asset asset)
	{
		foreach (Floor f in m_floors) {

			if (f.m_floorState == FloorState.Empty) {

				f.m_floorState = FloorState.Occupied;
				f.m_installedUpgrade = asset;
				f.m_baseDefense = asset.m_defenseValue;
				m_currentAssets.Add (asset);
				return;
			}
		}
	}

	public void InstallAsset (int floorNumber, Asset asset)
	{
		foreach (Floor f in m_floors) {

			if (f.m_floorNumber == floorNumber) {

				f.m_floorState = FloorState.Occupied;
				f.m_installedUpgrade = asset;
				f.m_baseDefense = asset.m_defenseValue;
				m_currentAssets.Add (asset);
				return;
			}
		}
	}

	public void RemoveAsset (int floorNumber)
	{
		foreach (Floor f in m_floors) {

			if (f.m_floorNumber == floorNumber) {

				f.m_floorState = FloorState.Empty;
				f.m_baseDefense = 0;
				m_currentAssets.Remove (f.m_installedUpgrade);
				f.m_installedUpgrade = null;

				return;
			}
		}
	}
}
