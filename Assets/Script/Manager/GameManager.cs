using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
  public CharactorStats charactorStats;
  private List<IEndGameObserver> observers = new();

  private void Update() {
    if (charactorStats.currentHealth == 0) {
      Notify();
    }
  }

  public void RigisterPlayer(CharactorStats stats) {
    charactorStats = stats;
  }

  public void AddObserver(IEndGameObserver observer) {
    observers.Add(observer);
  }

  public void RemoveObserver(IEndGameObserver observer) {
    observers.Remove(observer);
  }

  private void Notify() {
    foreach (var observer in observers) {
      observer.EndNotify();
    }
  }
}
