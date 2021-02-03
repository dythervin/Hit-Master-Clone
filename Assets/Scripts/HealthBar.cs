using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HealthBar {
    [SerializeField] Image Main = null;
    [SerializeField] Image Minor = null;
    Coroutine healthFallTask;

    [Range(0, 0.5f)]
    readonly float painReduceSpeed = 0.4f;

    private MonoBehaviour mono;
    public float HealthPercent {
        get { return Main.fillAmount; }
        set {
            Main.fillAmount = value;
            healthFallTask = mono.StartCoroutine(HealthFall());
        }
    }
    public void Init(MonoBehaviour mono, float hp) {
        this.mono = mono;
        Main.fillAmount = hp;
        Minor.fillAmount = hp;
    }

    private IEnumerator HealthFall() {
        while (Minor.fillAmount > HealthPercent) {
            Minor.fillAmount -= painReduceSpeed * Time.deltaTime;
            yield return null;
        }
        Minor.fillAmount = HealthPercent;
    }

    public void Disable() {
        //start disable animation
        //animation end
        if (healthFallTask != null)
            mono.StopCoroutine(healthFallTask);
    }
}
