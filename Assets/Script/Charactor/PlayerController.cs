using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, IEndGameObserver {
  private NavMeshAgent agent;
  private Animator animator;
  private GameObject attackTarget;
  private CharactorStats charactorStats;
  private float attackTimer;
  private bool isDead;

  private void Awake() {
    agent = GetComponent<NavMeshAgent>();
    animator = GetComponent<Animator>();
    charactorStats = GetComponent<CharactorStats>();
  }

  private void Start() {
    if (MouseManager.IsInitialized) {
      MouseManager.Instance.OnMouseClicked += MoveToTarget;
      MouseManager.Instance.OnEnemyClicked += AttackEvent;
    }

    if (GameManager.IsInitialized) {
      GameManager.Instance.RigisterPlayer(charactorStats);
    }
  }

  private void OnEnable() {
    if (GameManager.IsInitialized) {
      GameManager.Instance.AddObserver(this);
    }
  }

  private void OnDisable() {
    if (GameManager.IsInitialized) {
      GameManager.Instance.RemoveObserver(this);
    }
  }


  private void Update() {
    isDead = charactorStats.currentHealth == 0;
    if (isDead) {
      animator.SetBool("Dead", isDead);
      return;
    }
    SwitchAnimation();

    if (attackTimer >= 0) {
      attackTimer -= Time.deltaTime;
    }
  }

  private void MoveToTarget(Vector3 target) {
    if (isDead) {
      return;
    }
    agent.isStopped = false;
    agent.destination = target;
    StopAllCoroutines();
  }

  private void SwitchAnimation() {
    animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    animator.SetBool("Critcal", charactorStats.isCritcal);
  }

  private void AttackEvent(GameObject target) {
    if (isDead) {
      return;
    }
    if (target != null) {
      attackTarget = target;
      StartCoroutine(AttackEventCoroutine());
    }
  }

  private IEnumerator AttackEventCoroutine() {
    agent.isStopped = false;
    while (Vector3.Distance(attackTarget.transform.position, transform.position) > charactorStats.attackRange) {
      agent.destination = attackTarget.transform.position;
      yield return null;
    }
    agent.isStopped = true;
    if (attackTimer < 0) {
      transform.LookAt(attackTarget.transform.position);
      charactorStats.isCritcal = UnityEngine.Random.value < charactorStats.criticalChance;
      animator.SetTrigger("Attack");
      attackTimer = 0.5f;
    }
  }

  private void Hit() {
    if (attackTarget == null) {
      return;
    }
    if (attackTarget.CompareTag("Attackable") && attackTarget.GetComponent<Rock>().state == Rock.State.HitNothing) {
      attackTarget.GetComponent<Rock>().state = Rock.State.HitEnemy;
      attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
      attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
    } else if (transform.IsFacingTarget(attackTarget.transform)) {
      attackTarget.GetComponent<CharactorStats>().TakeDamage(charactorStats);
    }
  }

  public void EndNotify() {
  }
}
