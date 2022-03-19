# cacheManager


## Description

multi layer cache manager example written in c# and .net core 3.

request ----------> first layer cache (memory) ----------> second layer cache (redis) ----------> Database
<br/>
request <---------- first layer cache (memory) <---------- second layer cache (redis) <---------- Database

## Installation

```bash
$ dotnet restore
```

## Running the app

```bash
$ dotnet run
```

## Test

```bash
# unit tests
$ dotnet test
```
