using UnityEngine;

public static class ElementEffectAssigner
{
    private const float GoldChancePerPiggyBank = 0.05f;

    public static void AssignRandomEffect(Element element)
    {
        if (element == null) return;

        int piggyBankCount = PlayerData.CountArtifact(ArtifactNameId.PiggyBank);
        float goldChance = Mathf.Clamp01(piggyBankCount * GoldChancePerPiggyBank);
        ElementEffect effect = ElementEffect.None;
        if (Random.value < goldChance) effect |= ElementEffect.Gold;
        if (Random.value < PlayerData.CriticalElementChance) effect |= ElementEffect.Critical;
        element.SetEffect(effect);
    }
}
