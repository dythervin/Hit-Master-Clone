using UnityEngine.Events;

public class Events {
    [System.Serializable] public class OnGameStateChangedEvent : UnityEvent<GameStateDef, GameStateDef> { }
}

