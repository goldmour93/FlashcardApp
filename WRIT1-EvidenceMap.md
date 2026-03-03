# WRIT1 Evidence Map (FlashcardApp)

This file maps WRIT1 report requirements to concrete project artefacts (code/tests/output) so you can reference them directly in your written report.

- Test case tables pack: `WRIT1-TestCasePack.md`

> Recommended WRIT1 scope: **FlashcardApp.Core** (business logic + JSON persistence) as the system under test, with **WPF UI** used for screenshots + manual system test evidence.

---

## 1) Introduction (Testing process + phases)
**Use:** STLC / testing lifecycle explanation.
- Artefact: `TestsReadMe.md` (sections 1,3,4)
- Evidence source: your narrative + diagrams (STLC flow)

---

## 2) Case study (system + requirements + screenshots)
**Codebase:**
- Core models/services: `FlashcardApp.Core/Models/*`, `FlashcardApp.Core/Services/*`, `FlashcardApp.Core/Facades/FlashcardEngineFacade.cs`
- UI screens for screenshots: `FlashcardApp.WpfUI/LoginWindow.xaml`, `TopicSelectionWindow.xaml`, `MainWindow.xaml`

**Functional requirements (examples):**
- FR1: User can login with username; user is loaded/created.
- FR2: User selects topic and starts study session.
- FR3: App shows question, user reveals answer.
- FR4: User rates (1–4) and XP/stats update.
- FR5: User progress persists in local JSON.

**Non-functional requirements (examples):**
- NFR1 (Reliability): corrupted/missing JSON handled safely.
- NFR2 (Performance): small JSON save/load is fast enough for interactive UX.
- NFR3 (Security/robustness): invalid inputs handled without crash.

---

## 3) Testing plan
### Scope
- In scope: Core services, facade behavior, JSON persistence.
- Out of scope: UI styling, third-party FSRS library internals.

### Testing types required by WRIT1
**White-box (unit):**
- Services + validators (examples):
  - `FlashcardApp.Tests/GamificationServiceTests.cs`
  - `FlashcardApp.Tests/SettingsValidatorTests.cs`
  - `FlashcardApp.Tests/StudySessionServiceTests.cs`
  - `FlashcardApp.Tests/UserServiceTests.cs`

**White-box (integration):**
- JSON persistence:
  - `FlashcardApp.Tests/Integration/JsonFileUserRepositoryIntegrationTests.cs`

**Black-box (system):**
- Manual system test scenarios (recommended in report):
  - Login → Topic select → Study session → Rate cards
  - Evidence: screenshots + pass/fail table in WRIT1 doc

### Techniques
- BVA/EP: see `SettingsValidatorTests.cs`, `GamificationServiceTests.cs`
- Negative testing: null/empty/invalid inputs across unit/integration tests
- Interaction testing (mocks): `UserServiceTests.cs` uses Moq

---

## 4) Test case design
**Requirement:** tabular test cases + justification.

Where to pull cases from:
- Unit test methods are already named in a “test case” style.
- Integration tests cover file I/O and resilience.
- System tests should be tabulated manually (black-box).

---

## 5) Test execution (pass/fail) + defect reporting + coverage
### Unit + Integration execution
- Command used:
  - `dotnet test .\FlashcardApp.Tests\FlashcardApp.Tests.csproj -c Debug -v minimal`
- Latest verified result (update in report as needed): **Total 52, Passed 52, Failed 0, Skipped 0**

### Coverage
- Use Coverlet + ReportGenerator output (you already have coverage artifacts in `FlashcardApp.Tests/coverage_report/`).
- Include: overall % and 1–2 screenshots of "hotspots" (files with lower coverage).

### Static analysis
- Analyzer is referenced by `FlashcardApp.Core/FlashcardApp.Core.csproj`:
  - `SonarAnalyzer.CSharp`
- In report: include a short summary and 1 screenshot of analyzer output (before/after optional).

---

## 6) Drivers, stubs, mocks (explicit WRIT1 wording)
- **Mock/stub:** `Moq.Mock<IUserRepository>` in `UserServiceTests.cs` simulates persistence.
- **Driver:** the unit test method itself acts as the driver calling the system under test.
- **Integration tests:** use real file system as the integration boundary.

---

## 7) Gaps to address in the WRIT1 document (not necessarily code)
- Include screenshots of UI for the case study.
- Provide a black-box system test table (manual execution is acceptable).
- Add short performance and security test plan sections (even if lightweight).
- Add a short defect log section (list defects found during development, even if fixed).
