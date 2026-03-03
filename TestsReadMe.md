# Systematic Software Testing: Planning and Application (PRES1 / WRIT1)

## Presentation Goal & Overview
**Goal:** Demonstrate a systematic testing process for *FlashcardApp* using static analysis, unit tests, integration tests, coverage, and documented black-box/system test scenarios.

---

## Testing Tech Stack
- **Language/Runtime:** C# / .NET 9
- **IDE:** JetBrains Rider
- **Unit testing framework:** xUnit
- **Mocking:** Moq
- **Coverage:** Coverlet (collector + msbuild)
- **Static analysis:** SonarAnalyzer.CSharp (Roslyn analyzer via `FlashcardApp.Core.csproj`)

---

## 1. Introduction (LO1)
Systematic testing is a structured process to find defects early through planned activities: requirements analysis → test planning → test design → environment setup → execution → reporting/closure. The goal is measurable evidence of quality (pass/fail results, coverage, and static analysis findings).

---

## 2. Case Study (LO1/LO2)
**FlashcardApp** is a .NET 9 spaced-repetition flashcard app integrating the FSRS scheduling library. The most defect-prone areas are: input validation (retention/rating), session-selection logic, XP rules, and persistence (JSON file I/O). Testing focuses on the Core layer where the business rules live.

---

## 3. Testing Plan (LO2/LO3)
### Scope
**In scope**
- Core domain logic: `GamificationService`, `SettingsValidator`, session selection (`StudySessionService`) and facade orchestration (`FlashcardEngineFacade`).
- Persistence: `JsonFileUserRepository` (file I/O integration).

**Out of scope**
- WPF UI layout/styling (manual smoke-tested only).
- Internal correctness of the third-party FSRS library (we validate integration boundaries and usage).

### Testing types
- **White-box unit testing:** pure logic methods/services.
- **White-box integration testing:** persistence boundaries (JSON repository writing/reading).
- **Black-box / system testing (planned + evidenced):** end-to-end scenarios described at the facade/UI level (treating Core as the system boundary).

### Functional + non-functional testing plan (WRIT1 requirement)
- **Functional:** ratings, XP, retention validation, session card selection, user creation/load/save.
- **Performance (lightweight):** JSON repository operations complete within acceptable time for small datasets (smoke/perf check).
- **Security (lightweight):** invalid inputs don’t crash the app; corrupted JSON fails safe; file-not-found scenarios handled gracefully.

---

## 4. Test Case Design Techniques (LO2/LO4)
### Boundary Value Analysis (BVA)
**Target:** `SettingsValidator.ValidateDesiredRetention(double desiredRetention)`
- **Rule (as implemented):** retention must be between **0.70 and 0.99 inclusive**.
- Tests cover valid boundaries (0.70, 0.99) and invalid values (0.69, 1.00).

### Equivalence Partitioning (EP)
**Target:** `GamificationService.CalculateXp(int rating)`
- Valid partitions: ratings 1–4
- Invalid partitions: anything else (e.g., 0, 5, -5) throws `ArgumentOutOfRangeException`

### Isolation / interaction testing (Mocking)
**Target:** `UserService`
- Moq is used to isolate `IUserRepository` so we can verify behavioral expectations (e.g., `SaveUserAsync` called once on success; not called on failed validation).

---

## 5. Integration Testing (JSON Persistence)
Integration tests target `JsonFileUserRepository` to verify real file I/O:
- Round-trip user persistence (save then load)
- Case-insensitive username lookups
- Resilience: missing file, empty file, corrupted JSON

> Note: the repository may use synchronization internally; we currently validate correctness/resilience outcomes, not heavy concurrent load.

---

## 6. Applications of Testing & Results (LO3/LO4)
### Dynamic testing results
- Current automated test run (verified locally): **52 total, 52 passed, 0 failed, 0 skipped**

### Static analysis results
SonarAnalyzer is integrated into the build so maintainability issues can be flagged early. In earlier iterations, it highlighted unused variables and “make method static” suggestions; changes were applied during cleanup and the solution currently builds cleanly.

---

## 7. Black-box / System Testing (WRIT1 requirement)
System tests are described at the workflow level (input/output observable behavior), for example:
- Login with username → choose topic → start session → show answer → rate card → XP + stats update
- Invalid retention/rating inputs are rejected without crashing

For WRIT1, include screenshots of the UI screens and a pass/fail table for these scenarios.

---

## How to Run the Tests
```powershell
 dotnet test .\FlashcardApp.Tests\FlashcardApp.Tests.csproj -c Debug -v minimal
```
