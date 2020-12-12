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

## Running

### Without docker

- Download and install the .NET SDK for your platform at https://dotnet.microsoft.com/download
- clone the repo
- cd into the project directory (cd BillPokemon)
- at the prompt, write "dotnet run" and press enter

### With docker

TODO

You can now open the browser to https://localhost:5001/swagger/index.html and use Swagger to read the API docs and/or try it out.