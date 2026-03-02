# Systematic Software Testing: Planning and Application (PRES1)

## ?? Presentation Goal & Overview (Slide 1 & 2)
**Goal:** To demonstrate the application of systematic software testing�including static analysis, unit testing, and formal test case design�to a Spaced Repetition Flashcard Application.

*(Note for Slide 2: Ensure you paste your OneDrive/YouTube video link here before submission!)*

---

## 1. Introduction (12%)
*   **What is Systematic Software Testing?** It is a structured, destructive process designed to identify defects (bugs) in software before they cause failures in production. It ensures quality and functionality through defined phases.
*   **The Big Myth:** Testing is not meant to prove that software works; it is meant to break it.
*   **Relevance:** As seen in historical failures (e.g., Therac-25), executing a defect leads to system failure. Systematic testing mitigates this risk, saving time, money, and reputation.

---

## 2. Case Study (16%)
*   **The System:** A C# .NET 9 Spaced Repetition Flashcard App (incorporating the FSRS algorithm).
*   **Rationale for Selection:** The application relies on complex mathematical scheduling (FSRS) and gamification logic (XP calculation). If a defect exists in the `SettingsValidator` (e.g., allowing a retention rate of 150%), the algorithm will fail, destroying the user's study schedule.
*   **Importance:** Systematic testing is critical here because the core value of the app relies entirely on the accuracy of its background logic, which cannot be easily verified by just clicking around the UI.

---

## 3. Testing Plan (16%)
### Scope of Testing
*   **In Scope:** Core domain logic (`GamificationService`, `SettingsValidator`), User state management (`UserService`), and JSON persistence via `IUserRepository`.
*   **Out of Scope:** UI layout/styling (WPF XAML) and third-party FSRS library internals.

### Testing Objectives
1.  Verify that gamification logic correctly assigns XP based on user ratings.
2.  Ensure system boundaries (e.g., retention rates) reject invalid data gracefully.
3.  Confirm that user data (Total XP, Topic XP) is accurately updated and saved.

### Testing Approach
*   **Static Analysis Integration:** We integrated **SonarAnalyzer.CSharp** (a Roslyn-based static code analyzer) directly into the `.csproj`. This acts as our "bugs tool" to detect code smells, unused variables, and maintainability issues *without* executing the code.
*   **Unit Testing Role:** We used **xUnit** and **Moq** to isolate individual classes. This ensures robust, modular functionality by verifying that each component works perfectly in isolation before integration.

---

## 4. Testing Phases (STLC) (16%)
We applied the Software Testing Life Cycle (STLC) to our case study:
1.  **Requirement Analysis:** Identified that the app needs to calculate XP based on a 1-4 rating scale.
2.  **Test Planning:** Decided to use xUnit for dynamic testing and SonarAnalyzer for static testing.
3.  **Test Case Design:** Used Equivalence Partitioning and Boundary Value Analysis to write C# test methods.
4.  **Environment Setup:** Configured the `FlashcardApp.Tests` project with Moq and xUnit dependencies.
5.  **Test Execution:** Ran `dotnet test` to execute the suite.
6.  **Test Cycle Closure:** Reviewed the pass/fail statistics and static analysis warnings to evaluate software quality.

---

## 5. Test Case Design (20%)
We utilized two primary black-box testing techniques:

### A. Boundary Value Analysis (BVA)
*   **Target:** `SettingsValidator.ValidateDesiredRetention(double retention)`
*   **Rule:** Retention must be strictly between `0.70` and `0.99`.
*   **Test Cases (Pass/Fail):**
    *   *Pass Scenario (Lower Boundary):* Input `0.70` -> Expected: No Exception.
    *   *Pass Scenario (Upper Boundary):* Input `0.99` -> Expected: No Exception.
    *   *Fail Scenario (Just Below):* Input `0.69` -> Expected: `ArgumentOutOfRangeException`.
    *   *Fail Scenario (Just Above):* Input `1.00` -> Expected: `ArgumentOutOfRangeException`.

### B. Equivalence Partitioning (EP)
*   **Target:** `GamificationService.CalculateXp(int rating)`
*   **Rule:** Ratings 1-4 are valid. Anything else is invalid.
*   **Test Cases (Pass/Fail):**
    *   *Pass Scenario (Valid Partition):* Input `3` (Good) -> Expected: Returns `10` XP.
    *   *Fail Scenario (Invalid Partition - High):* Input `5` -> Expected: `ArgumentOutOfRangeException`.
    *   *Fail Scenario (Invalid Partition - Low):* Input `0` -> Expected: `ArgumentOutOfRangeException`.

---

## 6. Applications of Testing and Results (20%)
### Applying the Tests
We executed the designed test cases using the `dotnet test` command. 
*   **Unit Testing Results:** 45 total tests were discovered and executed. **100% Pass Rate (45/45).**
*   **Mocking Results:** Using `Moq`, we successfully verified that `UserService.AddXpToUserAsync` calls the repository `SaveUserAsync` exactly once on success, and *never* calls it if an invalid XP amount throws an exception.

### Static Analysis Results
The integration of **SonarAnalyzer.CSharp** yielded immediate code quality metrics and warnings during the build process:
*   *Warning S2325:* Suggested making methods like `CalculateXp` static, as they do not use instance data.
*   *Warning S1481:* Detected an unused local variable (`reviewLog`) in `FlashcardEngineFacade.cs`.

### Key Insights & Implications
1.  **Early Defect Detection:** Static analysis found maintainability issues (like unused variables) that unit tests would have missed.
2.  **Robustness:** By testing the extreme boundaries (e.g., `0.69` retention), we proved the system is protected against catastrophic mathematical failures in the FSRS algorithm.
3.  **Modularity:** Using Moq forced us to rely on Dependency Injection (`IUserRepository`), proving that our architecture is clean, modular, and highly testable.

---

## 6b. Integration Testing (JSON persistence)

### Why integration tests?
Unit tests can't catch file I/O defects like:
- missing or locked files
- corrupted JSON
- incorrect serialization of user data

### What we added
We added JSON repository integration tests:
- `FlashcardApp.Tests/Integration/JsonFileUserRepositoryIntegrationTests.cs`
- They round-trip a `User` + `Deck`, validate ID assignment, and verify resilience to corrupted JSON

### How to run it
```powershell
 dotnet test .\FlashcardApp.Tests\FlashcardApp.Tests.csproj -c Debug -v minimal
```
