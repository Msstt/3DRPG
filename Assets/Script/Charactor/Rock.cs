using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour {
  public enum State {
    HitPlayer,
    HitEnemy,
    HitNothing
  }
  private Rigidbody rigidbody_;
  private Vector3 direction;
  public State state;
  public GameObject target;
  public float force;
  public int damage;
  public GameObject breakEffect;

  private void Start() {
    rigidbody_ = GetComponent<Rigidbody>();
    FlyToTarget();
  }

  private void FixedUpdate() {
    if (rigidbody_.velocity.sqrMagnitude < 1.0) {
      state = State.HitNothing;
    }
  }

  private void FlyToTarget() {
    rigidbody_.velocity = Vector3.one;
    direction = (target.transform.position - transform.position + Vector3.up).normalized;
    rigidbody_.AddForce(direction * force, ForceMode.Impulse);
  }

  private void OnCollisionEnter(Collision other) {
    Debug.Log(state);
    switch (state) {
      case State.HitPlayer:
        if (other.gameObject.CompareTag("Player")) {
          other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
          other.gameObject.GetComponent<NavMeshAgent>().velocity = force * direction;

          other.gameObject.GetComponent<Animator>().SetBool("Dizzy", true);
          other.gameObject.GetComponent<CharactorStats>().TakeDamage(damage);

          state = State.HitNothing;
        }
        break;
      case State.HitEnemy:
        if (other.gameObject.GetComponent<Golem>()) {
          other.gameObject.GetComponent<CharactorStats>().TakeDamage(damage);

          Destroy(gameObject);
          Instantiate(breakEffect, transform.position, quaternion.identity);

          state = State.HitNothing;
        }
        break;
    }
  }
}
