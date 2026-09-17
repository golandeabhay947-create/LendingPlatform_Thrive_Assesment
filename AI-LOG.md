## Step 1 — Initial Project Setup

AI Tool: Codex

### Prompt Purpose
Create the initial full-stack project structure using ASP.NET Core Web API, React, and xUnit.

### What Codex Did
- Created the ASP.NET Core Web API project.
- Created the Domain class library.
- Created the xUnit test project.
- Created the React frontend.
- Created README.md, AI-LOG.md, .gitignore, and NuGet.Config.
- Initialized the Git repository.

### Testing / Verification
- Backend build: Passed
- xUnit tests: Passed (1/1)
- React development server: Passed
- React production build: Passed

### Human Review
Verified that the project builds and runs successfully.
No lending business logic, LTV calculation, statistics, database, authentication, or loan UI was added in this step.

### Corrections / Iterations
No corrections were required during the initial setup.




## Step 2 — Domain Model, LTV and Loan Decision Logic

AI Tool: Codex

### Prompt Purpose
Implement the domain model, LTV calculation, loan decision rules, input validation, and unit tests without adding API or frontend functionality.

### What Codex Did
- Created LoanApplication entity.
- Created LoanDecision enum.
- Created LoanDecisionService.
- Implemented LTV calculation.
- Implemented the supplied lending decision rules.
- Added validation for loan amount, asset value, and credit score.
- Created comprehensive xUnit tests.
- Removed the default UnitTest1.cs placeholder.

### Testing / Verification
- Build: Passed
- Warnings: 0
- Errors: 0
- Tests: 25 passed
- Failed: 0
- Skipped: 0

### Human Review
Reviewed the reported business rules and boundary cases.
The logic correctly distinguishes:
- LTV < 60% from LTV = 60% for loans below £1,000,000.
- LTV <= 60% for loans of £1,000,000 or more.
- LTV >= 90% as declined for loans below £1,000,000.

### Iteration / Correction
The initial full build encountered a locked API executable from the earlier setup.
The running local API process was stopped and the build was rerun successfully.

### Assumptions
- Loan amount <= 0 is treated as technically invalid.
- Asset value <= 0 is invalid because LTV cannot be calculated.
- Credit score must be between 1 and 999.
- Validation occurs before lending decision rules.
- Successful means the supplied lending rules are satisfied only.



## Step 3 — Backend API and In-Memory Statistics

AI Tool: Codex

### Prompt Purpose
Connect the existing domain logic to an ASP.NET Core Web API and add
in-memory application storage and statistics.

### What Codex Did
- Added POST /api/loans/applications.
- Added GET /api/loans/statistics.
- Added request and response models.
- Added in-memory application storage.
- Connected the API to the existing LoanDecisionService.
- Added dependency injection.
- Added API/application-layer tests.
- Added validation for invalid input.
- Kept lending business rules in the Domain layer.
- Did not modify the React frontend.

### Testing / Verification
- Solution build: Passed
- Warnings: 0
- Errors: 0
- xUnit tests: 35 passed
- Failed: 0

Manual API verification:
- Successful application: Passed
- Declined application: Passed
- Statistics: Passed
- Invalid input / HTTP 400: Passed

### Statistics Verification
After one successful 50% LTV application and one declined 80% LTV
application:

successfulApplications = 1
declinedApplications = 1
totalLoanValue = 500000
meanLtv = 65.0

This confirmed that mean LTV includes both successful and declined
applications.

### Iteration / Correction
The initial live API verification encountered an HTTPS-redirection
configuration issue when running the API on an explicit HTTP port.
The unused HTTPS redirection was removed and the build, tests, and
live API checks were rerun successfully.

### Assumptions
- totalLoanValue means the sum of successful applications only.
- meanLtv includes successful and declined applications.
- With no applications, statistics return 0, including mean LTV.