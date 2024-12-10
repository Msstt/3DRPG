using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SaveManager : Singleton<SaveManager> {

  private void Update() {
    if (Input.GetKeyDown(KeyCode.S)) {
      SavePlayerData();
    }
    if (Input.GetKeyDown(KeyCode.L)) {
      LoadPlayerData();
    }
  }

  public void SavePlayerData() {
    Save(GameManager.Instance.charactorStats.charactorData, GameManager.Instance.charactorStats.charactorData.name);
  }

  public void LoadPlayerData() {
    Load(GameManager.Instance.charactorStats.charactorData, GameManager.Instance.charactorStats.charactorData.name);
  }

  public void Save(Object data, string key) {
    PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
    PlayerPrefs.Save();
    Debug.Log("?");
  }

  public void Load(Object data, string key) {
    Debug.Log("??");
    if (PlayerPrefs.HasKey(key)) {
      Debug.Log("???");
      JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
    }
  }
}
