using UnityEngine;
using System.Collections;

public interface ISubject{

	void AddObserver (IObserver observer);

	void RemoveObserver (IObserver observer);

	void Notify (ISubject subject, GameEvent thisGameEvent);
}
