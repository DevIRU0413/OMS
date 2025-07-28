# 백만 구독자 만들기!(One million subscribers)

Unity 기반의 협업 프로젝트입니다. 본 문서는 팀원 간 일관된 작업을 위한 개발 규칙 및 컨벤션을 정의합니다.



## 프로젝트 개요

- **목표**: **`눈빛보내기`** 게임을 모작 및 재해석한 게임 개발 
- **사용 목적**: 10분 게임 콘테스트 [그때 그 게임편] - 만들래


## 브랜치 전략

- `main`: 배포 혹은 안정 버전
- `develop`: 개발 통합 브랜치
- `feature/기능명`: 개별 기능 개발 (예: `feature/popup-system`)
- `hotfix/버그명`: 긴급 수정 사항
- `refactor/대상`: 리팩토링 작업

> ✅ 모든 작업은 `develop`에서 `feature/` 브랜치를 만들어 작업 후 PR<br>
✅ 브랜치 마지막 뒤에 `이니셜` 본인의 이니셜 붙여 작업 진행(ex. `feature/기능명_YSJ`)

```
feature/[본인 이름 이니셜]_[용도(파스칼 표기법 사용)]
예시) 
feature/Monster_YSJ
feature/Network_YSJ
```

## 커밋 메시지 컨벤션

**형식**

```
<타입>: <작업 내용 요약>

본문 (선택)
```

**타입 목록**

- `feat`: 새로운 기능 추가
- `fix`: 버그 수정
- `docs`: 문서 수정
- `refactor`: 코드 리팩토링 (기능 변화 없음)
- `style`: 코드 스타일 (공백, 세미콜론 등)
- `test`: 테스트 코드 추가/수정
- `chore`: 기타 변경사항
- `build`: 빌드 관련 파일 수정 / 모듈 설치 또는 삭제에 대한 커밋

**예시**

```
feat: 팝업 UI 시스템 추가

- YJH_UIManager 구현
- YSJ_PopupController와 연결
```

## PR 규칙

- `feature/*` 브랜치에서 작업 후 `develop`으로 PR
- PR 제목은 `[#이슈번호] 기능 설명`
- 충돌 발생 시 본인이 직접 해결
- 1일 이상 머지 대기 권장


## 코드 컨벤션
프로젝트는 아래의 코드 컨벤션을 따라 작성됩니다. 


| 요소                    | 규칙                | 예시                                  |
| --------------------- | ----------------- | ----------------------------------- |
| **클래스 / 인터페이스**       | `PascalCase`      | `PlayerController`, `IGameService`  |
| **메서드**               | `PascalCase`      | `StartGame()`, `GetData()`          |
| **변수 / 필드 (private)** | `camelCase`       | `playerName`, `currentHealth`       |
| **상수 / readonly 필드**  | `대문자` + `_`      | `MAX_HEALTH`, `DEFAULT_SPEED`         |
| **이벤트**               | `PascalCase` + 동사 | `OnDamageTaken`, `PlayerDied`       |
| **로컬 변수**             | `camelCase`       | `index`, `tempScore`                |
| **enum 타입**           | `PascalCase`      | `PlayerState` / `Idle`, `Running` 등 |
| **제네릭 타입 매개변수**       | `T` 접두어 사용        | `TEntity`, `TResult`                |

`private`는 `_`로 시작해서 카멜 규칙을 적용해주시면 됩니다.

`상수 / readonly` 필드는 링크의 규칙을 따르지 않고 위의 표의 규칙을 따릅니다.

> 링크: https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/coding-style/identifier-names

<br>

| 항목                 | 스타일                            | 예시                            |
| ------------------ | ------------------------------ | ----------------------------- |
| **중괄호 `{}`**       | 항상 새 줄에                        | `if ()\n{\n}`                 |
| **들여쓰기**           | 공백 4칸                          | VS 기본 설정                      |
| **공백 규칙**          | 연산자 양 옆 공백                     | `x = y + z;`                  |
| **줄바꿈**            | 논리 단위로 구분                      | 함수 간 한 줄 띄움                   |
| **파일 하나에 하나의 클래스** | 권장                             | `ClassA.cs`, `ClassB.cs` 따로   |
| **`this.` 사용**     | 선택적 (모호할 때만)                   | `this.name = name;`           |
| **접근제한자 순서**       | `public → protected → private` | 클래스/메서드/필드 모두 해당              |
| **네임스페이스**         | PascalCase로                    | `namespace MyApp.Controllers` |

> 링크: https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/coding-style/coding-conventions


### 추가 규칙
- 코루틴
  - 변수 `Coroutine` => `currentCo` 변수 뒤, `Co`
  - 함수 `IEnumerator` => `IE_Attack() { }` 함수 네임 앞 `IE_`


## 파일 관리 룰셋
- 00_WorkSpace - ID에서 작업을 하되 폴더명은 영어로 만든다.
- 기본적으로 작업자 초성_스크립트명으로 작업을한다.

```
[본인 이름 이니셜]_[용도(파스칼 표기법 사용)]
예시) 
KMS_MinionFactory
YSJ_NetworkManager
```
🔧✅