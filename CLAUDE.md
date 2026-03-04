# Planet Defense Game — Project Context & Implementation Guide

## 프로젝트 개요
- **목표**: 모바일 우주 행성 방어 디펜스 게임 클론 (MVP)
- **원작 참고**: com.cyberjoy.prjw5n2 (Google Play)
- **엔진**: Unity 3D (Universal 3D / URP)
- **렌더링**: 2.5D — 3D 오브젝트 + Orthographic 카메라
- **개발 방식**: AI 바이브 코딩 (Claude Code + Unity MCP)
- **프로젝트 경로**: C:\AMD\Chipset_Software\work\DLP

---

## 원작 게임 분석 (스크린샷 기반)

### 게임플레이
- 세로(Portrait) 화면, 행성(지구)이 화면 중앙
- 4대의 함선이 행성 궤도를 돌며 자동 사격
- 운석/탄막이 사방에서 행성 방향으로 쏟아짐
- 함선이 운석을 요격, 못 막으면 행성 HP 감소
- 웨이브 종료 후 보스("아스트랄 터미너스" 같은 거대 화염 행성) 등장
- 보스 처치 → Stage Clear, 행성 HP 0 → Game Over

### UI/HUD 레이아웃
- **상단 좌**: 행성 아이콘 + HP값(3000) + 초록 HP 세그먼트 바
- **상단 중**: 스테이지명("일반 스테이지 1"), 배속 버튼(X1)
- **상단 우**: 타이머(MM:SS), 레벨(Lv.7)
- **보스 HP바**: 보스 등장시 화면 상단에 이름 + HP바 + 배수(X9)
- **하단**: 4개 함선 카드 슬롯 (함선 썸네일)
- **하단 우**: "자동" 버튼
- **전투 중**: 데미지 숫자 팝업 (-165, -107, -72 등)

### 시각적 특징 (2.5D)
- 행성: 3D 렌더링 + 궤도 링 이펙트 + 자전
- 함선: 3D 모델을 렌더링한 디테일한 외형
- 보스: 표면 용암 애니메이션 + 입체 조명
- 발사체: 글로우 + 잔상(트레일) 이펙트
- 배경: 성운/우주 깊이감

---

## 기술 결정사항

### Unity 프로젝트 설정
- **Unity 버전**: 2022.3 LTS (최신 패치) — 상용화 목적, 커뮤니티 자료/에셋 호환성/빌링SDK 안정성 우선
- **템플릿**: Universal 3D (URP)
- **카메라**: Orthographic, Size=10, Z=-10
- **해상도**: 1080x1920 (Portrait)
- **게임플레이 평면**: XY (Z=0 고정)
- **물리**: 3D (SphereCollider, Rigidbody, OnTriggerEnter)
- **빌드 타겟**: Android (Windows에서 iOS 빌드 불가 — Mac+Xcode 필요)
- **설치 모듈**: Android Build Support + OpenJDK + Android SDK & NDK Tools
- **코드 에디터**: VS Code (Visual Studio Community 미설치)

### MCP 도구
- **Unity MCP**: Unity-MCP (IvanMurzak) 추천 — 50+ 도구, 스크립트 직접 생성, 스크린샷 캡처 가능
  - GitHub: https://github.com/IvanMurzak/Unity-MCP
  - 대안: mcp-unity (CoderGamester) — https://github.com/CoderGamester/mcp-unity
- **NotebookLM MCP**: 설치 완료
  - 노트북 URL: https://notebooklm.google.com/notebook/f0083615-13fa-410e-9cfc-72ef88eac01a?authuser=1
  - 내용: AI 활용 게임 개발 (스프라이트 생성, 애니메이션, 코딩 에이전트 활용법)

### 에셋 생성 전략 (Two-Track + 에셋 스토어)

**Phase 1 (프로토타입)**: Unity 기본 프리미티브(Sphere, Cube)로 게임 완성에 집중
**Phase 2 (비주얼 교체)**: 에셋 교체 (프리팹 Mesh만 교체하면 끝)

- **Track A (3D 모델)**:
  - AI 생성: Meshy, Tripo3D, Rodin(Hyper3D) — 텍스트/이미지 → 3D 모델
  - 에셋 스토어: `Spaceship Low Poly`, `Planet`, `Asteroid Pack` (무료~$15)
- **Track B (VFX/이펙트)**:
  - 에셋 스토어: `Sci-Fi VFX`, `Space VFX` 팩 (발사체 궤적, 폭발, 쉴드, 글로우 등)
  - 이펙트가 비주얼의 절반 이상 — VFX 팩 활용이 필수
- **Track C (2D/UI)**: Midjourney/DALL-E (UI 아이콘, 카드 이미지)
- **Track D (배경)**: Blockade Labs (AI 스카이박스), 에셋 스토어 `Space Skybox`
- **Track E (코딩)**: Claude Code가 모든 C# 스크립트 작성 + URP 포스트프로세싱(Bloom, Emission)

> **에셋 스토어 선택 시 주의**: URP 호환 확인, 2022.3 호환 확인, 리뷰/평점 확인

---

## 개발 범위 (MVP)

### 포함
1. 행성 방어 핵심 메카닉 (행성 HP, 피격)
2. 함선 4종 (궤도 회전, 자동 조준, 발사)
3. 적 웨이브 스폰 시스템 (운석)
4. 보스 전투 (HP바, 탄막)
5. 스테이지 내 레벨업 시스템
6. 기본 HUD/UI
7. 데미지 넘버 팝업
8. 오브젝트 풀링

### 미포함 (추후)
- 가챠/뽑기 시스템
- 함선 강화/업그레이드 (전투 외)
- 편대 편성
- 상점/재화 시스템
- 다중 스테이지 선택
- 사운드/BGM

---

## 프로젝트 구조

```
Assets/
├── Scripts/
│   ├── Core/          # GameManager, StageManager, WaveSpawner
│   ├── Planet/        # Planet, PlanetHP
│   ├── Ship/          # ShipController, ShipWeapon, ShipOrbit
│   ├── Enemy/         # EnemyBase, Meteor, BossEnemy
│   ├── Combat/        # Projectile, DamageNumber
│   ├── UI/            # HUDManager, BossHPBarUI, ShipSlotUI
│   ├── Data/          # ScriptableObject 정의 (ShipData, EnemyData, StageData)
│   └── Utils/         # ObjectPool
├── Prefabs/
│   ├── Ships/
│   ├── Enemies/
│   ├── Projectiles/
│   ├── Effects/
│   └── UI/
├── ScriptableObjects/
│   ├── ShipData/
│   ├── EnemyData/
│   └── StageData/
├── Scenes/
│   └── GameScene.unity
├── Materials/
└── Textures/
```

## 씬 계층 구조

```
GameScene
├── --- MANAGERS ---
│   ├── GameManager          (GameManager.cs)
│   ├── StageManager         (StageManager.cs)
│   └── WaveSpawner          (WaveSpawner.cs)
├── --- GAMEPLAY ---
│   ├── Planet               (Planet.cs, PlanetHP.cs)
│   │   └── OrbitCenter      (빈 오브젝트)
│   ├── ShipContainer
│   │   ├── Ship_0~3         (ShipController, ShipWeapon, ShipOrbit)
│   └── ProjectilePool       (ObjectPool.cs)
├── --- CAMERA ---
│   └── Main Camera          (Orthographic, Size=10, Z=-10)
├── --- UI ---
│   └── Canvas (Screen Space - Overlay)
│       ├── TopBar (PlanetHPBar, StageLabel, SpeedButton, TimerText, LevelText)
│       ├── BossHPBar (비활성 → 보스시 활성)
│       ├── ShipSlots (하단 4개)
│       ├── AutoButton
│       └── DamageNumberPool
└── --- BACKGROUND ---
    └── SpaceBackground
```

---

## 스크립트 목록 (16개)

### Managers
| Script | 역할 | 핵심 메서드 |
|--------|------|------------|
| `GameManager.cs` | 게임 상태 관리 (싱글톤) | `StartStage()`, `PauseGame()`, `GameOver()`, `StageClear()` |
| `StageManager.cs` | 스테이지/웨이브 진행 | `LoadStage(StageData)`, `NextWave()`, `SpawnBoss()`, `AddExp(int)` |
| `WaveSpawner.cs` | 적 스폰 로직 | `SpawnWave(WaveData)`, `SpawnEnemy(EnemyData, Vector2)` |

### Planet
| Script | 역할 | 핵심 |
|--------|------|------|
| `Planet.cs` | 행성 엔티티 | `maxHP`, `currentHP`, `TakeDamage(int)`, `OnDestroyed` event |
| `PlanetHP.cs` | HP 바 시각화 | `UpdateHPBar()`, HP 세그먼트 |

### Ship
| Script | 역할 | 핵심 |
|--------|------|------|
| `ShipOrbit.cs` | 궤도 회전 | `orbitRadius`, `orbitSpeed`, `orbitAngle` |
| `ShipWeapon.cs` | 사격 | `fireRate`, `damage`, `FindTarget()`, `Fire()` |
| `ShipController.cs` | 통합 관리 | `shipData(SO)`, `level`, `Initialize()`, `LevelUp()` |

### Enemy
| Script | 역할 | 핵심 |
|--------|------|------|
| `EnemyBase.cs` | 기본 클래스 | `hp`, `speed`, `damage`, `TakeDamage()`, `Die()` |
| `Meteor.cs` : EnemyBase | 운석 | 행성 방향 직진 |
| `BossEnemy.cs` : EnemyBase | 보스 | 화면 상단 고정, 탄막 발사 |

### Combat
| Script | 역할 | 핵심 |
|--------|------|------|
| `Projectile.cs` | 발사체 | `OnTriggerEnter()` (3D Collider) |
| `DamageNumber.cs` | 데미지 팝업 | `Show(int, Vector3)`, 페이드아웃 |

### UI
| Script | 역할 |
|--------|------|
| `HUDManager.cs` | 상단 HUD |
| `BossHPBarUI.cs` | 보스 HP바 |
| `ShipSlotUI.cs` | 하단 함선 카드 |

### Utils
| Script | 역할 |
|--------|------|
| `ObjectPool.cs` | 제네릭 오브젝트 풀 |

---

## ScriptableObject 데이터

### ShipData
```csharp
[CreateAssetMenu] public class ShipData : ScriptableObject {
    public string shipName;
    public Sprite icon;
    public float baseDamage;
    public float fireRate;
    public float orbitRadius;
    public float orbitSpeed;
    public float projectileSpeed;
}
```

### EnemyData
```csharp
[CreateAssetMenu] public class EnemyData : ScriptableObject {
    public string enemyName;
    public Sprite sprite;
    public float hp;
    public float speed;
    public float damage;
    public bool isBoss;
    public float bossMultiplier;
}
```

### StageData
```csharp
[CreateAssetMenu] public class StageData : ScriptableObject {
    public string stageName;
    public WaveData[] waves;
    public EnemyData bossPrefab;
    public float timeLimit;
}

[System.Serializable] public class WaveData {
    public EnemyData enemyType;
    public int count;
    public float spawnInterval;
    public float delayBeforeWave;
}
```

---

## Game Flow
```
[Stage Start] → HUD 표시, 함선 배치
    ↓
[Wave Loop] (3~5 웨이브)
    → 운석 스폰 → 행성 방향 이동
    → 함선 자동 조준 & 발사
    → 피격 → 데미지 넘버
    → 적 처치 → 경험치 → 레벨업
    → 운석 행성 도달 → 행성 HP 감소
    ↓
[Boss Phase]
    → 보스 등장 + HP바
    → 보스 탄막 + 함선 공격
    → 보스 HP 0 → Stage Clear
    ↓
[Result]
    → 행성 HP 0 → Game Over
    → 보스 처치 → Stage Clear
```

## Physics Layers
| Layer | 용도 |
|-------|------|
| Planet | 행성 |
| Ship | 함선 |
| Enemy | 운석, 보스 |
| PlayerProjectile | 함선 발사체 |
| EnemyProjectile | 보스 탄막 |

충돌: PlayerProjectile ↔ Enemy, EnemyProjectile ↔ Planet, Enemy ↔ Planet

## Placeholder Art (3D 프리미티브)
- 행성: Sphere(스케일2) + 파란색 + Y축 자전 + Torus 궤도링
- 함선: Cube/Capsule(스케일0.3) × 4종(색상 구분) + LookAt
- 운석: Sphere(스케일0.3~0.5) + 주황/붉은색 + 랜덤 회전
- 보스: Sphere(스케일4) + 빨간 Emission + 불꽃 파티클
- 발사체: Sphere(스케일0.1) + Emission 하늘색 + TrailRenderer
- 배경: 검정 Skybox + 별 파티클
- 조명: Directional Light(푸른빛) + Point Light(행성 위치, 따뜻한 빛)

---

## 구현 순서
1. 프로젝트 셋업 — 폴더 구조, 기본 씬
2. Planet + Camera — 행성 배치, HP 시스템
3. Ship 시스템 — 궤도 회전, 자동 조준, 발사
4. Enemy 시스템 — 운석 스폰, 이동, 행성 충돌
5. Combat — 발사체 충돌, 데미지 계산, 데미지 넘버
6. Object Pool — 발사체, 적, 데미지 넘버 풀링
7. Wave/Stage — 웨이브 스포너, 스테이지 진행
8. Boss — 보스 등장, 탄막, HP바
9. HUD/UI — 상단바, 하단 함선 슬롯, 자동 버튼
10. Polish — 레벨업 시스템, 타이머, 게임오버/클리어

---

## 개발자 정보
- Unity 개발 경험 없음 (완전 초보)
- 디자인 리소스 제작 경험 없음
- AI 바이브 코딩으로 진행 (Claude Code가 코드 작성 + 에디터 조작 가이드 제공)
- AI가 스크립트 생성 → 사용자가 Unity 에디터에서 조립 → 결과 확인 → 반복
- 에러 발생시 Console 메시지를 Claude에 전달하여 해결
- **최종 목표**: 상용화 서비스 (빌링/IAP 연동 포함)

## 협업 방식
- **Claude Code**: C# 스크립트 전량 작성, Unity 에디터 단계별 가이드, 디버깅
- **사용자**: Unity 에디터 조작 (가이드 따라), 에셋 생성(AI 도구), 테스트 실행
- **Unity MCP 활용시**: AI가 씬 오브젝트 생성/수정, 컴포넌트 추가, 스크립트 연결 자동화
- **난이도 평가**: 중하 (10점 만점 4점) — Unity 에디터 조작은 "메뉴 클릭 → 드래그 → 값 입력" 수준

### 워크플로우
```
1. AI: 스크립트 파일 생성 + 에디터 조작 가이드
2. 사용자: Unity에서 가이드대로 조립
3. 사용자: Play → 결과 확인 / 에러시 Console 복붙
4. AI: 수정 코드 + 다음 단계 가이드
5. 반복
```

## 현재 진행 상태
- [x] 게임 분석 및 기술 스택 결정
- [x] 개발 플랜 수립
- [x] Unity 2022.3 LTS 설치 완료 (Android Build Support 포함)
- [x] Unity 프로젝트 생성 (Universal 3D 템플릿, 경로: C:\AMD\Chipset_Software\work\DLP)
- [ ] Unity MCP 설치 (Unity-MCP by IvanMurzak 예정)
- [ ] Phase 1: 프리미티브 프로토타입 개발 시작
