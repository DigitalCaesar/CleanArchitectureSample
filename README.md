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

### Domain Driven Design Refactoring

Steps
1. Create constructors in entities for exepected values
2. Move complex construction to static Create methods
3. Make constructors private to force use of Create method (why create versus constructor?)
4. Make property setters private
5. Move logic to methods in entity (i.e. SendInvitation)
6. Change lists to readonly

### Entity Creation
This lesson comes after introducing the prebuilt solution that already contains the BaseEntity.  The lesson rewinds the clock and isolates the initial entity creation.

Steps
1. Add unique identifier
2. Override Equals
3. Override GetHashCode (use multiple of prime number)
4. Add IEquatable
5. Entities inherit from BaseEntity

### Entity Validation

Strategies
1. Use Exceptions:  Easier debugging with stacktrace
2. Use Results: More expressive, house errors in catelog, self documenting, but does not halt execution making debugging more difficult

#### Exception Strategy

Steps
1. Create a base DomainException to identify Domain Exceptions
2. Create Exceptions inheriting from Domain Exceptions
3. Throw Exceptions defensively where ever domain state is theatened

#### Results Strategy

Steps
1. Create base Error
2. Create base Result
3. Create base Result<T>
4. Change return types that reference entities to Result<Entity> types
5. Replace Exceptions thrown to Result type of Failure with associated Error
6. Create Errors static class to house all Error types

## Credit
Milan Jovanovic
Clean Architecture & DDD Series
https://www.youtube.com/playlist?list=PLYpjLpq5ZDGstQ5afRz-34o_0dexr1RGa