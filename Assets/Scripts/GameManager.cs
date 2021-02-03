using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStateDef { Idle, Running, Paused, Over, Win }
public class GameManager : Singleton<GameManager> {
    [SerializeField] GameObject[] systems = null;
    [SerializeField, ReadOnly] GameStateDef gameState;
    public Events.OnGameStateChangedEvent onGameStateChanged;
    [SerializeField] LevelContainer[] levels = null;
    int currentLevelIndex = -1;
    LevelContainer currentLevel;
    public GameStateDef GameState => gameState;

    protected override void Awake() {
        base.Awake();
        foreach (var system in systems) {
            Instantiate(system, transform);
        }

        SetUpNextLevel();
    }

    private void SetUpNextLevel() {
        if (currentLevel != null)
            Destroy(currentLevel.gameObject);

        if (++currentLevelIndex >= levels.Length)
            currentLevelIndex = 0;

        currentLevel = Instantiate(levels[currentLevelIndex]);
        currentLevel.BuildNavMesh();
        PlayerController.Instance.waves = currentLevel.waves;
        PlayerController.Instance.Init(currentLevel.playerPos.position);
    }

    public void UpdateGameState(GameStateDef target) {
        switch (gameState) {
            case GameStateDef.Idle:
                break;
            case GameStateDef.Running:
                break;
            case GameStateDef.Paused:
                break;
            case GameStateDef.Over:
                break;
            case GameStateDef.Win:
                break;
        }
        onGameStateChanged?.Invoke(gameState, target);
        gameState = target;
        switch (target) {
            case GameStateDef.Idle:
                break;
            case GameStateDef.Running:
                break;
            case GameStateDef.Paused:
                break;
            case GameStateDef.Over:
                currentLevelIndex--;
                StartCoroutine(SetUpNextLevel(1));
                break;
            case GameStateDef.Win:
                StartCoroutine(SetUpNextLevel(1));
                break;
        }
    }

    private IEnumerator SetUpNextLevel(float delay) {
        yield return new WaitForSeconds(delay);
        SetUpNextLevel();
    }

}

public interface IOnGameStateChanged {
    void OnGameStateChanged(GameStateDef previous, GameStateDef target);
}
