using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
  private static T instance;

  public static T Instance {
    get { return instance; }
  }

  protected void Awake() {
    if (instance == null) {
      instance = (T)this;
    } else {
      Destroy(this);
    }
  }

  public static bool IsInitialized {
    get { return instance != null; }
  }

  protected void OnDestroy() {
    if (instance == this) {
      instance = null;
    }
  }
}
