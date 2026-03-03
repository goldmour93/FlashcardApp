# PRES1 Speaker Script (15 Minutes)

Use this as your read-aloud script. Keep a natural pace and avoid rushing.

---

## Slide 1 - Title and Context (0:00-0:25)
"Hello, I am [Name], and this is my PRES1 presentation on Systematic Software Testing. My case study is FlashcardApp, a .NET 9 spaced-repetition application. I will demonstrate how I applied a structured testing process and present measurable evidence from unit testing, integration testing, coverage, and static analysis."

---

## Slide 2 - Required Video Link (0:25-0:35)
"This slide contains the required final video link used for submission and marking."

---

## Slide 3 - Agenda and Rubric Mapping (0:35-1:05)
"I will follow the assessment structure directly: introduction, case study, testing plan, testing phases, test case design, and finally applications of testing with results. This mapping is intentional so each marking section is explicitly addressed."

---

## Slide 4 - What Is Systematic Testing (1:10-2:20)
"Systematic software testing is a planned, repeatable process for identifying defects before release. It is not random checking. It follows phases: requirement analysis, planning, test design, environment setup, execution, and closure. The key objective is evidence-based quality assurance.

In this project, the purpose is to challenge the application with both valid and invalid inputs, try to break risky paths safely, and confirm that failures are handled correctly rather than causing uncontrolled crashes. I will support each phase with evidence: test results, coverage, static analysis output, and at least one defect lifecycle example."

---

## Slide 5 - Case Study: FlashcardApp (2:20-3:30)
"I selected FlashcardApp because it contains high-value logic that is sensitive to defects: spaced repetition scheduling integration, rating-to-XP rules, and persistence of user progress. If validation or persistence fails, user learning data becomes unreliable.

The core test target is the Core layer: validators, services, facade logic, and repository behavior. The WPF user interface is used for context and black-box workflow checks."

---

## Slide 6 - Features and Risks (3:20-4:25)
"The key functional flow is: login or create user, select topic, review flashcards, rate answers, and persist progress.

The highest risks I targeted were: boundary and invalid-input issues, incorrect XP updates, and persistence reliability such as missing or corrupted JSON. These risks directly informed my test design and what I prioritised for coverage."

---

## Slide 7 - Testing Plan: Scope and Objectives (4:40-5:50)
"My scope includes `GamificationService`, `SettingsValidator`, `StudySessionService`, `UserService`, `FlashcardEngineFacade`, and `JsonFileUserRepository`.

Out of scope are UI styling automation and internal FSRS library formula validation.

Objectives are to verify rule correctness, enforce robust error handling, prevent regressions with automation, and produce measurable quality evidence.

Entry criteria were: the solution builds and the test project restores successfully. Exit criteria were: all automated tests pass, a coverage report is generated, static analysis is reviewed, and a defect lifecycle example is documented."

---

## Slide 8 - Testing Plan: Approach and Tools (5:50-6:55)
"For dynamic testing I used xUnit with Moq for isolation and interaction verification. For integration, I tested real JSON file I/O. For coverage I used Coverlet and ReportGenerator. For static analysis, SonarAnalyzer.CSharp is integrated at project level.

I selected these techniques intentionally. Boundary value analysis and equivalence partitioning efficiently target boundary and partition defects in business rules. Moq isolates dependencies so I can verify repository interactions and prevent regressions. Integration tests catch real file and serialization faults that unit tests cannot. Static analysis complements execution by finding maintainability and bug risks without running the code.

The strengths are speed and repeatability. The limitation is that unit tests don’t prove the full UI behavior, and coverage is not proof of correctness."

---

## Slide 9 - Testing Phases Applied (STLC) (6:55-8:15)
"In requirements analysis, I identified high-risk rules such as rating boundaries and persistence reliability. In planning, I defined scope and test types. In design, I applied boundary value analysis, equivalence partitioning, and negative testing.

In setup, I configured .NET 9, Rider, xUnit, Moq, and coverage tooling. In execution, I ran automated tests and captured evidence. In closure, I reviewed pass/fail outcomes, defect lifecycle evidence, and key quality insights."

---

## Slide 10 - Test Case Design Methods (8:15-9:30)
"Boundary value analysis was used on retention validation, testing lower and upper boundaries and just-outside values.

Equivalence partitioning was used on rating-to-XP logic, splitting valid ratings 1 to 4 from invalid partitions.

I also used interaction testing with mocks to verify repository save behavior under success and failure conditions.

For each test, the oracle is explicit: either an expected return value, an expected exception, or a safe null result for fault scenarios."

---

## Slide 11 - Sample Test Cases: Pass + Fail Intent (9:30-10:50)
"This table shows representative test specifications with expected outcomes for valid and invalid paths. Invalid-input cases are included intentionally to try to break the system safely.

A key point is that a negative test can be marked as pass when the expected defensive behavior occurs, for example throwing a controlled exception or returning a safe null value.

I also include one defect lifecycle example: initially the app failed due to a database dependency error, which I captured as evidence. I then refactored to JSON persistence and re-tested, confirming the issue was resolved."

---

## Slide 12 - Unit and Integration Results (10:50-12:00)
"The automated suite currently reports 52 discovered tests with 52 passes, 0 failures, and 0 skipped.

For clarity, unit tests here cover Core logic in isolation, often using mocks. Integration tests here mean JSON persistence using `JsonFileUserRepository` with real file I/O and serialization. It does not mean database or network integration.

This demonstrates stable behavior across core business logic and JSON persistence boundaries in the current build baseline."

---

## Slide 13 - Coverage Results (12:00-12:55)
"Coverage summary is: 97% line coverage, 80% branch coverage, and 100% method coverage.

The branch metric is especially important because it indicates that decision logic paths are exercised, not just straight-line code. Branch coverage is lower partly because it includes defensive and exception decision paths, and I prioritised high-risk business-rule decisions first. Coverage is still a completeness indicator, not proof of correctness, so it must be paired with strong oracles and negative testing."

---

## Slide 14 - Static Analysis and Defect Lifecycle (12:55-14:10)
"Static analysis is integrated through SonarAnalyzer.CSharp. I will show the analyzer integration in the project file, and I will also show the IDE inspection output so there is evidence of actual findings.

For defect lifecycle evidence, I will focus on one provable story: I show the initial failure evidence, describe the fix in a single sentence, and then show the re-test evidence with a green test run. This demonstrates fail-intent testing, root-cause analysis, corrective action, and re-test confirmation."

---

## Slide 15 - Conclusion (14:15-15:10)
"In conclusion, this project demonstrates systematic software testing as a complete process: planned scope, suitable techniques, structured execution, and measurable evidence.

The outcomes show a strong quality baseline supported by passing automated tests, high coverage, and documented defect resolution. The next step is to complete and evidence the full manual black-box table and lightweight performance timings for extended assurance."

---

## Slide 16 - References (optional if time permits)
"These are my Harvard references for the tools and techniques used." 

(Do not narrate further; leave on screen while you end the recording.)

---

## Delivery Tips for Top Marks
- Use assessor vocabulary explicitly: "scope", "objectives", "testing phases", "test case design", "applications and results".
- Always tie claims to evidence on-screen.
- Do not read code for too long; show snippets briefly and explain risk -> test -> outcome.
- Mention at least one fail->fix->re-test story clearly.
- Keep transitions explicit: "This now addresses the Testing Plan section of the rubric."

---

## Quick Rehearsal Timing
- First full read: 17-18 minutes (natural)
- Second read (trim pauses): ~15 minutes
- Final recording target: 14:30 to 15:00
