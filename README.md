# MiAlma — Proposal Tracker

A small app for tracking proposals against RFPs. Backend is a .NET 8 Web API with PostgreSQL (EF Core, Clean Architecture), frontend is React + TypeScript with Vite.

## 1. How to run it

You'll need the .NET 8 SDK, Node.js (18+), and PostgreSQL running locally (no Docker involved).

### Backend

1. Create an empty database, e.g. `mialma`.
2. Open `src/MiAlma.Api/appsettings.Development.json` and update the connection string with your own PostgreSQL user/password:
   ```json
   "DefaultConnection": "Host=localhost;Port=5432;Database=mialma;Username=<your_user>;Password=<your_password>"
   ```
3. Run it, either from the terminal:
   ```bash
   cd src/MiAlma.Api
   dotnet run
   ```
   or by hitting Run in Visual Studio (both the `http` and `https` profiles work fine). On startup it applies the EF Core migrations and seeds some sample data by itself, so there's no manual `dotnet ef` step. It'll be listening on `http://localhost:5268`, with Swagger at `http://localhost:5268/swagger`. (There's also an HTTPS endpoint at `https://localhost:7148`, but the frontend talks to the plain HTTP one to avoid dealing with the local dev certificate.)

**Test user (already seeded):**
- Email: `test@mialma.dev`
- Password: `Password123!`

### Frontend

```bash
cd frontend
npm install
npm run dev
```

It expects the API at `http://localhost:5268` by default (see `.env.example`), and runs on `http://localhost:5173`. The backend's CORS is set up specifically for that port, so if it's already taken and Vite jumps to 5174/5175, the requests will get blocked — just free up 5173 first, or update the CORS origin in `Program.cs` if you'd rather change it.

Once it's running, open `http://localhost:5173`, log in with the test user above, and browse the seeded RFPs.

### Tests

```bash
dotnet test src/MiAlma.Tests/MiAlma.Tests.csproj
```

## 2. Where the lifecycle rules live, and why

The rules around how a proposal moves between statuses (`Draft → InReview → Submitted → Won|Lost`, one step at a time, no going backwards, and no editing once it's `Submitted`) are split across two places on purpose:

- `MiAlma.Domain/Policies/ProposalStatusPolicy.cs` holds the actual rules — which transitions are allowed from which status. It doesn't touch the database or any framework code, so it's easy to unit test on its own (see `MiAlma.Tests/Domain/ProposalStatusPolicyTests.cs`).
- The command handlers in `MiAlma.Application/Features/Proposals/Commands/` (`ChangeProposalStatusCommandHandler`, `UpdateProposalCommandHandler`) are the ones that actually call that policy and throw the right exception when something's not allowed (`InvalidStatusTransitionException`, `ProposalNotEditableException`, or `ForbiddenException` if you're not the owner).

The reasoning for keeping this in the Domain layer instead of the controller (or as a database constraint) is that it's a business rule, not something tied to HTTP or to how the data is stored — it should work the same way no matter who's calling it: a controller, a test, or something else down the line.

All of those exceptions get turned into proper HTTP status codes (400/403/404/401/409, never a bare 500) in one single spot: `MiAlma.Api/Middleware/ExceptionHandlingMiddleware.cs`. That keeps the error-handling logic out of the controllers and out of the handlers themselves.

## 3. What I'd do differently for production

Right now the JWT secret and the database password are sitting in plain text inside `appsettings.Development.json`. That file is gitignored, so it never actually made it into the repo, but it's still a local-only config file rather than a real secret store. For production I'd move both out to environment variables or a proper secrets manager (Azure Key Vault, AWS Secrets Manager, or at least `dotnet user-secrets` locally).

On the frontend, a few other things I'd want before this went in front of real users:
- Pick an actual UI/styling library instead of the plain hand-written CSS I used here, once there's a real design to work from.
- Add proposal templates, so what gets created looks more polished and professional when it's actually presented to a client, instead of just a free-text box.
- Swap the plain `<textarea>` for a real rich-text editor (formatting, headings, lists, etc.).

## 4. Time spent and help used

I used Claude Code and ChatGPT throughout this project — for architecture suggestions, picking compatible versions of Node and .NET, choosing the security library for password hashing (BCrypt), and getting a step-by-step plan for how to build everything (the backend in phases, the frontend in small incremental steps, each one committed separately). Claude Code also helped write some of the more repetitive, boilerplate-y parts of the code, like DTOs. I also leaned on a couple of my own past personal projects, both backend and frontend, for patterns and structure.

All in, this took me around **20 hours**.
