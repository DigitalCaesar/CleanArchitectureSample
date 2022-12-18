# CleanArchitectureSample
A sample for best practices and experimenting with Clean Architecture and CQRS


## Structure

Root
- source : Source files for the solution containing the solutions and all related projects
- tests : All test projects for the solution

Source
- Presentation : All of the client facing application projects (console, web, api)
- Core : All of the application and domain logic
- Infrastructure : All of the low level services (persistence, messaging, authentication)

Tests
- Architecture Tests : Ensures the project dependencies are respected as expected
- Integration Tests : Higher level tests for interdependent projects
- Unit Tests : Low level code tests

## Lessons
These lessons assume you start with a pre-existing project using CQRS and an anemic model.  Each lesson refactors the previous iteration starting with the initial anemic solution.

Lesson List
-----------
1. Project Setup
2. Architecture Tests
3. DDD
4. Entity
5. Validation
6. Value Objects
7. Aggregate Root
8. Domain Event
9. Outbox
10. Email Uniqueness
11. CQRS
12. MediatR Validation
13. Polly Outbox Retry
14. Idempotent Handlers
15. Unit Test CQRS
16. Unit Test Entity Framework
17. Smart Enums
18. Functional Programming

### Project Setup

Source Detail
- Core Layer
-- Application
--- Abstractions
--- Behaviors
--- Contracts
--- Exceptions
--- Features
-- Domain
--- Entities
--- Events
--- Exceptions
--- Shared
--- ValueObject
-Infrastructure
-- Data
-- Messaging
- Presenation
-- Api
--- Controllers
--- Middleware

### Architecture Tests

Steps
1. Create XUnit Test
2. Add NetArchTest.Rules
3. Add Assembly reference (empty class) to all projects
4. Test each project for expected dependencies
5. Test each project for unexpected dependencies

### Domain Driven Design

Steps
1. Create constructors in entities for exepected values
2. Move complex construction to static Create methods
3. Make constructors private to force use of Create method (why create versus constructor?)
4. Make property setters private
5. Move logic to methods in entity (i.e. SendInvitation)
6. Change lists to readonly

## Credit
Milan Jovanovic
Clean Architecture & DDD Series
https://www.youtube.com/playlist?list=PLYpjLpq5ZDGstQ5afRz-34o_0dexr1RGa