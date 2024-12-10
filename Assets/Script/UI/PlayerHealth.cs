using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
  private Image health;
  private Image exp;
  private Text level;
  private void Awake() {
    health = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    exp = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    level = transform.GetChild(2).GetComponent<Text>();
  }
  private void Update() {
    CharactorStats charactorStats = GameManager.Instance.charactorStats;
    if (charactorStats == null) {
      return;
    }
    health.fillAmount = (float)charactorStats.currentHealth / charactorStats.maxHealth;
    exp.fillAmount = (float)charactorStats.currentExp / charactorStats.baseExp;
    level.text = "Level " + charactorStats.currentLevel.ToString("00");
  }
}
