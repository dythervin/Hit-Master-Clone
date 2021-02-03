using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] Rigidbody[] rigidbodies = null;
    [SerializeField] Collider[] colliders = null;

    public void SetKinematic(bool active) {
        foreach (var rb in rigidbodies) {
            rb.isKinematic = active;
        }
    }

    public void SetColliderActive(bool active) {
        foreach (var item in colliders) {
            item.enabled = active;
        }
    }

    public void SetTag(string tag) {
        foreach (var item in colliders) {
            item.tag = tag;
        }
    }
}
