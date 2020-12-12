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

## Tests

There are two different sets of test: unit tests, and integration (end to end) tests.

