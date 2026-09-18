# Lending Platform

A full-stack technical-assessment project for evaluating secured lending applications. The system accepts a loan application, calculates loan-to-value (LTV), returns a lending decision, and presents in-memory portfolio statistics through a React dashboard.

---

## Features

- Domain-driven lending decisions with boundary-focused xUnit tests
- ASP.NET Core REST API with input validation and in-memory application storage
- Portfolio statistics for successful/declined applications, loan value, and mean LTV
- Responsive React lending workspace with form validation, results, loading, and error states
- Professional banking/lending-themed user interface
- Clear separation between frontend, API, and domain business logic

---

## Technology Stack

- **Backend:** C# / ASP.NET Core Web API (.NET 10)
- **Frontend:** React with Vite
- **Testing:** xUnit
- **Development:** VS Code, Git

---

## Project Structure

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

---

# 🚀 How to Run Locally

Follow these steps from the beginning if you are running this project on a new computer.

## 1. Prerequisites

Install the following software before running the project:

- **Git**
- **.NET 10 SDK**
- **Node.js**
- **npm**

You can verify the installations by opening a terminal and running:

```powershell
git --version
dotnet --version
node --version
npm --version
```

The commands should return the installed versions.

---

## 2. Clone the Repository

Open PowerShell, Command Prompt, or a terminal and run:

```powershell
git clone https://github.com/golandeabhay947-create/LendingPlatform_Thrive_Assesment.git
```

Then move into the project directory:

```powershell
cd LendingPlatform_Thrive_Assesment
```

---

## 3. Open the Project

If you are using VS Code:

```powershell
code .
```

The project contains separate backend and frontend applications.

---

## 4. Start the Backend API

Open the **first terminal** in the project root.

Run:

```powershell
dotnet run --project backend/LendingPlatform.Api/LendingPlatform.Api.csproj --launch-profile http
```

The API should start on:

```text
http://localhost:5209
```

Keep this terminal running.

---

## 5. Start the React Frontend

Open a **second terminal** in the project root.

Move into the frontend directory:

```powershell
cd frontend
```

Install the frontend dependencies:

```powershell
npm install
```

Start the React development server:

```powershell
npm run dev
```

Vite will display the frontend address in the terminal. Normally it is:

```text
http://localhost:5173
```

Open the displayed address in a web browser.

---

## 6. Running the Complete Application

Both applications must be running at the same time.

```text
┌─────────────────────────────┐
│       React Frontend        │
│     http://localhost:5173   │
└──────────────┬──────────────┘
               │
               │ HTTP API
               ▼
┌─────────────────────────────┐
│     ASP.NET Core API        │
│     http://localhost:5209   │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│   Domain Lending Service    │
│   Loan Decision Rules       │
└─────────────────────────────┘
```

### Terminal 1 — Backend

```powershell
dotnet run --project backend/LendingPlatform.Api/LendingPlatform.Api.csproj --launch-profile http
```

### Terminal 2 — Frontend

```powershell
cd frontend
npm install
npm run dev
```

Then open:

```text
http://localhost:5173
```

---

## 7. Using the Application

Once the frontend is open:

1. Enter the **Loan Amount (£)**.
2. Enter the **Secured Asset Value (£)**.
3. Enter the **Applicant Credit Score**.
4. Submit the application.
5. The frontend sends the application to the backend API.
6. The backend calculates the LTV and applies the lending rules.
7. The application result displays:
   - Successful or Declined
   - Calculated LTV
8. Portfolio statistics are displayed on the dashboard.

The backend is the **single source of truth** for lending decisions. The frontend does not duplicate the lending rules.

---

## 8. API Configuration

The frontend API client defaults to:

```text
http://localhost:5209
```

To use a different backend port, create:

```text
frontend/.env.local
```

Add:

```text
VITE_API_BASE_URL=http://localhost:your-port
```

Replace `your-port` with the actual backend port.

For example:

```text
VITE_API_BASE_URL=http://localhost:5000
```

---

# Lending Rules

The backend is the sole source of truth for these rules.

- Decline loans below **£100,000** or above **£1,500,000**.
- For loans of **£1,000,000 or more**:
  - LTV must be at most **60%**
  - Credit score must be at least **950**
- For loans below **£1,000,000**:
  - LTV below **60%** requires credit score **750 or higher**
  - LTV from **60% to below 80%** requires credit score **800 or higher**
  - LTV from **80% to below 90%** requires credit score **900 or higher**
  - LTV of **90% or more** is declined

### LTV Formula

```text
LTV = (loan amount / secured asset value) × 100
```

---

# API

## Submit a Loan Application

```text
POST /api/loans/applications
```

Submits a loan application and returns its lending decision and LTV.

### Example Request

```json
{
  "loanAmount": 500000,
  "assetValue": 1000000,
  "creditScore": 800
}
```

### Example Response

```json
{
  "decision": "Successful",
  "ltv": 50.0
}
```

---

## Get Portfolio Statistics

```text
GET /api/loans/statistics
```

Returns the current in-memory portfolio statistics.

---

# Statistics

Statistics are calculated from the applications currently held in memory.

- `successfulApplications` — number of successful applications
- `declinedApplications` — number of declined applications
- `totalLoanValue` — sum of successful applications only; a loan is treated as written after a successful decision
- `meanLtv` — mean LTV across both successful and declined applications
- When no applications have been submitted, all statistics return `0`

### Important

The application data is stored **in memory**.

Restarting the API clears the applications and resets the statistics.

---

# Testing

From the repository root:

## Backend Build

```powershell
dotnet build backend/LendingPlatform.slnx
```

## Backend Tests

```powershell
dotnet test backend/LendingPlatform.Tests/LendingPlatform.Tests.csproj
```

## Frontend Production Build

```powershell
cd frontend
npm run build
```

The project includes automated tests covering lending decisions, boundary conditions, validation, application storage, and statistics.

---

# Important Assumptions

- Loan amount and asset value must be greater than zero.
- Credit score must be a whole number between **1 and 999**.
- This assessment intentionally uses **in-memory storage**.
- There is no database, authentication, or persistent state.
- A successful application is treated as a loan written for the purpose of calculating `totalLoanValue`.
- The mean LTV includes both successful and declined applications.

---

# Development Notes

The application follows a separation-of-concerns approach:

```text
React Frontend
      ↓
ASP.NET Core API Controller
      ↓
Application / In-Memory Store
      ↓
Domain LoanDecisionService
      ↓
Lending Business Rules
```

The lending rules are implemented in the backend domain layer so that the business logic is not duplicated in the React frontend.

---

# AI Assistance

AI tools were used during development as required by the technical assessment.

The development process, key prompts, testing, human review, iterations, and corrections are documented in:

```text
AI-LOG.md
```

The AI log records the actual development process rather than fabricated interactions.

---

# Project Status

The project has been verified with:

- Backend build passing
- xUnit tests passing
- React production build passing
- Successful loan application tested
- Declined loan application tested
- Invalid input and HTTP 400 validation tested
- Statistics endpoint tested
- CORS preflight tested
- Responsive desktop and mobile layouts checked
- No functional issues identified during final verification