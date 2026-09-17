# Lending Platform

Starter repository for a full-stack lending-platform technical assessment.

## Structure

- `backend/LendingPlatform.Api` — ASP.NET Core Web API host. Future HTTP controllers belong in `Controllers`.
- `backend/LendingPlatform.Domain` — domain/business layer, intentionally empty at setup stage.
- `backend/LendingPlatform.Tests` — xUnit test project for domain tests.
- `frontend` — React starter application.

## Prerequisites

- .NET 10 SDK
- Node.js and npm

## Commands

From the repository root:

```powershell
dotnet build backend/LendingPlatform.slnx
dotnet test backend/LendingPlatform.Tests/LendingPlatform.Tests.csproj
cd frontend
npm install
npm run dev
```

The React development server prints the local address to open in a browser.

## Scope

This repository includes the domain lending rules and an in-memory backend API. It deliberately has no database, authentication, backend/frontend connection, or loan UI.

## Statistics assumption

`totalLoanValue` represents the value of successful applications only. This treats a loan as written only after a successful decision. `meanLtv` includes both successful and declined applications.
