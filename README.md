# MiAlma — Proposal Tracker

A small proposal-tracking platform for RFPs. Backend built with .NET 8 Web API + PostgreSQL (EF Core, code-first, Clean Architecture), frontend with React + TypeScript (Vite).

## 1. How to run it

Prerequisites: .NET 8 SDK, Node.js (18+), and a PostgreSQL instance running locally (no Docker).

### Backend

1. Create an empty database in your local PostgreSQL instance, e.g. `mialma`.
2. Edit `src/MiAlma.Api/appsettings.Development.json` and set `ConnectionStrings:DefaultConnection` with your real username/password:
   ```json
   "DefaultConnection": "Host=localhost;Port=5432;Database=mialma;Username=<your_user>;Password=<your_password>"
   ```
3. Run it either from the terminal or from Visual Studio:
   ```bash
   cd src/MiAlma.Api
   dotnet run
   ```
   or press Run in Visual Studio (either the `http` or `https` launch profile works). On startup, the app automatically applies EF Core migrations and seeds sample data (no need to run `dotnet ef` by hand). The API is available over plain HTTP at `http://localhost:5268` in both launch profiles, with Swagger at `http://localhost:5268/swagger`. (The `https` profile also exposes `https://localhost:7148`, but the frontend is configured to talk to the HTTP port to avoid local dev-certificate trust issues.)

**Seeded test user:**
- Email: `test@mialma.dev`
- Password: `Password123!`

### Frontend

```bash
cd frontend
npm install
npm run dev
```

Defaults to `VITE_API_URL=http://localhost:5268` (see `.env.example`). Runs on `http://localhost:5173` — the backend has CORS enabled specifically for that origin, so if you change the frontend port you'll also need to update `Program.cs`. If port 5173 is already taken, Vite will silently pick the next free one (5174, 5175, ...) and CORS will then reject the requests — free up 5173 first, or update `Program.cs` to match.

Open `http://localhost:5173`, log in with the test user, and browse the seeded RFPs.

### Tests

```bash
dotnet test src/MiAlma.Tests/MiAlma.Tests.csproj
```

## 2. Where the lifecycle rules are enforced, and why

The transition rules (`Draft → InReview → Submitted → Won|Lost`, only one step forward at a time, no going back, and no editing title/content once `Submitted`) live in two deliberately separate places:

- **`MiAlma.Domain/Policies/ProposalStatusPolicy.cs`**: the matrix of valid transitions. It's pure business logic with no dependency on infrastructure or MediatR, so it can be tested in isolation (see `MiAlma.Tests/Domain/ProposalStatusPolicyTests.cs`).
- **`MiAlma.Application/Features/Proposals/Commands/*Handler.cs`**: the command handlers (`ChangeProposalStatusCommandHandler`, `UpdateProposalCommandHandler`) are the ones that call that policy and decide which domain exception to throw (`InvalidStatusTransitionException`, `ProposalNotEditableException`, `ForbiddenException` for the ownership check).

I put the policy in Domain instead of the controller or the database because it's a *business* rule, not an HTTP transport detail or a persistence concern: it needs to be evaluable without an `HttpContext` or a PostgreSQL connection, and it must behave the same whether the caller is a REST controller, a test, or (in the future) a background job.

Domain exceptions are translated into the correct HTTP status codes (400/403/404/401/409, never 500 for these cases) in a single place: `MiAlma.Api/Middleware/ExceptionHandlingMiddleware.cs`. That way the handlers know nothing about HTTP, and controllers don't repeat `try/catch` in every action.

## 3. What I'd do differently for production

Right now the JWT signing secret (`Jwt:Key`) and the database password live in plain text in `appsettings.Development.json`, which is also committed to the repo. For production, the first thing I'd pull out of there would be moving both to environment variables or a secrets manager (Azure Key Vault, AWS Secrets Manager, or at minimum `dotnet user-secrets` for local development), and rotating the current JWT key since it's already been exposed in the git history.

## 4. Time spent and help used

This project was built iteratively with **Claude Code** (Anthropic) as a pair-programming assistant across several phases (backend: EF Core infrastructure, JWT auth, proposals CRUD; frontend: 10 incremental steps, each in its own commit). The assistant generated most of the codebase, ran builds/tests and end-to-end verification against the real backend at each step, and I reviewed, gave design direction, and approved each commit before moving on.

_(Fill in your actual estimate of hours spent reviewing/directing the work here — the repo's commit history has the step-by-step breakdown if that's useful as a reference.)_
