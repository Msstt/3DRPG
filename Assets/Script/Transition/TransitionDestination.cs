using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDestination : MonoBehaviour {
  public enum DestinationType {
    ENTER, A, B, C
  }

  public DestinationType type;
}
