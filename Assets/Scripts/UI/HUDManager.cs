using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlanetDefense.Core;
using PlanetDefense.Planet;

namespace PlanetDefense.UI
{
    public class HUDManager : MonoBehaviour
    {
        [Header("행성 HP")]
        [SerializeField] private Slider planetHPBar;
        [SerializeField] private TextMeshProUGUI planetHPText;

        [Header("스테이지 정보")]
        [SerializeField] private TextMeshProUGUI stageNameText;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI levelText;

        [Header("배속")]
        [SerializeField] private Button speedButton;
        [SerializeField] private TextMeshProUGUI speedButtonText;

        private void Start()
        {
            SubscribeEvents();
            InitializeUI();
        }

        private void SubscribeEvents()
        {
            if (Planet.Planet.Instance != null)
            {
                Planet.Planet.Instance.OnHPChanged += UpdatePlanetHP;
            }

            if (StageManager.Instance != null)
            {
                StageManager.Instance.OnWaveChanged += UpdateWaveInfo;
                StageManager.Instance.OnLevelUp += UpdateLevel;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameSpeedChanged += UpdateSpeedButton;
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            }

            if (speedButton != null)
            {
                speedButton.onClick.AddListener(OnSpeedButtonClicked);
            }
        }

        private void InitializeUI()
        {
            // 행성 HP 초기화
            if (Planet.Planet.Instance != null)
            {
                UpdatePlanetHP(Planet.Planet.Instance.CurrentHP, Planet.Planet.Instance.MaxHP);
            }

            // 스테이지명 초기화
            if (StageManager.Instance != null && StageManager.Instance.CurrentStageData != null)
            {
                if (stageNameText != null)
                    stageNameText.text = StageManager.Instance.CurrentStageData.stageName;
            }

            // 배속 버튼 초기화
            UpdateSpeedButton(1f);

            // 레벨 초기화
            UpdateLevel(1);
        }

        private void Update()
        {
            UpdateTimer();
        }

        private void UpdatePlanetHP(float current, float max)
        {
            if (planetHPBar != null)
            {
                planetHPBar.maxValue = max;
                planetHPBar.value = current;
            }

            if (planetHPText != null)
            {
                planetHPText.text = Mathf.CeilToInt(current).ToString();
            }
        }

        private void UpdateWaveInfo(int currentWave, int totalWaves)
        {
            if (waveText != null)
            {
                waveText.text = $"Wave {currentWave}/{totalWaves}";
            }
        }

        private void UpdateTimer()
        {
            if (timerText == null || StageManager.Instance == null) return;

            float time = StageManager.Instance.StageTimer;
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        private void UpdateLevel(int level)
        {
            if (levelText != null)
            {
                levelText.text = $"Lv.{level}";
            }
        }

        private void UpdateSpeedButton(float speed)
        {
            if (speedButtonText != null)
            {
                speedButtonText.text = $"X{speed:0}";
            }
        }

        private void OnSpeedButtonClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ToggleSpeed();
            }
        }

        private void HandleGameStateChanged(GameState state)
        {
            // HUD는 Playing 상태에서만 타이머 업데이트
            // 다른 상태 처리는 GameOverPanel/StageClearPanel에서 담당
        }

        private void OnDestroy()
        {
            if (Planet.Planet.Instance != null)
            {
                Planet.Planet.Instance.OnHPChanged -= UpdatePlanetHP;
            }

            if (StageManager.Instance != null)
            {
                StageManager.Instance.OnWaveChanged -= UpdateWaveInfo;
                StageManager.Instance.OnLevelUp -= UpdateLevel;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameSpeedChanged -= UpdateSpeedButton;
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
            }

            if (speedButton != null)
            {
                speedButton.onClick.RemoveListener(OnSpeedButtonClicked);
            }
        }
    }
}