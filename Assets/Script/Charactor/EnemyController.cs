using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { GUARD, PATROL, CHASE, DEAD };

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharactorStats))]
public class EnemyController : MonoBehaviour, IEndGameObserver {
  private EnemyStates state;
  private NavMeshAgent agent;
  protected GameObject target;
  protected Animator animator;
  protected CharactorStats charactorStats;
  private BoxCollider boxCollider;
  protected bool isWalk;
  protected bool isChase;
  protected bool isFollow;
  protected bool isDead;
  private bool gameOver;
  private Vector3 waypoint;
  private Vector3 originPoint;
  private Quaternion originQuaternion;
  private float stopTimer;
  private float attackTimer;

  [Header("基础设置")]
  public float sightRadius;
  public float speed;

  [Header("追击")]
  public bool isGurad;
  public float patrolRadius;
  public float lookAtTime;

  private void Awake() {
    agent = GetComponent<NavMeshAgent>();
    animator = GetComponent<Animator>();
    charactorStats = GetComponent<CharactorStats>();
    boxCollider = GetComponent<BoxCollider>();
  }

  private void Start() {
    originPoint = transform.position;
    originQuaternion = transform.rotation;
    if (isGurad) {
      state = EnemyStates.GUARD;
    } else {
      state = EnemyStates.PATROL;
      GetWaypoint();
    }

    if (GameManager.IsInitialized) {
      GameManager.Instance.AddObserver(this);
    }
  }

  private void OnEnable() {

  }

  private void OnDisable() {
    if (GameManager.IsInitialized) {
      GameManager.Instance.RemoveObserver(this);
    }
  }

  private void Update() {
    isDead = charactorStats.currentHealth == 0;
    if (gameOver) {
      return;
    }
    SwitchStates();
    SwitchAnimation();

    attackTimer -= Time.deltaTime;
  }

  virtual protected void SwitchAnimation() {
    animator.SetBool("Walk", isWalk);
    animator.SetBool("Chase", isChase);
    animator.SetBool("Follow", isFollow);
    animator.SetBool("Critcal", charactorStats.isCritcal);
    animator.SetBool("Dead", isDead);
  }

  private void SwitchStates() {
    if (isDead) {
      state = EnemyStates.DEAD;
    } else if (FindPlayer()) {
      state = EnemyStates.CHASE;
      stopTimer = 0;
    }
    switch (state) {
      case EnemyStates.GUARD:
        GuardUpdate();
        break;
      case EnemyStates.PATROL:
        PatrolUpdate();
        break;
      case EnemyStates.CHASE:
        ChaseUpdate();
        break;
      case EnemyStates.DEAD:
        DeadUpdate();
        break;
    }
  }

  private void GuardUpdate() {
    isChase = false;
    isFollow = false;
    if (Vector3.Distance(transform.position, originPoint) > agent.stoppingDistance) {
      agent.destination = originPoint;
      isWalk = true;
    } else {
      transform.rotation = Quaternion.Lerp(transform.rotation, originQuaternion, 0.01f);
      isWalk = false;
    }
  }

  private void PatrolUpdate() {
    isChase = false;
    isFollow = false;
    if (Vector3.Distance(transform.position, waypoint) <= agent.stoppingDistance) {
      isWalk = false;
      if (stopTimer < lookAtTime) {
        stopTimer += Time.deltaTime;
      } else {
        stopTimer = 0;
        GetWaypoint();
      }
    } else {
      agent.destination = waypoint;
      isWalk = true;
    }
  }

  private void GetWaypoint() {
    float waypointX = Random.Range(-patrolRadius, patrolRadius);
    float waypointZ = Random.Range(-patrolRadius, patrolRadius);
    Vector3 randomPoint = new Vector3(originPoint.x + waypointX, originPoint.y, originPoint.z + waypointZ);
    waypoint = NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRadius, 1) ? hit.position : transform.position;
  }

  private void ChaseUpdate() {
    isWalk = false;
    isChase = true;
    agent.speed = speed;
    if (target == null) {
      isFollow = false;
      agent.destination = transform.position;
      if (stopTimer < lookAtTime) {
        stopTimer += Time.deltaTime;
      } else {
        if (isGurad) {
          state = EnemyStates.GUARD;
        } else {
          state = EnemyStates.PATROL;
        }
      }
    } else {
      agent.isStopped = false;
      isFollow = true;
      agent.destination = target.transform.position;
    }

    if (TargetInAttackRange() || TargetInSkillRange()) {
      isFollow = false;
      agent.isStopped = true;

      if (attackTimer < 0) {
        attackTimer = charactorStats.coolDown;
        Attack();
      }
    }
  }

  private bool TargetInAttackRange() {
    if (target == null) {
      return false;
    }
    return Vector3.Distance(transform.position, target.transform.position) <= charactorStats.attackRange;
  }

  private bool TargetInSkillRange() {
    if (target == null) {
      return false;
    }
    return Vector3.Distance(transform.position, target.transform.position) <= charactorStats.skillRange;
  }

  private void Attack() {
    charactorStats.isCritcal = Random.value < charactorStats.criticalChance;
    transform.LookAt(target.transform);
    if (TargetInAttackRange()) {
      animator.SetTrigger("Attack");
    } else if (TargetInSkillRange()) {
      animator.SetTrigger("Skill");
    }
  }

  private bool FindPlayer() {
    var colliders = Physics.OverlapSphere(transform.position, sightRadius);
    foreach (var collider in colliders) {
      if (collider.CompareTag("Player")) {
        target = collider.gameObject;
        return true;
      }
    }
    target = null;
    return false;
  }

  private void DeadUpdate() {
    agent.radius = 0;
    boxCollider.enabled = false;
    Destroy(gameObject, 2f);
  }

  private void Hit() {
    if (target != null && transform.IsFacingTarget(target.transform)) {
      target.GetComponent<CharactorStats>().TakeDamage(charactorStats);
    }
  }

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, sightRadius);
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position, patrolRadius);
  }

  public void EndNotify() {
    animator.SetBool("Win", true);
    target = null;
    isChase = false;
    isWalk = false;
    isFollow = false;
    gameOver = true;
  }
}
