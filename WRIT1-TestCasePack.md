# WRIT1 Test Case Pack (FlashcardApp)

Use this pack directly in your WRIT1 report. It is aligned to the project and rubric requirements:
- White-box unit testing
- White-box integration testing
- Black-box/system testing
- Non-functional testing (performance + security)

> Note: This pack is a curated reporting subset for WRIT1 tables. The full automated suite currently reports **52/52 passing** via `dotnet test`.

---

## 1) ID Scheme + Status
- `TC-UNIT-xxx`: Unit tests (white-box)
- `TC-INT-xxx`: Integration tests (white-box)
- `TC-SYS-xxx`: System tests (black-box, manual)
- `TC-PERF-xxx`: Performance tests
- `TC-SEC-xxx`: Security/robustness tests

Status values:
- `PASS`
- `FAIL`
- `N/E` (not executed yet)

Evidence references:
- `EV-TEST-xx`: test runner output screenshots/logs
- `EV-COV-xx`: coverage screenshots/reports
- `EV-UI-xx`: UI screenshots/manual execution evidence
- `EV-AN-xx`: static analysis evidence

---

## 2) Unit Test Cases (White-box)

| Test ID | Requirement | Test Basis | Preconditions | Inputs/Steps | Expected Result | Type | Evidence | Status |
|---|---|---|---|---|---|---|---|---|
| TC-UNIT-001 | Validate retention range | `SettingsValidator.ValidateDesiredRetention` | None | Call with `0.70` | No exception | BVA | EV-TEST-01 | PASS |
| TC-UNIT-002 | Validate retention range | `SettingsValidator.ValidateDesiredRetention` | None | Call with `0.99` | No exception | BVA | EV-TEST-01 | PASS |
| TC-UNIT-003 | Reject invalid retention | `SettingsValidator.ValidateDesiredRetention` | None | Call with `0.69` | `ArgumentOutOfRangeException` | BVA/Negative | EV-TEST-01 | PASS |
| TC-UNIT-004 | Reject invalid retention | `SettingsValidator.ValidateDesiredRetention` | None | Call with `1.00` | `ArgumentOutOfRangeException` | BVA/Negative | EV-TEST-01 | PASS |
| TC-UNIT-005 | XP mapping for valid rating | `GamificationService.CalculateXp` | None | Input `1` | Returns `0` | EP | EV-TEST-01 | PASS |
| TC-UNIT-006 | XP mapping for valid rating | `GamificationService.CalculateXp` | None | Input `2` | Returns `5` | EP | EV-TEST-01 | PASS |
| TC-UNIT-007 | XP mapping for valid rating | `GamificationService.CalculateXp` | None | Input `3` | Returns `10` | EP | EV-TEST-01 | PASS |
| TC-UNIT-008 | XP mapping for valid rating | `GamificationService.CalculateXp` | None | Input `4` | Returns `10` | EP | EV-TEST-01 | PASS |
| TC-UNIT-009 | Reject invalid rating | `GamificationService.CalculateXp` | None | Input `0` / `5` / `-5` | `ArgumentOutOfRangeException` | EP/Negative | EV-TEST-01 | PASS |
| TC-UNIT-010 | XP update and save on success | `UserService.AddXpToUserAsync` + Moq | Mock repo configured | Add positive XP to user/topic | Total and topic XP updated; `SaveUserAsync` called once | Interaction/Mock | EV-TEST-02 | PASS |
| TC-UNIT-011 | Reject negative XP | `UserService.AddXpToUserAsync` + Moq | Mock repo configured | Add negative XP | Exception; repo save not called | Negative/Interaction | EV-TEST-02 | PASS |
| TC-UNIT-012 | Existing user retrieval | `UserService.GetOrCreateUserAsync` + Moq | Repo returns existing user | Request existing username | Existing user returned; save not called | Branch/Interaction | EV-TEST-02 | PASS |
| TC-UNIT-013 | New user creation | `UserService.GetOrCreateUserAsync` + Moq | Repo returns null user | Request new username | New user created and saved once | Branch/Interaction | EV-TEST-02 | PASS |

Source files:
- `FlashcardApp.Tests/SettingsValidatorTests.cs`
- `FlashcardApp.Tests/GamificationServiceTests.cs`
- `FlashcardApp.Tests/UserServiceTests.cs`

---

## 3) Integration Test Cases (White-box)

| Test ID | Requirement | Test Basis | Preconditions | Inputs/Steps | Expected Result | Type | Evidence | Status |
|---|---|---|---|---|---|---|---|---|
| TC-INT-001 | Persist and reload user/deck | `JsonFileUserRepository` | Temp file path | Save user with deck; load by username | Loaded user equals saved values | File I/O integration | EV-TEST-03 | PASS |
| TC-INT-002 | Assign user ID if empty | `JsonFileUserRepository.SaveUserAsync` | Temp file path | Save user with empty `Id` | User gets non-empty GUID | Data integrity | EV-TEST-03 | PASS |
| TC-INT-003 | Update existing record by ID | `JsonFileUserRepository` | Existing saved user | Save updated user with same ID | Latest values persisted | Update behavior | EV-TEST-03 | PASS |
| TC-INT-004 | Case-insensitive username lookup | `JsonFileUserRepository.GetUserByUsernameAsync` | Saved user "CaseUser" | Lookup "caseuser" | User found | Lookup behavior | EV-TEST-03 | PASS |
| TC-INT-005 | Whitespace username handling | `JsonFileUserRepository.GetUserByUsernameAsync` | Temp repo | Lookup with whitespace | Returns null safely | Robustness | EV-TEST-03 | PASS |
| TC-INT-006 | Corrupted JSON handling | `JsonFileUserRepository` | Invalid JSON file exists | Lookup any username | Returns null, no crash | Resilience | EV-TEST-03 | PASS |
| TC-INT-007 | Empty JSON handling | `JsonFileUserRepository` | Empty JSON file exists | Lookup any username | Returns null, no crash | Resilience | EV-TEST-03 | PASS |
| TC-INT-008 | Missing file handling | `JsonFileUserRepository` | File deleted | Lookup by username and ID | Returns null, no crash | Resilience | EV-TEST-03 | PASS |
| TC-INT-009 | Null user save blocked | `JsonFileUserRepository.SaveUserAsync` | Temp repo | Save `null` user | `ArgumentNullException` | Negative | EV-TEST-03 | PASS |

Source file:
- `FlashcardApp.Tests/Integration/JsonFileUserRepositoryIntegrationTests.cs`

---

## 4) System Test Cases (Black-box, Manual)

These are WRIT1-required black-box tests for whole workflow behavior. Execute in the running app and capture screenshots.

| Test ID | Requirement | Preconditions | Steps | Expected Result | Evidence | Status |
|---|---|---|---|---|---|---|
| TC-SYS-001 | Login existing/new user | App starts at login screen | Enter username -> click Continue | User proceeds to topic selection | EV-UI-01 | N/E |
| TC-SYS-002 | Topic selection required | Topic window visible | Attempt to start with no valid topic selected | User prevented or prompted; app does not crash | EV-UI-02 | N/E |
| TC-SYS-003 | Start topic session | Topic selected | Click Start Session | Main study window opens with first card | EV-UI-03 | N/E |
| TC-SYS-004 | Reveal answer flow | Card shown with hidden answer | Click Show Answer | Back/answer text becomes visible; rating buttons shown | EV-UI-04 | N/E |
| TC-SYS-005 | Rating updates stats | Card answer visible | Click `Good (3)` | Cards reviewed increments; XP stats update | EV-UI-05 | N/E |
| TC-SYS-006 | Session completion flow | Exhaust all cards in topic session | Rate until complete | Completion message shown; continue/switch dialog displayed | EV-UI-06 | N/E |
| TC-SYS-007 | Continue same topic | Completion dialog shown | Click Yes | New session starts with same topic | EV-UI-07 | N/E |
| TC-SYS-008 | Switch topic | Completion dialog shown | Click No | Topic selection window opens | EV-UI-08 | N/E |

Suggested screenshots:
- Login screen
- Topic selection screen
- Main window before/after show answer
- Completion dialog
- Stats before/after rating

---

## 5) Non-Functional Test Cases

### 5.1 Performance (lightweight, WRIT1-compliant)

| Test ID | Goal | Preconditions | Steps | Acceptance Criteria | Evidence | Status |
|---|---|---|---|---|---|---|
| TC-PERF-001 | JSON save/load latency for small dataset | Local machine idle, Debug/Release noted | Save and load user with small deck (e.g., 25 cards), measure elapsed time | Save + load complete within threshold you define (e.g., <500 ms local) | EV-TEST-04 | N/E |
| TC-PERF-002 | Repeated operations stability | Temp file repository | Execute repeated save/load loop (e.g., 100 iterations) | No exceptions; consistent completion | EV-TEST-04 | N/E |

### 5.2 Security/Robustness (lightweight, WRIT1-compliant)

| Test ID | Goal | Preconditions | Steps | Expected Result | Evidence | Status |
|---|---|---|---|---|---|---|
| TC-SEC-001 | Input robustness for invalid values | App/test environment ready | Use invalid retention/rating values in tests | Exceptions thrown predictably; no crash | EV-TEST-01 | PASS |
| TC-SEC-002 | Corrupted data resilience | Corrupted `users.json` present | Try user lookup/load | Safe failure (null/handled), no app crash | EV-TEST-03 | PASS |
| TC-SEC-003 | Missing file resilience | JSON file deleted | Lookup by username/ID | Safe failure, no crash | EV-TEST-03 | PASS |

---

## 6) Execution Summary Template (Paste into WRIT1)

| Test Type | Planned | Executed | Passed | Failed | Not Executed | Pass Rate |
|---|---:|---:|---:|---:|---:|---:|
| Unit | 13 | 13 | 13 | 0 | 0 | 100% |
| Integration | 9 | 9 | 9 | 0 | 0 | 100% |
| System (manual) | 8 | 0 | 0 | 0 | 8 | 0% |
| Performance | 2 | 0 | 0 | 0 | 2 | 0% |
| Security/Robustness | 3 | 3 | 3 | 0 | 0 | 100% |
| **Pack subtotal** | **35** | **25** | **25** | **0** | **10** | **100% (executed only)** |

### 6.1 Full Automated Suite (xUnit discovery)
| Metric | Value |
|---|---:|
| Total discovered automated tests | 52 |
| Passed | 52 |
| Failed | 0 |
| Skipped | 0 |

Update these values using latest run output before submission.

### 6.2 Defect-Driven Evidence (Fail -> Fix -> Re-test)
Use this table to show the testing mindset your lecturer wants: tests/scenarios that initially exposed failures, then fixes and verification.

| Defect ID | Failing scenario/test | Initial result | Root cause | Fix applied | Re-test result | Evidence |
|---|---|---|---|---|---|---|
| DEF-001 | App login flow with DB-backed repo (historical run) | Fail (`NpgsqlException`, connection refused) | Environment dependency (Postgres unavailable) | Switched to local JSON persistence path in app setup | Pass | EV-DEF-01 |
| DEF-002 | Manual UI workflow (topic/start controls clipped on smaller default windows) | Fail (button clipping) | Insufficient startup window sizing/layout constraints | Increased initial window dimensions and layout spacing | Pass | EV-DEF-02 |
| DEF-003 | Static analysis quality gate (historical warnings) | Fail (code smell warnings) | Unused variables/dead code patterns | Cleanup and refactor pass applied | Pass | EV-DEF-03 |

This section can include manual/system failures and is acceptable even if your final automated suite is all green.

---

## 7) Defect Log Template (WRIT1)

| Defect ID | Found In | Severity | Description | Repro Steps | Status | Linked Test ID |
|---|---|---|---|---|---|---|
| DEF-001 | Example | Medium | Example defect description | 1) ... 2) ... 3) ... | Fixed | TC-SYS-00X |

Tip: Include at least 2-4 real defects found during development/testing (even if already fixed).

---

## 8) Traceability Matrix (WRIT1)

| Requirement ID | Requirement | Test IDs | Evidence |
|---|---|---|---|
| FR1 | User login/create | TC-UNIT-012, TC-UNIT-013, TC-SYS-001 | EV-TEST-02, EV-UI-01 |
| FR2 | Topic selection and session start | TC-SYS-002, TC-SYS-003 | EV-UI-02, EV-UI-03 |
| FR3 | Reveal answer | TC-SYS-004 | EV-UI-04 |
| FR4 | Rating updates XP/stats | TC-UNIT-010, TC-SYS-005 | EV-TEST-02, EV-UI-05 |
| FR5 | Persist user data | TC-INT-001, TC-INT-003 | EV-TEST-03 |
| NFR1 | Reliability (bad/missing JSON) | TC-INT-006, TC-INT-007, TC-INT-008, TC-SEC-002, TC-SEC-003 | EV-TEST-03 |
| NFR2 | Performance | TC-PERF-001, TC-PERF-002 | EV-TEST-04 |
| NFR3 | Robust input handling | TC-UNIT-003, TC-UNIT-004, TC-UNIT-009, TC-SEC-001 | EV-TEST-01 |

---

## 9) Commands (for evidence capture)

```powershell
dotnet test .\FlashcardApp.Tests\FlashcardApp.Tests.csproj -c Debug -v minimal
```

Use your existing coverage workflow to generate and screenshot the report summary in `FlashcardApp.Tests/coverage_report/`.
