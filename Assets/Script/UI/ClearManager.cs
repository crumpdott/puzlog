using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearManager : MonoBehaviour
{
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI ArtifactText;
    public Button TitleButton;

    private void Start()
    {
        MoneyText.text = $"所持金: {PlayerData.Money}円";
        ArtifactText.text = PlayerData.Artifacts.Count == 0
            ? "アーティファクト: なし"
            : "アーティファクト\n" + string.Join("\n", PlayerData.Artifacts
                .GroupBy(id => id)
                .Select(group => $"{GetName(group.Key)} ×{group.Count()}"));
        TitleButton.onClick.AddListener(() => SceneManager.LoadScene("Title"));
    }

    private static string GetName(ArtifactNameId id)
    {
        return id switch
        {
            ArtifactNameId.PiggyBank => "豚の貯金箱",
            ArtifactNameId.Fireball => "ファイヤボール",
            ArtifactNameId.CrackedCrystalBall => "割れた水晶玉",
            ArtifactNameId.Spinach => "ほうれん草",
            _ => id.ToString(),
        };
    }
}
