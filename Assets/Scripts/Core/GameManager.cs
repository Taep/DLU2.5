using System;
using UnityEngine;
using PlanetDefense.Data;
using PlanetDefense.Utils;

namespace PlanetDefense.Core
{
    public enum GameState
    {
        Idle,
        Playing,
        Paused,
        GameOver,
        StageClear
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("참조")]
        [SerializeField] private StageManager stageManager;
        [SerializeField] private StageData defaultStageData;

        [Header("풀 참조 (재시작 정리용)")]
        [SerializeField] private ObjectPool projectilePool;
        [SerializeField] private ObjectPool damageNumberPool;

        [Header("게임 속도")]
        [SerializeField] private float[] speedOptions = { 1f, 2f };
        private int currentSpeedIndex;

        private GameState currentState = GameState.Idle;

        public GameState CurrentState => currentState;
        public float GameSpeed => speedOptions[currentSpeedIndex];

        public event Action<GameState> OnGameStateChanged;
        public event Action<float> OnGameSpeedChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            // 자동으로 기본 스테이지 시작
            if (defaultStageData != null)
            {
                StartStage(defaultStageData);
            }
        }

        public void StartStage(StageData data)
        {
            SetState(GameState.Playing);
            Time.timeScale = speedOptions[currentSpeedIndex];

            // Subscribe to planet destruction
            if (Planet.Planet.Instance != null)
            {
                Planet.Planet.Instance.OnDestroyed += HandlePlanetDestroyed;
                Planet.Planet.Instance.ResetHP();
            }

            if (stageManager != null)
            {
                stageManager.StartStage(data);
            }
        }

        public void PauseGame()
        {
            if (currentState != GameState.Playing) return;
            SetState(GameState.Paused);
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            if (currentState != GameState.Paused) return;
            SetState(GameState.Playing);
            Time.timeScale = speedOptions[currentSpeedIndex];
        }

        public void GameOver()
        {
            SetState(GameState.GameOver);
            Time.timeScale = 0f;

            if (stageManager != null)
                stageManager.StopStage();
        }

        public void StageClear()
        {
            SetState(GameState.StageClear);
            Time.timeScale = 0f;

            if (stageManager != null)
                stageManager.StopStage();
        }

        public void ToggleSpeed()
        {
            currentSpeedIndex = (currentSpeedIndex + 1) % speedOptions.Length;
            Time.timeScale = speedOptions[currentSpeedIndex];
            OnGameSpeedChanged?.Invoke(speedOptions[currentSpeedIndex]);
        }

        public void RestartStage()
        {
            Time.timeScale = 1f;
            currentSpeedIndex = 0;

            // 발사체/데미지넘버 풀 정리
            if (projectilePool != null) projectilePool.ReturnAll();
            if (damageNumberPool != null) damageNumberPool.ReturnAll();

            if (defaultStageData != null)
                StartStage(defaultStageData);
        }

        private void HandlePlanetDestroyed()
        {
            GameOver();
        }

        private void SetState(GameState newState)
        {
            if (currentState == newState) return;
            currentState = newState;
            OnGameStateChanged?.Invoke(newState);
        }

        private void OnDestroy()
        {
            if (Planet.Planet.Instance != null)
                Planet.Planet.Instance.OnDestroyed -= HandlePlanetDestroyed;

            if (Instance == this)
                Instance = null;
        }
    }
}