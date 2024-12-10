using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorStats : MonoBehaviour {
  public CharactorData_SO templateData;

  [HideInInspector]
  public CharactorData_SO charactorData;
  public AttackData_SO attackData;

  [HideInInspector]
  public bool isCritcal;

  [HideInInspector]
  public event Action<int, int> healthUpdate;

  private void Awake() {
    charactorData = Instantiate(templateData);
  }

  #region 获取数据
  public int maxHealth {
    get { return charactorData == null ? 0 : charactorData.maxHealth; }
    set { charactorData.maxHealth = value; }
  }

  public int currentHealth {
    get { return charactorData == null ? 0 : charactorData.currentHealth; }
    set { charactorData.currentHealth = value; }
  }

  public int baseDefence {
    get { return charactorData == null ? 0 : charactorData.baseDefence; }
    set { charactorData.baseDefence = value; }
  }

  public int currentDefence {
    get { return charactorData == null ? 0 : charactorData.currentDefence; }
    set { charactorData.currentDefence = value; }
  }

  public int currentLevel {
    get { return charactorData == null ? 0 : charactorData.currentLevel; }
    set { charactorData.currentLevel = value; }
  }

  public int maxLevel {
    get { return charactorData == null ? 0 : charactorData.maxLevel; }
    set { charactorData.maxLevel = value; }
  }

  public int currentExp {
    get { return charactorData == null ? 0 : charactorData.currentExp; }
    set { charactorData.currentExp = value; }
  }

  public int baseExp {
    get { return charactorData == null ? 0 : charactorData.baseExp; }
    set { charactorData.baseExp = value; }
  }

  public int killPoint {
    get { return charactorData == null ? 0 : charactorData.killPoint; }
    set { charactorData.killPoint = value; }
  }

  public float attackRange {
    get { return attackData == null ? 0 : attackData.attackRange; }
    set { attackData.attackRange = value; }
  }

  public float skillRange {
    get { return attackData == null ? 0 : attackData.skillRange; }
    set { attackData.skillRange = value; }
  }

  public float coolDown {
    get { return attackData == null ? 0 : attackData.coolDown; }
    set { attackData.coolDown = value; }
  }

  public int minDamage {
    get { return attackData == null ? 0 : attackData.minDamage; }
    set { attackData.minDamage = value; }
  }

  public int maxDamage {
    get { return attackData == null ? 0 : attackData.maxDamage; }
    set { attackData.maxDamage = value; }
  }

  public float criticalMulitiplier {
    get { return attackData == null ? 0 : attackData.criticalMulitiplier; }
    set { attackData.criticalMulitiplier = value; }
  }

  public float criticalChance {
    get { return attackData == null ? 0 : attackData.criticalChance; }
    set { attackData.criticalChance = value; }
  }

  #endregion

  public void TakeDamage(CharactorStats attacker) {
    float damage = UnityEngine.Random.Range(attacker.minDamage, attacker.maxDamage);
    if (attacker.isCritcal) {
      GetComponent<Animator>().SetTrigger("Hit");
      damage *= attacker.criticalMulitiplier;
    }
    damage = Mathf.Max(damage - currentDefence, 0);
    currentHealth = Mathf.Max(currentHealth - (int)damage, 0);
    healthUpdate?.Invoke(currentHealth, maxHealth);
    if (currentHealth == 0) {
      attacker.UpdateExp(killPoint);
    }
  }

  public void TakeDamage(int damage) {
    damage = Mathf.Max(damage - currentDefence, 0);
    currentHealth = Mathf.Max(currentHealth - damage, 0);
    healthUpdate?.Invoke(currentHealth, maxHealth);
  }

  public void UpdateExp(int point) {
    charactorData.currentExp += point;
    if (charactorData.currentExp >= charactorData.baseExp) {
      LevelUp();
    }
  }

  private void LevelUp() {
    if (charactorData.currentLevel == charactorData.maxLevel) {
      charactorData.currentExp = charactorData.baseExp;
      return;
    }
    charactorData.currentLevel += 1;
    charactorData.currentExp -= charactorData.baseExp;
    Debug.Log("Level Up");
  }
}
