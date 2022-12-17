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

Tests
- Architecture Tests : Ensures the project dependencies are respected as expected
- Integration Tests : Higher level tests for interdependent projects
- Unit Tests : Low level code tests
