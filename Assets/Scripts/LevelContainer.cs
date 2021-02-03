using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelContainer : MonoBehaviour {
    public Wave[] waves = null;
    public Transform playerPos = null;
    [SerializeField] NavMeshSurface navMeshSurface = null;

    public void BuildNavMesh() {
        navMeshSurface.BuildNavMesh();
    }
}
