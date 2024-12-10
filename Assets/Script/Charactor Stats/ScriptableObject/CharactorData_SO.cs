using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Charactor Stats/Data")]
public class CharactorData_SO : ScriptableObject {
  public int maxHealth;
  public int currentHealth;
  public int baseDefence;
  public int currentDefence;

  [Header("经验")]
  public int currentLevel;
  public int maxLevel;
  public int currentExp;
  public int baseExp;
  public int killPoint;
}
