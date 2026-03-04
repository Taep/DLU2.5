using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlanetDefense.Core;

namespace PlanetDefense.UI
{
    public class ResultPanelUI : MonoBehaviour
    {
        [Header("패널")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject stageClearPanel;

        [Header("Game Over")]
        [SerializeField] private TextMeshProUGUI gameOverTitleText;
        [SerializeField] private Button gameOverRetryButton;

        [Header("Stage Clear")]
        [SerializeField] private TextMeshProUGUI stageClearTitleText;
        [SerializeField] private TextMeshProUGUI stageClearStatsText;
        [SerializeField] private Button stageClearRetryButton;

        private void Start()
        {
            // 기본 비활성
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (stageClearPanel != null) stageClearPanel.SetActive(false);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            }

            if (gameOverRetryButton != null)
                gameOverRetryButton.onClick.AddListener(OnRetryClicked);

            if (stageClearRetryButton != null)
                stageClearRetryButton.onClick.AddListener(OnRetryClicked);
        }

        private void HandleGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.GameOver:
                    ShowGameOver();
                    break;
                case GameState.StageClear:
                    ShowStageClear();
                    break;
                case GameState.Playing:
                    HideAll();
                    break;
            }
        }

        private void ShowGameOver()
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);

            if (gameOverTitleText != null)
                gameOverTitleText.text = "GAME OVER";
        }

        private void ShowStageClear()
        {
            if (stageClearPanel != null)
                stageClearPanel.SetActive(true);

            if (stageClearTitleText != null)
                stageClearTitleText.text = "STAGE CLEAR!";

            if (stageClearStatsText != null && StageManager.Instance != null)
            {
                float time = StageManager.Instance.StageTimer;
                int minutes = Mathf.FloorToInt(time / 60f);
                int seconds = Mathf.FloorToInt(time % 60f);
                int level = StageManager.Instance.CurrentLevel;

                stageClearStatsText.text = $"Time: {minutes:00}:{seconds:00}\nLevel: {level}";
            }
        }

        private void HideAll()
        {
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (stageClearPanel != null) stageClearPanel.SetActive(false);
        }

        private void OnRetryClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RestartStage();
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
            }

            if (gameOverRetryButton != null)
                gameOverRetryButton.onClick.RemoveListener(OnRetryClicked);

            if (stageClearRetryButton != null)
                stageClearRetryButton.onClick.RemoveListener(OnRetryClicked);
        }
    }
}