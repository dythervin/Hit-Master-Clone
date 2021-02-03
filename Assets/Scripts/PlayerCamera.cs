using UnityEngine;

public class PlayerCamera : Singleton<PlayerCamera> {
    [Header("Must be child of character")]
    Transform character;
    [SerializeField] Vector3 offset;
   public Camera Cam { get; private set; }

    protected override void Awake() {
        base.Awake();
        offset = transform.localPosition;
        character = transform.parent;
        transform.parent = null;
        Cam = GetComponent<Camera>();
    }

    private void LateUpdate() {
        transform.position = character.position + offset;
    }
}
