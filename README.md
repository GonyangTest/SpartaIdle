# SpartaIdle

## 🎮 1. 게임 개요
이 프로젝트는 내일배움캠프에서 진행한 과제로, Unity 기반의 방치형(Idle) RPG 게임입니다. 플레이어가 자동으로 스테이지를 진행하며 적을 처치하고 경험치와 아이템을 획득하는 게임입니다.

### 주요 특징
- **자동 전투 시스템**: 플레이어 캐릭터가 AI로 자동 전투
- **스테이지 진행**: 다양한 스테이지와 적 패턴
- **아이템 시스템**: 무기, 방어구, 소모품 등 다양한 아이템
- **버프 시스템**: 플레이어 능력치 강화
- **경험치 및 레벨링**: 캐릭터 성장 시스템

## ⚙ 2. 유니티 버전
Unity 2022.3.17f1

## 🚀 3. 실행 방법

1. Unity에서 프로젝트 열기
2. SampleScene 씬 로드
3. Play 버튼 클릭하여 게임 시작

## 🏗️ 4. 프로젝트 구조

### 📁 에셋 구조

```
Assets/
├── Scripts/          # C# 스크립트
├── Scenes/           # 게임 씬 파일
├── Prefabs/          # 프리팹 오브젝트
├── Sprite/           # 2D 스프라이트
├── Animations/       # 애니메이션 클립
├── Resources/        # 런타임 로드 리소스
├── Fonts/            # 폰트 파일
└── Settings/         # 프로젝트 설정
```

### 📁 Scripts 폴더 구성

```
Assets/Scripts/
├── Manager/           # 게임 관리 클래스들
├── UI/               # 사용자 인터페이스
├── Entity/           # 게임 엔티티 (플레이어, 적)
├── Data/             # 게임 데이터 구조
├── Item/             # 아이템 시스템
├── Interface/        # 인터페이스 정의
├── Utility/          # 유틸리티 클래스들
└── Constants/        # 상수 정의
```



## 5. 🔧 주요 시스템 및 기능

### 1) Manager 시스템

#### GameManager
- 게임의 전체적인 흐름 제어
- 스테이지 시작/재시작/다음 스테이지 진행
- 다른 매니저들의 초기화 및 관리

#### StageManager
- 스테이지 데이터 로드 및 관리
- 적 스폰 및 스테이지 클리어 조건 체크
- 경험치, 골드, 아이템 보상 관리

#### UIManager
- UI 시스템의 중앙 관리
- Fixed UI, Window UI, Popup UI 관리

#### PlayerManager
- 플레이어 상태 관리 (레벨, 경험치, 스탯)
- 버프 시스템과의 연동
- 플레이어 위치 초기화 및 체력 관리

#### PlayerBuffManager
- 플레이어 버프 관리
- 버프 적용 및 해제 시스템
- 스탯 보너스 계산

### 2) AI 시스템

#### AIPlayer
- NavMesh 기반 자동 이동
- 상태 머신을 통한 행동 제어
- 자동 전투 및 타겟 추적

### 3) UI 시스템

Unity의 UI 시스템을 활용한 모듈화된 UI 구조:
- **Fixed UI**: 항상 화면에 표시되는 UI
- **Window UI**: 모달 형태의 창 UI
- **Popup UI**: 스택 구조의 팝업 UI

### 4) 아이템 시스템

- ItemInstance: 기본 아이템 클래스
- WeaponInstance: 무기 아이템
- ArmorInstance: 방어구 아이템

### 5. 데이터 관리

- ScriptableObject 기반 데이터 관리
- 리소스 경로 및 게임 관련 상수 관리 (ResourcePaths, GameConstants)

## 🎯 6. 주요 기능

### 스테이지 시스템
- 다양한 적 패턴의 스테이지
- 스테이지 클리어 조건 (모든 적 처치)
- 보상 시스템 (경험치, 골드, 아이템)

### 전투 시스템
- 자동 전투 (AI 기반)
- 공격력, 방어력 기반 데미지 계산

### 성장 시스템
- 경험치 획득 및 레벨업
- 버프를 통한 능력치 강화
- 아이템 장착을 통한 스탯 증가

## 🛠️ 7. 기술 스택
- **엔진**: Unity
- **언어**: C#
- **패턴**: Singleton, State Machine, Object Pool
- **UI**: Unity UI System (Canvas, UGUI)
- **AI**: NavMesh Agent 기반 자동 이동



 