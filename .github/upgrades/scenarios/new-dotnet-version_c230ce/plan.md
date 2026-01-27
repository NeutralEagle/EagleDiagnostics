# .NET Version Upgrade Plan

Table of Contents
- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Plans](#project-by-project-plans)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Testing Strategy](#testing-strategy)
- [Risk Management](#risk-management)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)
- [Appendix & References](#appendix--references)

---

## Executive Summary

Scenario: Upgrade `EagleDiagnostics` solution from `.NET 6.0 (net6.0-windows)` to `net10.0-windows`.

Scope:
- Projects affected: 1 (`EagleDiagnostics\\EagleDiagnostics.csproj`)
- Current target: `net6.0-windows`
- Proposed target: `net10.0-windows`
- Total lines of code: 3,289
- NuGet packages identified: 2 (1 recommended update)

Key findings from assessment:
- API compatibility issues are concentrated in Windows Forms usage: ~2,770 binary-incompatible API references and 50 source-incompatible items.
- One NuGet package has a recommended update: `Newtonsoft.Json` 13.0.3 → 13.0.4. `ZedGraph` is reported compatible.
- Project is SDK-style WinForms application with no project-to-project dependencies.

Selected Strategy
**All-At-Once Strategy** — All projects upgraded simultaneously in a single atomic operation.

Rationale:
- Single project solution — suitable for All-At-Once.
- SDK-style WinForms project simplifies project file edits.
- Atomic upgrade reduces multi-pass complexity for widespread API breakages.

Primary risks:
- Large number of binary-incompatible Windows Forms APIs will require many code fixes in a single pass.
- UI behavioral changes require manual verification.


## Migration Strategy

Approach: All-At-Once (atomic) upgrade of the entire solution in one coordinated operation.

High-level steps (atomic operation):
1. Validate environment and prerequisites (SDK, `global.json`).
2. Ensure repo state: handle pending changes per the selected action (`commit`), create and switch to branch `upgrade-to-NET10`.
3. Update project files and any imported MSBuild props/targets:
   - Set `TargetFramework` to `net10.0-windows` in `EagleDiagnostics\\EagleDiagnostics.csproj`.
   - Ensure WinForms support: add `<UseWindowsForms>true</UseWindowsForms>` if not already present.
   - Inspect and update `Directory.Build.props`, `Directory.Packages.props`, or other imported files for conditional logic and package version management.
4. Update NuGet package references per assessment (see §Package Update Reference).
5. Restore packages and build solution to surface compile-time errors.
6. Fix compilation errors and API incompatibilities discovered during build (single pass).
7. Rebuild and verify solution builds with zero errors.
8. Run test projects (if any) and execute UI validation checklist.

Notes:
- All changes are prepared on branch `upgrade-to-NET10` and recorded as the atomic upgrade commit(s) following All-At-Once guidance.
- This plan documents the exact changes to be made; execution must be performed by an executor agent or developer.


## Detailed Dependency Analysis

Summary:
- Total projects: 1
- Project dependency graph: trivial (no project-to-project references)
- Migration order (dependency-respecting): only `EagleDiagnostics\\EagleDiagnostics.csproj` — it is both a leaf and a root node.

Implication:
- No dependency ordering constraints across projects. The single-project scope simplifies the atomic upgrade.


## Project-by-Project Plans

### Project: `EagleDiagnostics\\EagleDiagnostics.csproj`

Current State:
- Target framework: `net6.0-windows`
- SDK-style: Yes
- Project type: WinForms desktop app
- Files: 18 (14 affected)
- Lines of code: 3,289
- Risk level: High (extensive binary-incompatible API surface and high LOC impact)

Target State:
- Target framework: `net10.0-windows`
- Packages updated per §Package Update Reference
- Project builds and unit/integration tests (if any) pass

Migration Steps (atomic operation):
1. Prerequisites
   - Validate .NET 10 SDK is installed on the machine(s) performing the upgrade. If `global.json` exists, ensure SDK version is compatible or update `global.json`.
   - Commit or stash pending local changes as configured (default: `commit`).
   - Create branch: `upgrade-to-NET10` from `master` and switch to it.
2. Project file updates (single coordinated change across repository)
   - Edit `EagleDiagnostics\\EagleDiagnostics.csproj`:
     - Change `<TargetFramework>net6.0-windows</TargetFramework>` to `<TargetFramework>net10.0-windows</TargetFramework>`.
     - Ensure `<UseWindowsForms>true</UseWindowsForms>` is present.
     - Review any conditional compilation symbols and adjust if needed (e.g., `NET6_0` → `NET10_0` or use `NET`-style conditions).
   - Inspect `Directory.Build.props`, `Directory.Packages.props`, and imported MSBuild fragments for TFM or package constraints; update accordingly.
3. Package updates
   - Apply package updates listed in §Package Update Reference across the project (atomic change).
4. Restore & Build
   - Restore NuGet packages and build the solution to produce compilation diagnostics.
5. Fix compilation errors
   - Address all compile-time errors resulting from API breaking changes and package updates in this single pass.
   - Prefer API replacements and minimal behavior-preserving changes where possible.
6. Rebuild and verify
   - Rebuild solution; target: zero compilation errors.
7. Test and validate
   - Run automated tests (if present) and execute UI validation checklist (see §Testing Strategy).
8. Finalize
   - When build and tests are green, prepare single atomic commit summarizing all project changes and package updates.

Validation Checklist (per-project):
- [ ] `TargetFramework` changed to `net10.0-windows` in `EagleDiagnostics\\EagleDiagnostics.csproj`.
- [ ] `<UseWindowsForms>true</UseWindowsForms>` present.
- [ ] All package updates from assessment applied (see §Package Update Reference).
- [ ] Solution builds with zero errors.
- [ ] Unit/integration tests pass (if any).
- [ ] No outstanding critical security advisories for packages used.


## Package Update Reference

### Aggregate updates (from assessment)
- `Newtonsoft.Json`: 13.0.3 → 13.0.4 (applies to: `EagleDiagnostics\\EagleDiagnostics.csproj`) — Reason: recommended update in assessment. No known breaking changes between these patch versions.
- `ZedGraph`: 5.2.0 — reported compatible; no update required per assessment.

Notes:
- Apply the exact target versions from assessment. Do not use `latest` or floating versions.
- If `Directory.Packages.props` central package management is used, update it there and not per-project.


## Breaking Changes Catalog

Overview:
- The most significant breaking change category is Windows Forms binary incompatibilities. The assessment identified many WinForms types and members that may be binary-incompatible or require recompilation.

Areas to review and common fixes:
- Windows Forms controls (e.g., `ToolStripMenuItem`, `ComboBox`, `Button`, `Label`, `TextBox`, `ListBox`) — expect compile-time errors where API signatures or access modifiers changed. Fix by updating calls to current APIs, replacing removed members, or adjusting overload usage.
- Control collection manipulation (e.g., `Control.Controls`, `Control.ControlCollection.Add/Remove`) — validate collection usage and update code to supported APIs.
- Events and delegates — event signatures might change; update subscriber code accordingly.
- Properties such as `Location`, `Size`, `Name`, `Text` — validate that property types or behavior remain compatible.
- `System.Drawing` / GDI+ usage — consider replacing `System.Drawing` usage with `System.Drawing.Common` where needed on non-Windows targets; since this is Windows-only app, ensure `System.Drawing.Common` usage is supported on the target platform.
- `app.config` / configuration API churn — migrate to `Microsoft.Extensions.Configuration` if refactoring; otherwise add `System.Configuration.ConfigurationManager` package as interim.

Specific expected compilation symptoms:
- Missing members (CS0117) referring to removed/renamed members.
- Ambiguous call or overload resolution errors due to changed signatures.
- Obsolete API warnings elevated to errors if project treats warnings as errors.

Recommendations for resolving breaking changes:
- Prefer minimal change replacements: adapt call sites, use new overloads, replace deprecated members with recommended alternatives.
- For UI behavior regressions, capture screenshots and test scenarios to compare pre/post behavior.
- When changes are extensive, consider small helper adapters to isolate compatibility adjustments.


## Testing Strategy

Goals:
- Provide confidence that upgrade does not break functionality and that code compiles and tests pass.

Levels:
1. Build verification (required): ensure solution builds successfully with zero compilation errors.
2. Automated tests (if any): run unit and integration tests. Record results.
3. UI verification (manual): exercise core UI workflows (open main window, load data, interact with critical controls listed below).

UI validation checklist (manual):
- Launch application main window.
- Verify menus and ToolStrip items render and respond.
- Verify data plotting controls (ZedGraph usage) render expected graphs.
- Verify common dialogs (OpenFileDialog, SaveFileDialog) function.
- Verify input controls (ComboBox, TextBox, Buttons) accept input and events fire.

Test projects discovered: none reported in assessment; if test projects exist, include them in automated run step.


## Risk Management

Risk summary for `EagleDiagnostics`:
- Risk: High — large portion of codebase impacted by WinForms API incompatibilities.
  - Mitigation:
    - Run the entire upgrade in an isolated branch (`upgrade-to-NET10`).
    - Commit pending changes prior to starting.
    - Keep original branch intact to allow rollback.
    - Allocate time in the execution phase for focused compile-fix pass across UI code.
    - Use code search to find patterns (e.g., `ToolStripMenuItem`, `ComboBox`) and batch-fix replacements.
    - Keep automated unit tests (if any) running and passing.
- Risk: UI behavioral regressions.
  - Mitigation: create UI validation checklist and capture pre-upgrade screenshots where feasible.
- Risk: Package compatibility or transitive dependency changes.
  - Mitigation: Lock package versions to the assessment-specified targets and verify with `dotnet restore` and `dotnet build`.

Contingency / Rollback:
- If the upgrade introduces unrecoverable regressions, abort the branch and return to `master`. Use the pre-upgrade commit as checkpoint.


## Complexity & Effort Assessment

Per-project complexity (relative):
- `EagleDiagnostics\\EagleDiagnostics.csproj` — Complexity: High
  - Reason: ~86% of LOC impacted and 2,770 binary-incompatible API findings focused on Windows Forms.

No time estimates included. Plan assumes a single atomic pass to perform all code changes.


## Source Control Strategy

Branching:
- Starting branch: `master` (current)
- Upgrade branch: `upgrade-to-NET10` — create from `master` after committing pending changes.

Commits:
- All code changes related to the upgrade should be made on `upgrade-to-NET10`.
- Prefer a single atomic commit for the project file and package reference changes together with compilation fixes, or a small set of logically grouped commits if needed for review. The plan recommends a single atomic commit when feasible to follow All-At-Once guidelines.

Pull Request / Review:
- Create a PR from `upgrade-to-NET10` into `master` once validation criteria are met. The PR should include:
  - Link to this `plan.md` and `assessment.md` files
  - Checklist of validation items
  - Notes on any manual verification performed


## Success Criteria

The migration is considered complete when all the following are true:
- `EagleDiagnostics\\EagleDiagnostics.csproj` targets `net10.0-windows`.
- All package updates listed in §Package Update Reference are applied.
- Solution builds with zero compilation errors.
- Automated tests (if any) pass.
- Manual UI validation checklist completed and signed off.
- No outstanding critical security vulnerabilities in NuGet packages used.


## Appendix & References

- Assessment file: `.github/upgrades/scenarios/new-dotnet-version_c230ce/assessment.md`
- Suggested branch name: `upgrade-to-NET10`
- Key files to edit: `EagleDiagnostics\\EagleDiagnostics.csproj`, `Directory.Build.props`, `Directory.Packages.props` (if present)


---

Plan prepared for All-At-Once upgrade to `net10.0-windows`.
