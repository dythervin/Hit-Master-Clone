using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {
    [SerializeField] List<NPCController> enemies = null;
    [SerializeField] List<NPCController> hostages = null;
    bool enemiesActive = false;
    private void Awake() {
        foreach (var enemy in enemies) {
            enemy.Init(this);
        }
        foreach (var hostage in hostages) {
            hostage.Init(this);
        }
    }
    public void RemoveEnemy(NPCController enemy) {
        enemies.Remove(enemy);
        if (enemies.Count == 0) {
            PlayerController.Instance.MoveToNextPoint();
            foreach (var hostage in hostages) {
                hostage.OnRescue();
            }
        }
    }

    public void AimAtThePlayer() {
        if (!enemiesActive) {
            enemiesActive = true;
            foreach (var item in enemies) {
                item.AimAtThePlayer();
            }
        }
    }

    public void HealthBarsSetActive(bool active) {
        foreach (var enemy in enemies) {
            enemy.HealthBarSetActive(active);
        }
        foreach (var hostage in hostages) {
            hostage.HealthBarSetActive(active);
        }

    }
}
