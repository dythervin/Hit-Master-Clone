using System;
using System.Collections;
using UnityEngine;


public class Projectile : MonoBehaviour {

    [SerializeField] float lifetime = 5;
    [SerializeField] Rigidbody rb = null;
    [SerializeField] new Collider collider = null;
    [SerializeField] float launchForce = 5;
    [SerializeField] float impactForce = 10;
    [SerializeField] TrailRenderer trailRenderer = null;

    Coroutine projectileLifetime;
    WaitForSeconds waitLifetime;
    Vector3 direction;
    float damage;
    bool active = false;
    private void Awake() {
        waitLifetime = new WaitForSeconds(lifetime);
    }
    public void Launch(float damage) {
        trailRenderer.emitting = true;
        this.damage = damage;
        rb.useGravity = false;
        active = true;
        projectileLifetime = StartCoroutine(DisableDelayed());
        direction = transform.forward.normalized;
        rb.isKinematic = false;
        rb.velocity = direction * launchForce;
        rb.angularVelocity = Vector3.zero;

    }
    private IEnumerator DisableDelayed() {
        yield return waitLifetime;
        projectileLifetime = StartCoroutine(DisableDelayed(1));
    }
    private IEnumerator DisableDelayed(float sec) {
        yield return new WaitForSeconds(sec);
        gameObject.SetActive(false);
        projectileLifetime = null;
    }

    private void OnCollisionEnter(Collision collision) {
        if (!active)
            return;


        trailRenderer.emitting = false;
        active = false;
        rb.useGravity = true;
        collider.enabled = false;
        if (collision.collider.CompareTag(Tags.hittable) || collision.collider.CompareTag(Tags.hittableHead)) {
            NPCController target = collision.gameObject.GetComponentInParent<NPCController>();
            direction *= impactForce;
            direction.y += 1;
            if (collision.collider.CompareTag(Tags.hittableHead)) {
                damage *= 3;
            }
            target.OnHit(damage, collision.rigidbody, direction);
        }

        if (projectileLifetime != null)
            StopCoroutine(projectileLifetime);
        projectileLifetime = StartCoroutine(DisableDelayed(1));
    }
}

