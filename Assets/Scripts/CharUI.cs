using System.Collections;
using TMPro;
using UnityEngine;

public class CharUI : MonoBehaviour {
    Transform target;
    [SerializeField] float offsetHeight = 0.05f;
    [SerializeField] RectTransform rectTransform = null;
    [SerializeField] HealthBar healthBar = null;
    public HealthBar HealthBar => healthBar;

    [SerializeField] TextMeshProUGUI popUpText = null;
    [SerializeField] Animation fadeOut = null;
    [SerializeField] Animation popUpTextFadeOut = null;
    CanvasGroup[] canvasGroups;

    float popUpFadeOutTime;

    private void Awake() {
        canvasGroups = GetComponentsInChildren<CanvasGroup>();
    }

    public void Init(Transform target, float hp) {
        this.target = target;
        healthBar.Init(this, hp);
        transform.localScale = Vector3.one;
        foreach (var item in canvasGroups) {
            item.alpha = 1;
        }
    }
    public void Disable() {
        fadeOut.Play();
    }
    public void DisableAnim() {
        healthBar.Disable();
        gameObject.SetActive(false);
    }


    public void SetPopUpText(string text, float duration) {
        if (duration <= 0)
            return;

        popUpFadeOutTime = Time.time + duration * 0.7f;
        StartCoroutine(SetPopUpTextTask(text));
    }

    private IEnumerator SetPopUpTextTask(string text) {
        popUpText.enabled = true;
        popUpText.SetText(text);
        popUpText.gameObject.SetActive(true);
        while (popUpFadeOutTime > Time.time)
            yield return new WaitForSeconds(Time.time - popUpFadeOutTime);
        //this.popUpText.gameObject.SetActive(false);
        popUpTextFadeOut.Play();
    }

    public void FollowTarget() {
        if (target != null)
            rectTransform.localPosition = UIManager.Instance.GetCanvasPos(target.position, offsetHeight);

    }
}

