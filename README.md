# Lending Platform

A full-stack technical-assessment project for evaluating secured lending applications. The system accepts a loan application, calculates loan-to-value (LTV), returns a lending decision, and presents in-memory portfolio statistics through a React dashboard.

## Features

- Domain-driven lending decisions with boundary-focused xUnit tests
- ASP.NET Core REST API with input validation and in-memory application storage
- Portfolio statistics for successful/declined applications, loan value, and mean LTV
- Responsive React lending workspace with form validation, results, loading, and error states

## Technology stack

- Backend: C# / ASP.NET Core Web API (.NET 10)
- Frontend: React with Vite
- Testing: xUnit
- Development: VS Code, Git

## Project structure

```text
LendingPlatform/
├── backend/
│   ├── LendingPlatform.Api/        # Controllers, API models, in-memory store
│   ├── LendingPlatform.Domain/     # Loan entity, decision enum, business service
│   ├── LendingPlatform.Tests/      # Domain and application-layer tests
│   └── LendingPlatform.slnx
├── frontend/
│   └── src/                        # React UI, components, and API client
├── README.md
└── AI-LOG.md
```

## Lending rules

The backend is the sole source of truth for these rules:

- Decline loans below £100,000 or above £1,500,000.
- For loans of £1,000,000 or more: LTV must be at most 60% and credit score at least 950.
- For loans below £1,000,000:
  - LTV below 60% requires credit score 750 or higher.
  - LTV from 60% to below 80% requires credit score 800 or higher.
  - LTV from 80% to below 90% requires credit score 900 or higher.
  - LTV of 90% or more is declined.

`LTV = (loan amount / secured asset value) × 100`

## Run locally

Prerequisites: .NET 10 SDK, Node.js, and npm.

Start the API in one terminal:

```powershell
dotnet run --project backend/LendingPlatform.Api/LendingPlatform.Api.csproj --launch-profile http
```

Start the frontend in another terminal:

```powershell
cd frontend
npm install
npm run dev
```

- Backend URL: `http://localhost:5209`
- Frontend URL: `http://localhost:5173` (or the Vite address printed in the terminal)

The frontend and backend run as separate development processes. The API client defaults to `http://localhost:5209`. To override it, create `frontend/.env.local`:

```text
VITE_API_BASE_URL=http://localhost:your-port
```

## API

| Method | Endpoint | Purpose |
| --- | --- | --- |
| POST | `/api/loans/applications` | Submit an application and return its decision and LTV |
| GET | `/api/loans/statistics` | Return in-memory portfolio statistics |

Example request:

```json
POST /api/loans/applications

{
  "loanAmount": 500000,
  "assetValue": 1000000,
  "creditScore": 800
}
```

Example response:

```json
{
  "decision": "Successful",
  "ltv": 50.0
}
```

## Statistics

Statistics are calculated from the applications currently held in memory:

- `successfulApplications` and `declinedApplications` are counts by decision.
- `totalLoanValue` is the sum of successful applications only; a loan is treated as written after a successful decision.
- `meanLtv` includes both successful and declined applications.
- When no applications have been submitted, all statistics return `0`.

Restarting the API clears the in-memory applications.

## Testing

From the repository root:

```powershell
dotnet build backend/LendingPlatform.slnx
dotnet test backend/LendingPlatform.Tests/LendingPlatform.Tests.csproj
cd frontend
npm run build
```

## Important assumptions

- Loan amount and asset value must be greater than zero.
- Credit score must be a whole number between 1 and 999.
- This assessment intentionally uses in-memory storage: it has no database, authentication, or persistent state.
