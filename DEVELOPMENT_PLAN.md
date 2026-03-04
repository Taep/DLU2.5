# Planet Defense Game - MVP 프로토타입 구현 계획

> **품질 기준**: 상용 서비스 수준의 코드 품질 (클린 아키텍처, 견고한 에러 처리, 확장성 고려)

## Context
빈 Unity 프로젝트(2022.3.62f3)에서 모바일 행성 방어 게임 MVP를 구현합니다.
현재 상태: Unity MCP 연결 완료, SampleScene만 존재, 커스텀 스크립트/프리팹/머티리얼 없음.

---

## Phase 0: 프로젝트 기반 설정 ✅
- [x] URP 패키지 설치 + 렌더 파이프라인 에셋 생성/할당
- [x] 폴더 구조 생성: Scripts/(Core,Planet,Ship,Enemy,Combat,UI,Data,Utils), Prefabs/, ScriptableObjects/, Materials/
- [x] GameScene 생성 + 카메라 설정 (Orthographic, size=10, Z=-10)
- [x] 조명 설정 (Directional Light 푸른톤 + Point Light 따뜻한톤)
- [x] Physics Layer 등록: Planet(6), Ship(7), Enemy(8), PlayerProjectile(9), EnemyProjectile(10)
- [x] Collision Matrix 설정

## Phase 1: 행성 시스템 ✅
- [x] ScriptableObject 정의 (ShipData, EnemyData, StageData)
- [x] ObjectPool 유틸리티
- [x] Planet.cs (HP, 자전, TakeDamage, 이벤트)
- [x] 머티리얼 (M_Planet, M_OrbitRing)
- [x] 행성 GameObject + 궤도 링 + OrbitCenter
- [x] 프리팹 저장

## Phase 2: 함선 시스템 ✅
- [x] ShipOrbit.cs (XY 평면 원형 궤도)
- [x] ShipWeapon.cs (자동 조준 + 발사 로직)
- [x] ShipController.cs (데이터 바인딩, 레벨업)
- [x] 함선 머티리얼 4종 + GameObject 4개
- [x] ShipData SO 인스턴스 4종

## Phase 3: 적 시스템 (운석) ✅
- [x] EnemyBase.cs (HP, TakeDamage, Die, 이벤트)
- [x] Meteor.cs (행성 방향 직진 + 랜덤 자전)
- [x] 운석 프리팹 + EnemyData SO 3종
- [x] WaveSpawner.cs 기본 버전

## Phase 4: 전투 시스템 ✅
- [x] Projectile.cs (이동, 충돌, 풀 반환)
- [x] DamageNumber.cs (TextMeshPro 팝업)
- [x] 발사체/데미지넘버 프리팹
- [x] ShipWeapon ↔ Projectile 연결

## Phase 5: 오브젝트 풀링 통합 ✅
- [x] ObjectPool.cs 완성 (Get/Return, 자동 확장)
- [x] 전체 시스템 풀링 적용

## Phase 6: 웨이브/스테이지 시스템 ✅
- [x] WaveSpawner.cs 본격 구현
- [x] StageManager.cs (웨이브 진행, 경험치/레벨업)
- [x] GameManager.cs (게임 상태 관리)
- [x] StageData SO 인스턴스

## Phase 7: 보스 시스템 ✅
- [x] BossEnemy.cs (상단 고정, 사인파 이동, 팬 탄막 발사)
- [x] 보스/탄막 프리팹 (Boss, BossProjectile)
- [x] StageManager 보스 스폰 연동 (BossPool + BossBulletPool)
- [x] Projectile.cs 보스 탄막→행성 충돌 처리 추가

## Phase 8: HUD/UI ✅
- [x] HUDManager.cs (상단바: HP바, 스테이지명, 배속, 타이머, 레벨, 웨이브)
- [x] BossHPBarUI.cs (보스 HP바 - 보스 등장 시 활성화)
- [x] ShipSlotUI.cs (하단 함선 카드 4개 - 색상/이름/레벨/데미지)
- [x] ResultPanelUI.cs (Game Over / Stage Clear 패널 + Retry 버튼)
- [x] Canvas 생성 (1080x1920, Scale With Screen Size)

## Phase 9: 비주얼 폴리싱 ✅
- [x] 우주 배경 (별 파티클 200개, 깜빡임 효과)
- [x] URP Post-Processing: Bloom (threshold 0.8, intensity 2.5) + Vignette
- [x] 폭발 파티클 이펙트 (Explosion 프리팹 + ObjectPool)
- [x] Emission 머티리얼 강화 (Projectile, Boss, BossProjectile, Planet)

## Phase 10: 최종 통합 테스트 ✅
- [x] 전체 플로우 테스트 (웨이브 1~3 → 보스 → Stage Clear 확인)
- [x] 밸런싱 조정 (ShipData 4종 스탯, 웨이브 구성)
- [x] 버그 수정 (StageClear 중복 호출, 풀 오브젝트 정리, testMode 비활성화)
- [x] 씬 정리 (풀 참조 연결, 재시작 로직 보강)

---

## 스크립트 의존성 순서
```
1. ObjectPool.cs (의존 없음)
2. ShipData.cs, EnemyData.cs, StageData.cs (의존 없음)
3. Planet.cs (의존 없음)
4. EnemyBase.cs → Meteor.cs, BossEnemy.cs (Planet 참조)
5. Projectile.cs, DamageNumber.cs (EnemyBase 참조)
6. ShipOrbit.cs, ShipWeapon.cs → ShipController.cs
7. WaveSpawner.cs → StageManager.cs → GameManager.cs
8. HUDManager.cs, BossHPBarUI.cs, ShipSlotUI.cs
```
