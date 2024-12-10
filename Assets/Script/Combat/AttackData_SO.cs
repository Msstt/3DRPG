using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/Data")]
public class AttackData_SO : ScriptableObject {
  public float attackRange;
  public float skillRange;
  public float coolDown;
  public int minDamage;
  public int maxDamage;
  public float criticalMulitiplier;
  public float criticalChance;
}
