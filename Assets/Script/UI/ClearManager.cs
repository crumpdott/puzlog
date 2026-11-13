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
            ArtifactNameId.GoldClock => "金の置き時計",
            ArtifactNameId.CreditCard => "クレジットカード",
            ArtifactNameId.FingerCot => "指サック",
            ArtifactNameId.Hourglass => "砂時計",
            ArtifactNameId.PickDriver => "ピックが付いた電動ドライバー",
            ArtifactNameId.FireFlower => "ファイヤーフラワー",
            ArtifactNameId.RainCloud => "雨雲",
            ArtifactNameId.Shiitake => "しいたけ",
            ArtifactNameId.Guitar => "ギター",
            ArtifactNameId.InkSquid => "インクまみれのイカ",
            ArtifactNameId.Acorn => "どんぐり",
            ArtifactNameId.TangledUsbHub => "絡まったUSBハブ",
            ArtifactNameId.ColdSaunaStone => "冷えたサウナストーン",
            ArtifactNameId.SpinachCoffee => "ほうれん草と珈琲のセット",
            ArtifactNameId.ChattyGlasses => "おしゃべりめがね",
            ArtifactNameId.Peace => "PEACE",
            ArtifactNameId.LoneSock => "かたっぽの靴下",
            ArtifactNameId.OlderBrotherHat => "兄の帽子",
            ArtifactNameId.YoungerBrotherHat => "弟の帽子",
            ArtifactNameId.FadedPrint => "色あせた錦絵",
            ArtifactNameId.ArtistManga => "とある絵師の漫画",
            ArtifactNameId.ApplePieCoffee => "アップルパイと珈琲のセット",
            ArtifactNameId.Dynamite => "ダイナマイト",
            ArtifactNameId.CursedSword => "呪われた剣",
            ArtifactNameId.DebtNote => "債務書",
            ArtifactNameId.Trinitrotoluene => "トリニトロトルエン",
            _ => id.ToString(),
        };
    }
}
