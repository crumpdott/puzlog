using System.Collections.Generic;

public static class PlayerData
{
    public const int LastRound = 20;
    public static int Money = 30;
    public static int Round = 1;
    public static int ReloadCountInCurrentRound = 0;
    public static int PiggyBankTmp = 0;
    public static List<ArtifactNameId> Artifacts = new List<ArtifactNameId>();
    public static ArtifactData[] KeptOffers = new ArtifactData[3];
    public static bool[] KeptSlots = new bool[3];
    public static ArtifactData[] ShopOffers = new ArtifactData[3];
    public static float BaseAttackPower = 0f;
    public static float[] AttributeAttackPower = new float[4];
    public static float IncomeMultiplier = 1f;
    public static float CriticalMultiplier = 2f;
    public static float CriticalElementChance = 0.001f;
    public static float ElementConnectRange = 1f;

    public static void StartNewRun()
    {
        Money = 30;
        Round = 1;
        ReloadCountInCurrentRound = 0;
        PiggyBankTmp = 0;
        Artifacts.Clear();
        KeptOffers = new ArtifactData[3];
        KeptSlots = new bool[3];
        ShopOffers = new ArtifactData[3];
        BaseAttackPower = 0f;
        AttributeAttackPower = new float[4];
        IncomeMultiplier = 1f;
        CriticalMultiplier = 2f;
        CriticalElementChance = 0.001f;
        ElementConnectRange = 1f;
    }

    public static int CountArtifact(ArtifactNameId nameId)
    {
        int count = 0;
        foreach (var artifact in Artifacts)
        {
            if (artifact == nameId)
            {
                count++;
            }
        }

        return count;
    }

    public static void AddArtifact(ArtifactNameId nameId)
    {
        Artifacts.Add(nameId);
        switch (nameId)
        {
            case ArtifactNameId.Fireball:
                AttributeAttackPower[(int)ElementAttribute.Fire] += 2f;
                AttributeAttackPower[(int)ElementAttribute.Water] -= 1f;
                break;
            case ArtifactNameId.CrackedCrystalBall:
                AttributeAttackPower[(int)ElementAttribute.Water] += 2f;
                AttributeAttackPower[(int)ElementAttribute.Wood] -= 1f;
                break;
            case ArtifactNameId.Spinach:
                AttributeAttackPower[(int)ElementAttribute.Wood] += 2f;
                AttributeAttackPower[(int)ElementAttribute.Fire] -= 1f;
                break;
            case ArtifactNameId.GoldClock:
                IncomeMultiplier += 0.5f;
                break;
        }
    }
}
