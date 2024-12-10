using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : Singleton<SceneController> {
  public void Transition(TransitionPoint point) {
    switch (point.type) {
      case TransitionPoint.TransitionType.SameScene:
        StartCoroutine(Transition(SceneManager.GetActiveScene().name, point.destination));
        break;
      case TransitionPoint.TransitionType.DiffScene:
        break;
    }
  }

  private IEnumerator Transition(string sceneName, TransitionDestination.DestinationType destination) {
    var dest = FindDestination(destination);
    GameObject player = GameManager.Instance.charactorStats.gameObject;
    player.transform.SetLocalPositionAndRotation(dest.transform.position, dest.transform.rotation);
    player.GetComponent<NavMeshAgent>().destination = player.transform.position;
    yield return null;
  }

  private TransitionDestination FindDestination(TransitionDestination.DestinationType destination) {
    var destinations = FindObjectsOfType<TransitionDestination>();
    for (int i = 0; i < destinations.Length; i++) {
      if (destinations[i].type == destination) {
        return destinations[i];
      }
    }
    return null;
  }
}
