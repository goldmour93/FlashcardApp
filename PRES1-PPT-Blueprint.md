# PRES1 15-Minute PPT Blueprint (Top-Mark Aligned)

This blueprint is mapped to the PRES1 marking sections:
- Introduction (12)
- Case Study (16)
- Testing Plan (16)
- Testing Phases (16)
- Test Case Design (20)
- Applications of Testing and Results (20)

It also explicitly maps to LO1-LO4.

---

## Slide Structure (15 minutes max)

| Slide | Title | Marking focus | LO | Target time |
|---|---|---|---|---:|
| 1 | Title and Context | Introduction | LO1 | 0:25 |
| 2 | Required Video Link | Submission requirement | - | 0:10 |
| 3 | Agenda and Rubric Mapping | Introduction | LO1 | 0:35 |
| 4 | What Is Systematic Testing | Introduction | LO1 | 1:10 |
| 5 | Case Study: FlashcardApp | Case Study | LO1, LO2 | 1:10 |
| 6 | System Features and Risks | Case Study | LO1, LO2 | 1:10 |
| 7 | Test Plan: Scope/Objectives | Testing Plan | LO1, LO2 | 1:10 |
| 8 | Test Plan: Approach/Tools | Testing Plan | LO1, LO2, LO3 | 1:05 |
| 9 | Testing Phases Applied (STLC) | Testing Phases | LO1, LO2 | 1:20 |
| 10 | Test Case Design Methods | Test Case Design | LO1, LO2, LO4 | 1:15 |
| 11 | Sample Test Cases (Pass + Fail intent) | Test Case Design | LO1, LO2, LO4 | 1:20 |
| 12 | Unit + Integration Execution Results | Applications and Results | LO3, LO4 | 1:10 |
| 13 | Coverage Evidence | Applications and Results | LO3 | 0:55 |
| 14 | Static Analysis + Defect Lifecycle | Applications and Results | LO3, LO4 | 1:20 |
| 15 | Conclusions and Next Steps | Conclusion strength | LO1-LO4 | 0:55 |
| 16 | References (Harvard) | Academic quality | - | 0:25 |

Total: ~14:40 to 15:00

---

## Slide-by-Slide Build Guide

## Slide 1 - Title and Context
**Slide text:**
- Systematic Software Testing (PRES1)
- Case Study: FlashcardApp (.NET 9)
- Student ID, Module, Assessment ID

**Visuals to paste:**
- Small collage screenshot of 3 UI screens:
  - `FlashcardApp.WpfUI/LoginWindow.xaml`
  - `FlashcardApp.WpfUI/TopicSelectionWindow.xaml`
  - `FlashcardApp.WpfUI/MainWindow.xaml`

**Speaker emphasis:**
- Structured testing process
- Evidence-led presentation

---

## Slide 2 - Required Video Link
**Slide text:**
- "Recorded presentation link (OneDrive/YouTube): [paste link]"
- "Final link submitted, no post-deadline edits"

**Visuals to paste:**
- None (clean slide for compliance)

---

## Slide 3 - Agenda and Rubric Mapping
**Slide text:**
- Introduction
- Case study
- Testing plan
- Testing phases
- Test case design
- Applications and results

**Visuals to paste:**
- One table showing section -> marks

**Speaker emphasis:**
- Explicitly say: "I will cover each section in rubric order."

---

## Slide 4 - What Is Systematic Testing
**Slide text:**
- Definition: planned, repeatable defect detection process
- Why it matters: quality, reliability, regression safety
- STLC phases (high-level)
- **Evidence-driven goal:** defects found early + measurable results (tests/coverage/static analysis)

**Visuals to paste:**
- Simple STLC loop diagram (Requirements -> Plan -> Design -> Setup -> Execute -> Closure)

**Top-mark tip:**
- Mention testing is to *try to break software safely* and verify controlled behavior.

---

## Slide 5 - Case Study: FlashcardApp
**Slide text:**
- .NET 9 spaced-repetition app using FSRS integration
- Core under test: validators, services, facade, JSON repository
- Rationale: rules-heavy logic + persistence risks

**Visuals to paste:**
- Architecture sketch (WPF UI -> Core Services/Facade -> IUserRepository -> JSON file)

---

## Slide 6 - System Features and Risks
**Slide text:**
- Key features: login/create user, topic study flow, rating/XP update, save/load progress
- Risks:
  - invalid ratings/retention
  - wrong XP accumulation
  - file corruption/missing file
  - session-selection edge cases

**Visuals to paste:**
- Screenshot of modernized `MainWindow` showing rating buttons and stats

---

## Slide 7 - Test Plan: Scope and Objectives
**Slide text:**
- Scope in:
  - `GamificationService`, `SettingsValidator`, `StudySessionService`, `UserService`, `FlashcardEngineFacade`, `JsonFileUserRepository`
- Scope out:
  - WPF styling automation
  - FSRS internals
- Objectives:
  - correctness
  - robustness
  - regression prevention
- **Entry criteria:** solution builds; test project restores; tools configured
- **Exit criteria:** all automated tests pass; coverage report generated; static analysis reviewed; at least one defect lifecycle recorded

**Visuals to paste:**
- Scope table (In/Out)

---

## Slide 8 - Test Plan: Approach and Tools
**Slide text:**
- Unit: xUnit
- Isolation: Moq
- Integration: JSON file repository tests
- Coverage: Coverlet + ReportGenerator
- Static analysis: SonarAnalyzer.CSharp
- **Technique justification (LO2):**
  - BVA/EP efficiently targets boundary + partition defects in business rules
  - Moq isolates dependencies and verifies interactions (saves/regressions)
  - Integration tests catch real file I/O + serialization faults
  - Static analysis finds maintainability/bug risks without executing code
- **Strengths/weaknesses (LO2):**
  - Strength: fast, repeatable regression safety
  - Weakness: unit tests don’t prove UI/system behavior; coverage ≠ correctness

**Visuals to paste:**
- Screenshot of `FlashcardApp.Core/FlashcardApp.Core.csproj` package reference for SonarAnalyzer
- Screenshot of `dotnet test` command in terminal
- **(Optional but strong)** Rider Inspect Code screenshot (before/after or issue list)

---

## Slide 9 - Testing Phases Applied (STLC)
**Slide text:**
- Requirements analysis -> identify rule risks
- Planning -> define scope/types/objectives
- Design -> BVA, EP, negative tests
- Setup -> Rider, .NET 9, xUnit/Moq/Coverlet
- Execution -> automated suite + manual black-box cases
- Closure -> pass/fail summary + defect lifecycle + insights

**Visuals to paste:**
- STLC phase-to-evidence table

---

## Slide 10 - Test Case Design Methods
**Slide text:**
- Boundary Value Analysis:
  - `ValidateDesiredRetention` (0.70, 0.99 valid; 0.69, 1.00 invalid)
- Equivalence Partitioning:
  - `CalculateXp` (ratings 1-4 valid; others invalid)
- Negative testing and interaction testing with Moq
- **Oracle definition:** expected return values, expected exceptions, or safe nulls

**Visuals to paste:**
- Small snippet screenshots from:
  - `FlashcardApp.Tests/SettingsValidatorTests.cs`
  - `FlashcardApp.Tests/GamificationServiceTests.cs`
  - `FlashcardApp.Tests/UserServiceTests.cs`

---

## Slide 11 - Sample Test Cases (Pass + Fail Intent)
**Slide text:**
- Table columns:
  - Test ID
  - Input
  - Expected
  - Actual
  - Pass/Fail
  - Defect ID (if failed)
- Show 6 representative rows from:
  - 2 unit
  - 2 integration
  - 2 black-box manual
- **Mandatory:** include **one historical FAIL -> Fix -> PASS** row (defect lifecycle)
  - Example: DB dependency crash (`NpgsqlException`) -> switched to JSON persistence -> pass

**Visuals to paste:**
- Table pulled from `WRIT1-TestCasePack.md` (use 6 rows)
- **Add a small defect lifecycle mini-table** (Fail / Fix / Re-test), even if it’s just 1 row

---

## Slide 12 - Unit and Integration Results
**Slide text:**
- Automated result: **52 total, 52 passed, 0 failed, 0 skipped**
- **Definition:** Unit tests = Core logic + mocks; **Integration tests here = JSON persistence (real file I/O + serialization), not database/network**
- What this proves: stable behavior across core logic + persistence

**Visuals to paste:**
- Terminal screenshot of `dotnet test` summary
- Optional Rider test explorer screenshot

---

## Slide 13 - Coverage Evidence
**Slide text:**
- Line: **97%**
- Branch: **80% (64/80)**
- Method: **100% (36/36)**
- Why branch matters: decision paths tested, not only statements
- **Limitations:** coverage is a completeness indicator, not proof of correctness; pair with strong oracles
- **Interpretation (defend 80% branch):** branch coverage includes defensive/exception decision paths; priority was high-risk business-rule decisions

**Visuals to paste:**
- `FlashcardApp.Tests/coverage_report/Summary.txt` screenshot
- One file-level hotspot screenshot (optional)

---

## Slide 14 - Static Analysis and Defect Lifecycle
**Slide text:**
- Static analysis integrated in build (SonarAnalyzer.CSharp)
- **Pick ONE main defect lifecycle story and make it crisp + evidenced:**
  - Fail evidence (screenshot/log)
  - 1-sentence fix
  - Re-test evidence (green test run)
- Optional additional defects can be one-line mentions only

**Must show evidence on-screen:**
- `FlashcardApp.Core.csproj` SonarAnalyzer reference
- **AND** Rider inspection/analyzer finding output (issue list) showing at least one finding

**Visuals to paste:**
- **Required:** screenshot of SonarAnalyzer reference in `.csproj`
- **Required:** screenshot of Rider inspection/analyzer output (Problems/Inspect Code list)
- Defect lifecycle mini-table (Fail -> Fix -> Re-test) + fail screenshot snippet

---

## Slide 15 - Key Insights and Conclusion
**Slide text:**
- Systematic testing improved correctness and resilience
- Fail-intent testing found real weaknesses
- Final state: strong evidence-backed quality baseline
- Next steps: execute full manual system table and lightweight perf timings in CI

**Visuals to paste:**
- 3-bullet summary card with metrics (52/52, 97% line, 80% branch)

---

## Slide 16 - References (Harvard)
**Slide text:**
- Module resources
- Microsoft .NET docs
- xUnit docs
- Moq docs
- Coverlet/ReportGenerator docs
- SonarAnalyzer docs

**Visuals to paste:**
- None; keep text clean/readable

---

## Evidence Capture Checklist (Do Before Recording)
- [ ] `dotnet test` result screenshot showing 52/52
- [ ] Coverage summary screenshot from `FlashcardApp.Tests/coverage_report/Summary.txt`
- [ ] SonarAnalyzer package screenshot from `FlashcardApp.Core/FlashcardApp.Core.csproj`
- [ ] 3 UI screenshots (login/topic/main)
- [ ] One test code snippet screenshot for BVA/EP
- [ ] Defect lifecycle table screenshot (Fail -> Fix -> Re-test)
- [ ] Slide 2 video link inserted and tested
- [ ] **Technique justification slide notes**: 1–2 lines on strengths/weaknesses
- [ ] **One historical defect** screenshot or log snippet (Npgsql error / UI clip) for fail->fix->pass
- [ ] **Static analysis evidence** screenshot (Rider inspection list or analyzer output)
- [ ] Slide 12 includes the one-line **integration definition** (JSON file persistence)
- [ ] Slide 14 includes **one fully evidenced defect story** (fail screenshot + fix + re-test screenshot)
- [ ] Static analysis evidence includes **finding output**, not only the `.csproj` package reference

---

## Timing Trim Notes (to stay under 15 min)
- Slide 3: keep to **20 seconds**
- Slide 6: trim by **15–20 seconds** (only 3 risks)
- Slide 16: do **not** narrate; leave on screen as you close

---

## Commands to Reproduce Evidence
```powershell
dotnet test .\FlashcardApp.Tests\FlashcardApp.Tests.csproj -c Debug -v minimal
```
