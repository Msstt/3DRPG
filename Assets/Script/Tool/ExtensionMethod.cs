using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod {
  private const float dotThresold = 0.5f;

  public static bool IsFacingTarget(this Transform transform, Transform target) {
    var toTarget = target.position - transform.position;
    toTarget.Normalize();
    return Vector3.Dot(toTarget, transform.forward) > dotThresold;
  }
}
