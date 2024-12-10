using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseManager : Singleton<MouseManager> {
  [Header("鼠标指针")]
  public Texture2D point;
  public Texture2D doorway;
  public Texture2D attack;
  public Texture2D target;
  public Texture2D arrow;

  public event Action<Vector3> OnMouseClicked;
  public event Action<GameObject> OnEnemyClicked;
  private RaycastHit hitInfo;

  private void Update() {
    SetCursorTexture();
    MouseControl();
  }

  private void SetCursorTexture() {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out hitInfo)) {
      switch (hitInfo.collider.tag) {
        case "Ground":
          Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
          break;
        case "Enemy":
          Cursor.SetCursor(attack, new Vector2(0, 0), CursorMode.Auto);
          break;
        case "Attackable":
          Cursor.SetCursor(attack, new Vector2(0, 0), CursorMode.Auto);
          break;
        case "Patrol":
          Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
          break;
      }
    }
  }

  private void MouseControl() {
    if (!Input.GetMouseButtonDown(0) || hitInfo.collider == null) {
      return;
    }
    if (hitInfo.collider.CompareTag("Ground")) {
      OnMouseClicked?.Invoke(hitInfo.point);
    }
    if (hitInfo.collider.CompareTag("Enemy")) {
      OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
    }
    if (hitInfo.collider.CompareTag("Attackable")) {
      OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
    }
    if (hitInfo.collider.CompareTag("Patrol")) {
      OnMouseClicked?.Invoke(hitInfo.point);
    }
  }

}
