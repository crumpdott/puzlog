using UnityEngine;

public static class ElementEffectAssigner
{
    private const float GoldChancePerPiggyBank = 0.05f;

    public static void AssignRandomEffect(Element element)
    {
        if (element == null) return;

        int piggyBankCount = PlayerData.CountArtifact(ArtifactNameId.PiggyBank);
        float goldChance = Mathf.Clamp01(piggyBankCount * GoldChancePerPiggyBank);

        if (Random.value < goldChance)
        {
            element.Effect = ElementEffect.Gold;
        }
    }
}
