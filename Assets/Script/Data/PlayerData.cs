using System.Collections.Generic;

public static class PlayerData
{
    public static int Money = 0;
    public static int Round = 1;
    public static int ReloadCountInCurrentRound = 0;
    public static int PiggyBankTmp = 0;
    public static List<ArtifactNameId> Artifacts = new List<ArtifactNameId>();

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
}
