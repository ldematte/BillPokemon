# BillPokemon: Pokemons in the vitorian era

The project provides a REST API implemented using ASP.NET Core.
It has Swagger for automatic documentation and ease client implementation.
It runs under any environment supported by .NET Core (Windows, Linux, Mac)

The API combines the results of two online services: Pokeapi and Funtranlsations.

Since this project is inteded as a tech demo, it implements the following features:

- Async everywhere
- Safe and flexible use of external services 
    - Circuit breakers
    - Configurable timeouts and retries
    - Mockable


## Design, project structure, implementation choices


### BillPokemon.Core
I have hidden external services behind interfaces, to make the solution more modular and testable.
This project includes the interfaces as well as common utilities that can be shared across services implementations.
Why just one project? For simplicity. In a more complex scenario, this project could be divided by service type (one project for each service, e.g. BillPokemon.Translation.Core) or by functionality (e.g. BillPokemon.Interfaces + BillPokemon.Common), or both.

### BillPokemon.Tests
Project for tests. This is a compromise/semplification: I could have done multiple test project (e.g. BillPokemon.Core.Tests), but in this case it would have been overengineering. I could always refactor and add other projects later if this becomes too big.

The test bank includes unit tests and integration tests. Integration tests will actually ping the external server APIs for data while the unit tests run off of mocked data.

*** Test are not meant to be complete! ***
- They do not provide complete coverage
- They do not cover every possible scenario
- Different tests with different techniques overlap
 
 This is intentional, and it is done in order to:
 - keep having fun: fun is in the scope of this project, being bored writing repetitive unit tests it is not :)
 - demonstrate different techniques: integration with real services, mocking of the real services, mocking of the services responses.


### BillPokemon.PokeApiNet

For the sake of the example (to show how things could be done in different ways), I decided to implement the description services using an existing library. To read Pokemon descriptions, I used the PokeApiNet library.
This is a good candidate for the following reasons:
- it is async (re: the non-functional requirement "async everywhere")
- it uses HttpClient (so we can control behaviour like retries and circuit breakers centrally through IHttpClientFactory)

### BillPokemon.FunTranslations

Again as an example, I implemented a very basic, simple, minimal client for the translation service using HttpClient and Newtonsoft.Json, based on the docs found on the funtranslations site.

## Running

### Without docker

- Download and install the .NET SDK for your platform at https://dotnet.microsoft.com/download
- clone the repo
- cd into the project directory (cd BillPokemon)
- at the prompt, write "dotnet run" and press enter

### With docker

TODO

You can now open the browser to https://localhost:5001/swagger/index.html and use Swagger to read the API docs and/or try it out.