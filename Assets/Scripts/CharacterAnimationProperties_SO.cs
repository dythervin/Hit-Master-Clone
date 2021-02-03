using UnityEngine;

[CreateAssetMenu(fileName = "new AP char", menuName = "Character/Animation Properties", order = 1)]
public class CharacterAnimationProperties_SO : ScriptableObject
{
    [UnityEngine.Header("Meters per clip:")]
    public float walk;
    public float run;
    public float sprint;
}
