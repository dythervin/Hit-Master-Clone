using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {
    [SerializeField] Transform healthParent = null;
    [SerializeField] CharUI charUIPrefab = null;
    [SerializeField] RectTransform rectTransform = null;
    Camera cam;
    private readonly HashSet<CharUI> charUIs = new HashSet<CharUI>();

    protected override void Awake() {
        base.Awake();
        cam = Camera.main;
    }

    public Vector3 GetCanvasPos(Vector3 worldSpacePos, float offsetHeightPercent = 0.01f) {
        Vector2 screenPoint = cam.WorldToScreenPoint(worldSpacePos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, null, out Vector2 canvasPos);
        canvasPos.y += offsetHeightPercent * rectTransform.rect.height;
        return canvasPos;
    }

    public CharUI GetCharUI() {
        CharUI charUI = Instantiate(charUIPrefab, healthParent);
        charUIs.Add(charUI);
        return charUI;
    }

    public void DestroyCharUI(CharUI charUI) {
        charUIs.Remove(charUI);
        charUI.Disable();
    }
    private void Update() {
        foreach (var charUI in charUIs) {
            charUI.FollowTarget();
        }
    }
}



