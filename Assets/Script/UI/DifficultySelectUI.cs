using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultySelectUI : MonoBehaviour
{
    [SerializeField] private Button[] difficultyButtons;
    [SerializeField] private Button confirmButton;
    [SerializeField] private GameObject difficultyDetailPanel;
    [SerializeField] private TMP_Text difficultyNameText;
    [SerializeField] private TMP_Text difficultyDescriptionText;

    private UnityAction[] selectActions;

    private void Start()
    {
        selectActions = new UnityAction[difficultyButtons.Length];
        for (int i = 0; i < difficultyButtons.Length; i++)
        {
            int difficulty = i;
            selectActions[i] = () => SelectDifficulty(difficulty);
            difficultyButtons[i].onClick.AddListener(selectActions[i]);
        }

        confirmButton.onClick.AddListener(OpenGame);
        confirmButton.interactable = false;
        difficultyDetailPanel.SetActive(false);
        for (int i = 1; i < difficultyButtons.Length; i++) difficultyButtons[i].interactable = false;
    }

    private void OnDestroy()
    {
        if (selectActions == null)
        {
            return;
        }

        for (int i = 0; i < difficultyButtons.Length; i++)
        {
            difficultyButtons[i].onClick.RemoveListener(selectActions[i]);
        }

        confirmButton.onClick.RemoveListener(OpenGame);
    }

    private void SelectDifficulty(int difficulty)
    {
        for (int i = 0; i < difficultyButtons.Length; i++)
        {
            difficultyButtons[i].image.color = i == difficulty
                ? new Color(0.26f, 0.43f, 0.59f)
                : new Color(0.12f, 0.17f, 0.23f);
        }

        bool hasDefinedDescription = difficulty == 0;
        difficultyDetailPanel.SetActive(hasDefinedDescription);
        confirmButton.interactable = true;
        if (hasDefinedDescription)
        {
            difficultyNameText.text = "難易度0";
            difficultyDescriptionText.text = "通常難易度";
        }
    }

    private void OpenGame()
    {
        PlayerData.StartNewRun();
        SceneManager.LoadScene("Puzzle");
    }
}
