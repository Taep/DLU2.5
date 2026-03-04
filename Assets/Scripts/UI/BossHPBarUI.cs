using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlanetDefense.Core;
using PlanetDefense.Enemy;

namespace PlanetDefense.UI
{
    public class BossHPBarUI : MonoBehaviour
    {
        [Header("UI 요소")]
        [SerializeField] private GameObject bossHPBarPanel;
        [SerializeField] private Slider bossHPSlider;
        [SerializeField] private TextMeshProUGUI bossNameText;
        [SerializeField] private TextMeshProUGUI bossHPText;

        private BossEnemy currentBoss;

        private void Start()
        {
            // 기본 비활성
            if (bossHPBarPanel != null)
                bossHPBarPanel.SetActive(false);

            if (StageManager.Instance != null)
            {
                StageManager.Instance.OnBossPhaseStarted += HandleBossSpawned;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            }
        }

        private void HandleBossSpawned(BossEnemy boss)
        {
            if (boss == null) return;

            currentBoss = boss;
            currentBoss.OnBossHPChanged += UpdateBossHP;
            currentBoss.OnDied += HandleBossDied;

            if (bossHPBarPanel != null)
                bossHPBarPanel.SetActive(true);

            // 보스 이름 설정
            if (bossNameText != null && StageManager.Instance != null
                && StageManager.Instance.CurrentStageData != null
                && StageManager.Instance.CurrentStageData.bossData != null)
            {
                bossNameText.text = StageManager.Instance.CurrentStageData.bossData.enemyName;
            }

            // HP 초기화
            UpdateBossHP(boss.CurrentHP, boss.MaxHP);
        }

        private void UpdateBossHP(float current, float max)
        {
            if (bossHPSlider != null)
            {
                bossHPSlider.maxValue = max;
                bossHPSlider.value = current;
            }

            if (bossHPText != null)
            {
                bossHPText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
            }
        }

        private void HandleBossDied(EnemyBase boss)
        {
            CleanupBoss();

            if (bossHPBarPanel != null)
                bossHPBarPanel.SetActive(false);
        }

        private void HandleGameStateChanged(GameState state)
        {
            if (state == GameState.GameOver || state == GameState.StageClear)
            {
                CleanupBoss();
                if (bossHPBarPanel != null)
                    bossHPBarPanel.SetActive(false);
            }
        }

        private void CleanupBoss()
        {
            if (currentBoss != null)
            {
                currentBoss.OnBossHPChanged -= UpdateBossHP;
                currentBoss.OnDied -= HandleBossDied;
                currentBoss = null;
            }
        }

        private void OnDestroy()
        {
            CleanupBoss();

            if (StageManager.Instance != null)
            {
                StageManager.Instance.OnBossPhaseStarted -= HandleBossSpawned;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
            }
        }
    }
}