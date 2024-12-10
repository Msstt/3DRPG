using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour {
  public enum TransitionType {
    SameScene, DiffScene
  }

  public string sceneName;
  public TransitionType type;
  public TransitionDestination.DestinationType destination;
  private bool canTrans;

  private void Update() {
    if (Input.GetKeyDown(KeyCode.E) && canTrans) {
      SceneController.Instance.Transition(this);
    }
  }

  private void OnTriggerStay(Collider other) {
    if (other.CompareTag("Player")) {
      canTrans = true;
    }
  }

  private void OnTriggerExit(Collider other) {
    if (other.CompareTag("Player")) {
      canTrans = false;
    }
  }
}
