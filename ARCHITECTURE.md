# ASI Application Architecture

- MyApp.Web.Api - Web API 
- MyApp.Core - Houses core functionality like services, configuration, entity models, mappers
- MyApp.Data - Houses data specifically required to connect the entities defined in Core into and out of a database via the DataAccess implementation
- ASI.Contracts.MyApp - Houses contracts for use between different applications that rely on MyApp views. Should be built as a nuget package.
- MyApp.Algolia - Contains all logic for interacting with Algolia. Implements Core/Search abstractions such as IMyEntitySearchProvider
- Test
  - MyApp.Core.Test - Sample unit test project for the Core project. Any additional projects added should have their own .Test projects defined.
  - MyApp.Web.Api.Test - Integration test project for the Web API. Includes working sample POST and PUT tests using an in-memory EF database to show the stack functioning properly.


## Request Processing Workflow

1. Request is made by client to the web api, `GET myapp.asicentral.com/myentities/1234`

2. ASP.NET Core libraries route the request to the respective `MyEntitiesController`
3. Authorization is performed by ASI Authorization middleware if the controller is configured to be set to require authorization. If it fails, processing stops and 401 is returned
4. The controller method assigned to `GET /1234` is invoked. The `{id}` route defines that the value `1234` be interpreted into the parameter `id` in the method arguments. 
5. The controller method transforms the data from a `View` object to a business object
6. It then sends that business object to the appropriate `IService` class, which processes the request, returning another business object.
7. That business object is then transformed back into a `View` object and returns it as the HTTP response.


## Dependency Injection Configuration

Each project will have it's own `Startup/DependencyInjection.cs` class defined that is responsible for exposing methods to register all injectable objects into an IServiceCollection when called. Any running application will then call all these registration methods that are needed for its use case. In `Web.Api.Startup/DependencyInjection.cs`, you will see the methods `services.AddCore()` called. This calls to the Core project's DI.cs method, that registers all the necessary objects to utilize services defined in that core project. It then calls any other similar methods that are required to run the web api.

Other applications can be run from the same codebase. For instance, a Barista plugin may be responsible for loading data into a database, but has no need for searching. In that case, the startup of that plugin might only call `AddData()` but not `AddAlgolia()`. This way, each application can define what services it relies upon, or even override them when needed, but still rely on the code present in this codebase.

The Core project should function as the primary hub for all business logic. Any reliance on other services should be abstracted through an interface defined in Core, and implemented in concrete project. For instance, a service might need to read from the database, so we have the `IDataAccess` interface defined in Core, but the logic for implementing that interface is contained within MyApp.Data. Then, if we decide to change out our database provider from say, SQL Server to MongoDB, all we need to do is re-implement the `IDataAccess` interface in a new project and the rest of the code will continue to function.


## Unit tests with Operations

The core Service methods such as `MyEntityService` often are the primary interface for all things related to the MyEntity objects. Over time, this can become full of many dependencies, making it very cumbersome to write unit tests for it. Every time you'd add a new dependency, every unit test would need to be updated.

To help solve this, we move complex functions into `Operation` classes. Those operations
have defined dependencies captured within a `Context` object. These operations then have the disparate parts of their logic split between methods that can then be tested either in tandem or individually.

## DataTransaction and SearchTransaction constructs

When making changes to a database, in order to write a unit test to ensure that the change is made, we have the construct of an `IDataTransaction`. This object keeps a record of all the changes that will be made to the database when `Commit()` is called. This way, changes can be batched together, and we can inspect the Commands stored on the transaction during unit tests to ensure that the correct actions are being taken.

This abstraction also allows the core code to be agnostic of how that data is actually being stored. Again this makes it easier to change our storage solution should the need arise.

The same is possible with searching via the `ISearchTransaction` construct. Not every change to a database will require a search index change, so it's up to the operation being performed to code for these situations. This structure makes it very easy to change from one search index provider to another. And, as abstract as it is, you could even define a transaction type that commits to BOTH at the same time. This could allow us to transition easily from one provider to another without affecting the existing logic for the existing provider.
