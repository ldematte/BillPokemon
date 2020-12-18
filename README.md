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

The choice of the two online services is fun, as one (Funtranslations) is much slower and rate limited.

From the requirements:
>
>API Requirements 
> 
>Retrieve Shakespearean description: 
> 
>GET ​endpoint: ​```/pokemon/<pokemon name>``` 
> 
>Usage example (using httpie): 
> 
>```http http://localhost:5000/pokemon/charizard```
> 
>Output format: 
>```{ 
>  "name": "charizard", 
>  "description": "Charizard flies 'round the sky in search of powerful 
>opponents. 't breathes fire of such most wondrous heat yond 't melts aught. 
>However, 't nev'r turns its fiery breath on any opponent weaker than itself." 
>}
>```


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

There was no constructor overload to pass a HttpClient to the library, so I forked the repo and submitted a pull request to the author. The pull request has been merged but not published yet, so for this example I uploaded a forked version of the package on NuGet.org (PokeApi2). Just temporary, will switch to the official package once it is published by the author.

### BillPokemon.FunTranslations

Again as an example, I implemented a very basic, simple, minimal client for the translation service using HttpClient and Newtonsoft.Json, based on the docs found on the funtranslations site.

### Implementation choices

Besides the aforementioned choices on tests (showcases on what would I test and how, not to be intended as a full-coverage test suite), I have *not* changed the requirements, even if I would have like to.

The specified response format does not include any way to specify if an error occoured in one of the services, so I have three choices:
1. modify/change/differentiate the response format. This might be a problem/not possible, so I skipped this option
2. when a service fail, the whole request fail (e.g. if one service gives a 429 or a 400 or a 500, the whole request will return a 500 error)
3. a service failure is embedded in the answer.

I went for the 2nd. This may be not ideal, or desired, depending on the user expectations: is the response useful even if partial? Would be necessary to include more info on which service is at fault? In those cases, I would  advice to go for the 1st, but that would need to be discussed with the API users because it might be a breaking change for them.

For example, an option is to go with a response with no `description` field but some additional fileds for error description:

```
{
  "name": "mew",
  "faulting-service": "Translation",
  "fault": "Response status code does not indicate success: 429 (Too Many Requests)."
}
```
I structured the code in a way that it is super easy to do either one. This could be a good point for discussion.

## Running

### Without docker

- Download and install the .NET SDK for your platform at https://dotnet.microsoft.com/download
- clone the repo
- cd into the project directory (cd BillPokemon)
- at the prompt, write `dotnet run` and press enter

You can now open the browser to https://localhost:49155/swagger/index.html and use Swagger to read the API docs and/or try it out.

To run tests, cd into the tests directory and run `dotnet test`

### With docker

There is a Docker file in the BillPokemon project directory (BillPokemon\BillPokemon). CD to the project directory and run 

```
docker build -t pokemon -f Dockerfile ..
```

run the docker container, then use ps to see where port 80 and 443 where mapped to:

```
PS BillPokemon\BillPokemon> docker ps
CONTAINER ID   IMAGE             COMMAND               CREATED         STATUS         PORTS
              NAMES
0bc2caa2edfc   billpokemon:dev   "tail -f /dev/null"   3 minutes ago   Up 3 minutes   0.0.0.0:49156->80/tcp, 0.0.0.0:49155->443/tcp   BillPokemon
```

You can now open the browser to https://localhost:49155/swagger/index.html and use Swagger to read the API docs and/or try it out.