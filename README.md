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

## Development commands

From the repository root:

```powershell
dotnet build backend/LendingPlatform.slnx
dotnet test backend/LendingPlatform.Tests/LendingPlatform.Tests.csproj
```

Run the backend API in one terminal:

```powershell
dotnet run --project backend/LendingPlatform.Api/LendingPlatform.Api.csproj --launch-profile http
```

Run the React frontend in a second terminal:

```powershell
cd frontend
npm install
npm run dev
```

The frontend defaults to `http://localhost:5209`, the API address provided by the HTTP launch profile. To use a different API address, create `frontend/.env.local` with `VITE_API_BASE_URL=http://localhost:your-port`. The frontend and backend run as separate development processes.

## Scope

This repository includes the domain lending rules, an in-memory backend API, and a React lending workspace. It deliberately has no database or authentication.

## Statistics assumption

`totalLoanValue` represents the value of successful applications only. This treats a loan as written only after a successful decision. `meanLtv` includes both successful and declined applications.
