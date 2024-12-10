using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController {
  [Header("Skill")]
  public float kickForce;
  public Transform handPosition;
  public GameObject rock;

  public void KickOff() {
    if (target != null && transform.IsFacingTarget(target.transform)) {
      Vector3 dir = target.transform.position - transform.position;
      dir.Normalize();
      target.GetComponent<NavMeshAgent>().isStopped = true;
      target.GetComponent<NavMeshAgent>().velocity = dir * kickForce;
      target.GetComponent<Animator>().SetTrigger("Dizzy");
      target.GetComponent<CharactorStats>().TakeDamage(charactorStats);
    }
  }

  protected override void SwitchAnimation() {
    animator.SetBool("Walk", isWalk);
    animator.SetBool("Chase", isChase);
    animator.SetBool("Follow", isFollow);
    animator.SetBool("Dead", isDead);
  }

  public void ThrowRock() {
    if (target != null) {
      var newRock = Instantiate(rock, handPosition.position, quaternion.identity);
      newRock.GetComponent<Rock>().target = target;
    }
  }
}
