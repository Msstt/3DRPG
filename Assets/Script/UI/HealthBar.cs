using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
  private Transform bar;
  private Transform camera_;
  private Image fill;
  private float visibleTimer;
  public GameObject healthBar;
  public Transform head;
  public bool alwaysVisible;
  public float visibleTime;


  private void Awake() {
    var stats = GetComponent<CharactorStats>();
    stats.healthUpdate += OnHealthUpdate;
  }

  private void OnEnable() {
    camera_ = Camera.main.transform;

    foreach (var canvas in FindObjectsOfType<Canvas>()) {
      if (canvas.renderMode == RenderMode.WorldSpace) {
        bar = Instantiate(healthBar, canvas.transform).transform;
        fill = bar.GetChild(0).GetComponent<Image>();
        bar.gameObject.SetActive(alwaysVisible);
      }
    }
  }

  private void OnHealthUpdate(int currentHealth, int maxHealth) {
    if (bar == null || bar.gameObject == null) {
      return;
    }
    if (currentHealth <= 0) {
      Destroy(bar.gameObject);
      return;
    }
    bar.gameObject.SetActive(true);
    fill.fillAmount = (float)currentHealth / maxHealth;
    visibleTimer = visibleTime;
  }

  private void LateUpdate() {
    if (fill != null) {
      bar.position = head.position;
      bar.forward = -camera_.forward;

      if (visibleTimer >= 0 && !alwaysVisible) {
        visibleTimer -= Time.deltaTime;
      } else {
        bar.gameObject.SetActive(false);
      }
    }
  }
}
