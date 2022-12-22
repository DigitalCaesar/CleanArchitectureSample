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

Architecture tests allow you to perform higher level checks on your solution.  With architecture tests, you can check dependencies to ensure projects are referring to to the right projects/packages.  This can flag unexpected issues if your dependencies change and help to avoid circular references.

Steps
1. Create XUnit Test
2. Add NetArchTest.Rules
3. Add Assembly reference (empty class) to all projects
4. Test each project for expected dependencies
5. Test each project for unexpected dependencies

### Domain Driven Design Refactoring

Refactoring starts with a working anemic model to demonstrate steps to change to a more DDD approach.  The idea is that you start with a quick and dirty anemic model and then refactor the system with DDD techniques to improve the system.  

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

At this stage, we talk about validation in terms of our strategy for handling invalid state.  Should we throw an exception if there is a problem or return a result.  Exceptions are easy to debug but hard in production.  Results are hard to debug but easy in production.

Strategies
1. Use Exceptions:  Easier debugging with stacktrace
2. Use Results: More expressive, house errors in catelog, self documenting, but does not halt execution making debugging more difficult

#### Exception Strategy

Exceptions can be easier for debugging because the execution will stop at the exception and you will have a stack trace.  This makes debugging easy because you always know where the failure happened immediately.  The downside is you need a try/catch at any point where you call the procedure to avoid creating invalid states and crashing the program.

Steps
1. Create a base DomainException to identify Domain Exceptions
2. Create Exceptions inheriting from Domain Exceptions
3. Throw Exceptions defensively where ever domain state is theatened

#### Results Strategy

Results allow you to handle conditions where returning an object may be optional and result in success or failure.  For example, if you search for an entity in the database, it is possible that you retrieve an object or the object is not found.  Results gives you a way to return a null object and check for success in the calling method.  Further, you can return meaningful error statements that describe what happened.  
The downside is returning a Result type is more difficult to debug.  This strategy does not stop execution of the program while debugging.  You need to set breakpoints to track failures and log errors.  Debugging would involve analyzing the logs to determine where the error took place and then setting breakpoints to pause execution in order to analyze the state of the system.  

Steps
1. Create base Error
2. Create base Result
3. Create base Result<T>
4. Change return types that reference entities to Result<Entity> types
5. Replace Exceptions thrown to Result type of Failure with associated Error
6. Create Errors static class to house all Error types

### Primitive Obsession and Value Object

Value objects replace primitives as properties on entities.  This allows you to enforce rules that preserve proper state that are not available for the primitive objects.  For example, you may want to ensure that a username is never more than 50 characters.  

Steps
1. Create base ValueObject
2. Replace entity property types with specific value types inheriting from ValueObject
3. Fix references to former properties to create new value objects

### Aggregate Root

An aggregate root is meant for entities that aggregate multiple other entities together.  The purpose is to manage all related entities as one single transaction rather than breaking them out separately.  For example, if a blog post item contains one or more tags, it is better to load and save them all at once rather than independently.   If the tag fails, then the post will be in an invalid state.  If you handle them all as one transaction, then you can manage invalid states.  

Steps
1. Create a new AggregateRoot base class inheriting from Entity simply as an empty class
2. Change reference root class for aggregates
3. Remove ability to retrieve subordinate entities directly (fetch and save as one whole not independently)
!!! This comes before explaining CQRS or setting up the data layer so a little hard to follow

Considerations
- Need functioning data layer

### Domain Events

Domain events allow you to separate the logic of processing a primary action from all the secondary actions that should be triggered after the primary completes.  For example, you may want to send an email notification whenever a new member is added to the system.  The primary action would be to add the member.  The email notification should follow once the member is successfully added.  

Steps
1. Add IDomainEvent
2. Add RaiseDomainEvent method to aggregate base
3. Use RaiseDomainEvent when events happen in your aggregate entity
4. Add MediatR to domain
5. Add INotification to IDomainEvents
6. Add event handler to application

Considerations:  
- Need to separate services in handlers (i.e. email service depends on repository to pull posts - what if the database is down)
- Need outbox pattern

### Data Layer Bonus Round

Added a basic data layer using entity framework and an in memory database.  

Steps
1. Add any missing IRepositories for entities in the domain layer
2. Add anemic models to data for each entity
3. Add mapping from entity to data and from data to entity
4. Add repositories for each entity (Question:  Only added aggregates.  Should it include all entities?)
5. Add Data Exceptions

### Outbox Pattern

The outbox pattern is used to make sure events are processed and not lost.  The pattern takes in events and holds them until they can be successfully processed.

When an action completes on an aggregate, it should raise an event.  An background job will periodically scan for events, pull them from the aggregate, store them to the datastore, handle them, and marked them complete.  

Steps
1. Add an OutboxMessage to persistence to store events to the database (Add model, include in DbSet)
2. Add methods to Get and Clear the events from the aggregate root
3. Create an interceptor to get the events from the aggregate, clear the events from the aggregate, and store them to the database.
4. Wire up the interceptor in the dbContext initialization
5. Add Quartz package for background jobs (job scheduler) to Infrastructure layer
6. Add Outbox Job processing
7. Need to wrap in try/catch for when job failures occur
8. Add Quartz.Extensions.DependencyInjection to app
9. Add Quartz Startup

TODO:  
- Handle null event in job
- Add try/catch for job failures

ISSUE
- The outbox pattern implemented on the entity with separate data model does not work when using the interception method -> You intercept the model, not the entity
- Complex interaction here.  Need to remember where to raise the event and ensure single point of entry

Strategies
- RaiseEvents on Model, intercept in EntityFramework, and save/clear messages
- Raise events on Entity, use EventRepository


### BONUS:  Added CQRS for members to allow for Email uniqueness test


### Email uniqueness test


Strategies
1. Check for uniqueness in the create handler
	a. Pros:  Simple solution where repository and entity are available
	b. Cons:  Need to remember to do the check where ever you access.
2. Check for uniqueness in the entity
	a. Pros:  Single entry point for creation will always check without needing to remember
	b. Cons:  Requires passing the repository to the entity (not good form)
3. Check for uniqueness in the hanlder and pass result to the entity
	a. Pros:  Keeps it simple without mixing repo/entity
	b. Cons:  Need to remember to do the uniqueness check and pass result to entity, easily bypassed and forgotten
4. Check in Repo
	a. Pros:  Avoids passing dependencies, cleaner
	b. Cons:  Need to remember to check update and other methods, requires try/catch or handling above

Steps

### BONUS: API

Notes
- Using base ApiController with ISender in constructor does not work with EndPointDefinition scanning.  Remove and inject in method.


### CQRS

Command Query Responsibility Segregation separates the read only queries from the write commands.  It allows you to keep the logic separate.  It allows you to separate the backend database to potentially have a different database for online rights and offline reads for better performance. 

Notes
- Uses Assembly reference static classes
- Separates ApiController from Api project
- Uses Screwter library to scann infrastructure and persisitence to register services
- Base API controller (ISender to send commands to mediatr (can also use publisher))
- Nothing was wired up so it is complicated to get started.  Need to have a session just on wiring up without requiring a bunch of outside packages.

Query Handler strategies
- pass context to handler and query
- query directly with dapper (used in earlier lessons)
- use the repository (preferred)

Steps
1. Add ICommand inheriting from IRequest to abstract MediatR
2. Add ICommand<Result> inheriting from IRequest<Result> to enforce returning a result type
3. Add ICommandHandler to abstract MediatR
4. Add CreateMemberCommand
5. Add CreateMemberRequest
6. Add CreateMemberCommandHandler
7. Add Assembly reference static class to all projects 
8. Add IQuery
9. Add IQueryHandler
10. Add GetMemberByIdQuery
11. Add GetMemberByIdQueryHandler
12. Add MemberResponse

### Validation

Steps
1. Create an interface to hold bad validation results IValidationResult
2. Add concrete ValidationResult and ValidationResult<T>
3. Create Validator class for each command/query

Notes
- By setting public const in entities, you can share the values need for rules to maintain consistency for example with maximum length
- Somewhere along the way we created generic Result types, but never added them to our command/queries
- watch for lazy loading not loading on each query - better way?

Strategies
- throw an exception and handle in exception handling middleware
- use MediatR pipeline* to return results

### Bonus Adding missing repositories and validations

Notes
- ValueObjects are too much.  To create, you get a result, you must check if the result was successful, then you must check if the value is null to satisfy the compiler.  Why not just return the value and check for null as success?

### Retry Outbox with Polly

The outbox pattern can fail to process for a variety of reasons.  For example, an email notifier could fail if the email service is temporarily down.  What happens if this condition occurs?  

strategies
1. try/catch approach
2. Polly Retry

Steps
1. Add Polly
2. Create a retry policy in a background job method
3. Execute the target method inside the policy
4. Capture the results
5. Handle errored results

### Duplicate Message | Idempotent Handler

Avoid duplicating events if retries are used

Steps
1. Create EventConsumer data class to track each consumer of the event
2. Add missing IDomainEventHandler
3. Add missing DomainEvent from IDomainEvent and change events to reference DomainEvents
4. Create IdempotentDomainEventHandler
5. Wire up the decorator (using Scrutor package)


Notes: 
- Introduces non-existent IDomainEventHandler
- Introduces non-existent DomainEvent
- EventHandlers need to reference IDomainEventHandler and IDomainEventHandler needs to reference INotificationHandler - why the complexity?
- 

### Unit Test Command Handlers

Steps
1. Make Application library internal visible to test
2. Create UnitTest project in test folder
3. reference Moq to Mock the repos
4. Use Mock.Setup() to set parameters and return type of mocked methods/properties
5. Write test against handlers using mocked repos

Notes
- naming convention class named for class tested
- method named for method tested, Should, expected outcome, and condition

### Unit Of Work

Using the unit of work pattern also abstracting entity framework (or your data layer) from the application/presentation to avoid shared dependencies.  It also for easier mocking during unit testing.  It also offloads the save responsibility from the repositories allowing you to more easily create transactions across multiple repos.  This method avoids using and wiring interceptors.  It also allows you to build in record log and management.
Similarly, abstracting the repos allows you to more easily mock in testing and shield the application layer from the data.


Steps
1. start with base UnitOfWork
2. Move Interceptor methods into UoW.

Notes
- starts with a very basic unit of work
- moves interceptors into unit of work
- References an Audit lesson that is not in the playlist - Audit interceptor for adding create and modify date tracking.

### Smart Enum (strongly typed enum)

Basic enumerations work well, but sometimes you need more functionality.  Building a SmartEnum allows you to mimic the functionality of enum and extend it to include additional functionality in a strongly typed class.

Steps
1. Create an Enumeration class to inherit from 
2. Switch ENUM concrete type to class inheriting from Enumeration
3. 


Notes
- did not relate to the solution

SmartEnum Base
```
public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
{
	public int Value { get; protected init; }
	public string Name { get; protected init; } = string.Empty;

	public bool Equals(Enumeration<TEnum>? other)
	{
		if(other is null)
			return false;

			return GetType() == other.GetType() &&
				Value == other.Value;
	}

	public override bool Equalt
}
```

SmartEnum implementation
```
public abstract class CreditCard : Enumeration<CreditCard>
{
	public static readonly CreditCard Standard = new StandardCreditCard();
	public static readonly CreditCard Premium = new PremiumCrediCard();
	public static readonly CreditCard Platinum = new PlatinumCreditCard();

	private CreditCard(int value, string name)
		: base(value, name)
	{ }

	public abstract double Discount { get; }

	private sealed class StandardCreditCard : CreditCard
	{
		public StandardCreditCard()
			: base(1, "Standard") { }

		public override double Discount => 0.0;
	}
	
	private sealed class PremiumCreditCard : CreditCard
	{
		public StandardCreditCard()
			: base(1, "Premium") { }

		public override double Discount => 0.05;
	}
	
	private sealed class PlatinumCreditCard : CreditCard
	{
		public StandardCreditCard()
			: base(1, "Standard") { }

		public override double Discount => 0.1;
	}
}
```
### Bonus Error Handling

Strategies
- Add middleware logic by adding a app.Use(() => {}) to program.cs using a try/catch before moving next
- Create a simple class for middleware
- Create a strongly typed middleware implementing IMiddleware

Steps
1. Create a middleware class
2. Add the class as a transient service for initialization
3. UseMiddleware in program.cs

```
app.Use(async (context, next) => 
{
	try
	{
		await next(context);
	}
	catch(Exception e)
	{
		context.Response.StatusCode = 500;
		// TODO: Add more meaningful information and logging
	}
})
```

### Bonus Decorator for Caching

Works between the API and the database

Steps
1. Add Cache Repository
2. Add IMemoryCache to Repository
3. Make key
4. Wire in program.cs - add the concrete repo, add cache implementing interface

Strategy for initialization
1. Initialize the concrete DbRepo, reference the DbRepo in the CacheRepo, initiailize the interface referencing the cacheRepo
   Pros:  Simple to wire up with no other dependencies
   Cons:  Can be complicated if you are initializing the repo anywhere else - it will break or not function as expected
2. Initialize the concrete DbRepo, reference the DbRepo in the CacheRepo, completex initialization of the interface
   Pros:  No dependencies
   Cons:  More complexity in the program.cs
3. Use Scrutor Decorator, Cache only references IMemberRepo
   Pros:  Clean and conflicts if referencing the interface
   Cons:  Brings in the SCutor dependency

Option 1
```
builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<IMemberRepository, MemberCacheRepository>();
```

Option 2
```
builder.Services.AddScoped<IMemberRepository>(provider => 
{
	var memberRepo = provider.GetService<MemberRepository>()!;

	return new MemberCacheRepository(
		memberRepository,
		provider.GetService<IMemoryCache>()!);
	)
})
```

Options 3
```
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.Decorate<IMemberRepository, MemberCacheRepository>();
```

Notes
- 

### Railway - Functional programming

Railway acts as a switch with two possibilities - failure or success. Extensions are used against the Result type to apply validations.  It can result in less code that reads more naturally.
It is implemented by chaining methods together to result in a Result type that can be returned.  
This approach abstracts the common patter of validation checks that look for conditions to decide if a result will be successful or failure.
Alternatively, it can be hard to debug and is not a common concept that could confuse others who are not familiar with the pattern.

Steps
1. Create an extension class for Result
2. Update conditions in classes that produce Result

Notes
- implemented on Email value object as a test

### Token Authentication



Steps
1. Add Jwt nuget
2. Add IJwtProvider to abstract the provider implementation
3. Add a LoginCommand
4. Add a LoginRequest
5. Add a LoginCommandHandler
6. Add a login endpoint to the member controller
7. Wire authentication/authorization to program.cs
8. create JwtOptions and wire
9. Create JwtBearerOptions and wire
10. Add options to config
++. Add to Swagger

## Credit
Milan Jovanovic
Clean Architecture & DDD Series
https://www.youtube.com/playlist?list=PLYpjLpq5ZDGstQ5afRz-34o_0dexr1RGa