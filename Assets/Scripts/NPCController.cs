using System.Collections;
using UnityEngine;

public enum CharacterTypeDef { Player, Enemy, Hostage }
public enum CharStateDef { Idle, Aiming, Moving, Dead }
public class NPCController : MonoBehaviour {
    [SerializeField] CharacterTypeDef characterType = CharacterTypeDef.Enemy;
    [SerializeField] Animator animator = null;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float rotationSpeed = 10;
    [SerializeField, ReadOnly] CharStateDef charState = CharStateDef.Idle;
    [SerializeField] Ragdoll ragdoll = null;
    [SerializeField] float randomIdleAnimationDelay = 10;


    float currentHealth;
    CharUI charUI;
    Wave wave;
    private void Awake() {
        ragdoll.SetKinematic(true);
        randomIdleDelay = new WaitForSeconds(randomIdleAnimationDelay);
        StartCoroutine(RandomIdle());
        currentHealth = maxHealth;
        charUI = UIManager.Instance.GetCharUI();
        charUI.Init(transform, 1);
        charUI.gameObject.SetActive(false);
    }
    public void Init(Wave wave) {
        this.wave = wave;
    }


    WaitForSeconds randomIdleDelay;
    static readonly int randomIdleHash = Animator.StringToHash("RandomIdle");
    static readonly int idleIdHash = Animator.StringToHash("IdleId");
    static readonly int rescueHash = Animator.StringToHash("Rescue");
    static readonly int aimHash = Animator.StringToHash("Aim");



    public void OnHit(float damage, Rigidbody rb, Vector3 direction) {
        if (currentHealth <= 0)
            return;

        currentHealth -= damage;
        charUI.HealthBar.HealthPercent = currentHealth / maxHealth;
        wave.AimAtThePlayer();
        if (currentHealth <= 0) {
            animator.enabled = false;
            ragdoll.SetKinematic(false);
            rb.AddForce(direction, ForceMode.Impulse);
            Destroy(gameObject, 10);
            charState = CharStateDef.Dead;
            switch (characterType) {
                case CharacterTypeDef.Enemy:
                    wave?.RemoveEnemy(this);
                    wave = null;
                    break;
                case CharacterTypeDef.Hostage:
                    GameManager.Instance.UpdateGameState(GameStateDef.Over);
                    wave.HealthBarsSetActive(false);
                    break;
            }
            UIManager.Instance.DestroyCharUI(charUI);
        }
    }

    public void HealthBarSetActive(bool active) {
        charUI.gameObject.SetActive(active);
    }

    private IEnumerator RandomIdle() {
        yield return new WaitForSeconds(Random.Range(0, randomIdleAnimationDelay));
        while (charState == CharStateDef.Idle) {
            animator.SetFloat(idleIdHash, Random.Range(0, 2));
            animator.SetTrigger(randomIdleHash);
            yield return randomIdleDelay;
        }
    }

    public void OnRescue() {
        animator.SetTrigger(rescueHash);
    }
    public void AimAtThePlayer() {
        animator.SetTrigger(aimHash);
        charState = CharStateDef.Aiming;
        StartCoroutine(TurnToPlayer());
    }

    IEnumerator TurnToPlayer() {
        Vector3 direction = (PlayerController.Instance.transform.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        while (transform.rotation != lookRotation) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }
    }
}
