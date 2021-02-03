using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : Singleton<PlayerController>, IOnGameStateChanged {
    [SerializeField] Transform gunpoint = null;
    [SerializeField] Projectile projectilePrefab = null;
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] Animator animator = null;
    [SerializeField] CharacterAnimationProperties_SO animProperties = null;
    [SerializeField] Ragdoll ragdoll = null;
    public Wave[] waves;
    [SerializeField, ReadOnly] CharStateDef charState = CharStateDef.Idle;
    [SerializeField] float damage = 20;
    [SerializeField, Range(0.1f, 0.9f)] float damageVariance = 0.5f;

    float AnimWalkThreshHold;
    float AnimMaxMoveSpeed;

    static readonly int moveSpeedHash = Animator.StringToHash("moveSpeed");
    static readonly int movingHash = Animator.StringToHash("Moving");
    static readonly int moveAnimSpeedHash = Animator.StringToHash("moveAnimSpeed");

    Camera cam;
    ObjectPooler<Projectile> projectilePooler;
    int currentWave = -1;

    protected override void Awake() {
        base.Awake();
        projectilePooler = new ObjectPooler<Projectile>(projectilePrefab);
        cam = Camera.main;
        AnimWalkThreshHold = animProperties.walk / animProperties.run;
        AnimMaxMoveSpeed = agent.speed / animProperties.run;
        ragdoll.SetKinematic(true);
    }

    public void Init(Vector3 position) {
        agent.Warp(position);
        currentWave = -1;
        charState = CharStateDef.Aiming;
        GameManager.Instance.onGameStateChanged.AddListener(OnGameStateChanged);
    }

    public void MoveToNextPoint() {
        if (currentWave >= 0)
            waves[currentWave].HealthBarsSetActive(false);

        if (++currentWave >= waves.Length) {
            return;
        }

        agent.SetDestination(waves[currentWave].transform.position);
        StartCoroutine(Moving());
    }

    IEnumerator Moving() {
        charState = CharStateDef.Moving;
        while (agent.velocity.Equals(Vector3.zero))
            yield return null;

        while (agent.velocity != Vector3.zero) {
            animator.SetBool(movingHash, true);
            float moveSpeed = agent.velocity.magnitude / agent.speed;
            animator.SetFloat(moveSpeedHash, moveSpeed);
            if (moveSpeed < AnimWalkThreshHold) {
                animator.SetFloat(moveAnimSpeedHash, moveSpeed * AnimMaxMoveSpeed / AnimWalkThreshHold);
            } else {
                animator.SetFloat(moveAnimSpeedHash, AnimMaxMoveSpeed);
            }
            yield return null;

        }
        animator.SetBool(movingHash, false);
        if (currentWave + 1 >= waves.Length) {
            GameManager.Instance.UpdateGameState(GameStateDef.Win);
        } else
            waves[currentWave].HealthBarsSetActive(true);
        charState = CharStateDef.Aiming;
    }



    private void Update() {
        if (charState == CharStateDef.Aiming) {
            if (Input.GetMouseButtonDown(0)) {
                TurnToTargetAndShoot(Input.mousePosition);
            } else if (Input.touchCount > 0) {
                TurnToTargetAndShoot(Input.GetTouch(0).position);
            }
        }
    }

    private void TurnToTargetAndShoot(Vector3 screenPos) {
        if (currentWave == -1) {
            MoveToNextPoint();
            return;
        }
        Ray ray = cam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            if (!hit.collider.CompareTag(Tags.player)) {
                Vector3 direction = hit.point - transform.position;
                direction.y = transform.position.y;
                transform.rotation = Quaternion.LookRotation(direction);
                Shoot(hit.point);
            }
        }
    }
    private void Shoot(Vector3 target) {
        Quaternion rotation = Quaternion.LookRotation(target - gunpoint.position);
        Projectile projectile = projectilePooler.GetPooledObject(gunpoint.position, rotation, true);
        projectile.Launch(damage * Random.Range(1 - damageVariance, 1 + damageVariance));
    }

    public void OnGameStateChanged(GameStateDef previous, GameStateDef target) {
        switch (target) {
            case GameStateDef.Over:
                charState = CharStateDef.Idle;
                break;
        }
    }
}
