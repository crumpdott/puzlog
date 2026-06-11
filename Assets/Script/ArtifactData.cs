using UnityEngine;

[CreateAssetMenu(fileName = "ArtifactData", menuName = "Scriptable Objects/ArtifactData")]
public class ArtifactData : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public int Cost;
    public string detail;
}
