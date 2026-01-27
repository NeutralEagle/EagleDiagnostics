# EagleDiagnostics .NET 10 Upgrade Tasks

## Overview

This document lists executable tasks to upgrade the `EagleDiagnostics` solution from `net6.0-windows` to `net10.0-windows`. Tasks implement the plan's prerequisites checks, the coordinated project/package upgrade and compilation fixes, automated validation per the plan, and the final commit.

**Progress**: 0/4 tasks complete (0%) ![0%](https://progress-bar.xyz/0)

---

## Tasks

### [▶] TASK-001: Verify prerequisites
**References**: Plan §Prerequisites, Plan §Project-by-Project Plans

- [ ] (1) Verify required .NET SDK (10.x) is installed on the execution host per Plan §Prerequisites
- [ ] (2) Installed runtime/SDK version meets minimum requirements (**Verify**)
- [ ] (3) If a `global.json` file is present, verify its SDK version is compatible with the required .NET 10 SDK per Plan §Prerequisites (**Verify**)
- [ ] (4) Verify required tooling (`dotnet` CLI) is available and at a compatible version (**Verify**)

### [ ] TASK-002: Atomic project and package upgrade with compilation fixes
**References**: Plan §Project-by-Project Plans, Plan §Package Update Reference, Plan §Breaking Changes Catalog

- [ ] (1) Update `EagleDiagnostics\EagleDiagnostics.csproj`: set `<TargetFramework>net10.0-windows</TargetFramework>` and ensure `<UseWindowsForms>true</UseWindowsForms>` per Plan §Project-by-Project Plans
- [ ] (2) Inspect and update imported MSBuild files if present (`Directory.Build.props`, `Directory.Packages.props`) for TFM or package constraints per Plan §Project-by-Project Plans and Plan §Package Update Reference
- [ ] (3) Apply package reference changes listed in Plan §Package Update Reference (e.g., `Newtonsoft.Json` → `13.0.4`), updating central package management files if used per Plan §Package Update Reference
- [ ] (4) Restore dependencies (`dotnet restore`) for the solution per Plan §Project-by-Project Plans
- [ ] (5) All dependencies restored successfully (**Verify**)
- [ ] (6) Build the solution to identify compilation errors (first-pass build) per Plan §Breaking Changes Catalog
- [ ] (7) Fix all compilation errors caused by framework and package upgrades using remedies from Plan §Breaking Changes Catalog (single bounded pass)
- [ ] (8) Rebuild the solution to verify fixes applied
- [ ] (9) Solution builds with 0 errors (**Verify**)

### [ ] TASK-003: Automated test validation (per plan)
**References**: Plan §Testing Strategy

- [ ] (1) Confirm Plan §Testing Strategy lists test projects; Plan reports no test projects discovered. Since no test projects are listed in the plan, skip automated test execution (**Verify**)
- [ ] (2) If test projects are later added to the plan, run those test projects, fix any failures, re-run tests, and ensure all tests pass with 0 failures per Plan §Testing Strategy (**Verify**)

### [ ] TASK-004: Final commit
**References**: Plan §Source Control Strategy

- [ ] (1) Commit all upgrade changes (project file edits, package updates, compilation fixes) with message: "TASK-004: Complete upgrade to `net10.0-windows`"
