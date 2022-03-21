# Credit line application API

A sample API to evaluate credit lines for different founding type customers.

## Table of contents

- [Features](#features)
- [Evaluating](#evaluating)
  - [Requirements](#requirements)
  - [Downloading the source code](#downloading-the-source-code)
  - [Build and run](#build-and-run)
  - [Testing](#testing)
- [Credits](#credits)

## Features

- Allow to process SME and Startup customers credit line applications.
- Maintain a memory based repository for each different customer.
- Configurable API rate limit throttling.

## Evaluating

In order to evaluate this project will need to install required software, download the source code and run some specific commands to test and execute the application.

### Requirements

- [NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
- [Git](https://git-scm.com/downloads).
- [Chrome](https://www.google.com/intl/es-419/chrome/) or any other browser of your choice.
- Command line access.

### Downloading the source code

In your command line preferred tool you need to type the following commands:

```
git clone https://github.com/pablontiv/tribal-credit-line-application
cd tribal-credit-line-application
```

### Build and run

In order to execute the application you will need to type the following command:

```
dotnet run tribal-credit-line-application
```

This will compile the source and start the application. Then you will be able to access to the URL http://localhost:5288/swagger.html and check the exposed endpoint with the required parameters.

### Testing

The project contains a full unit test battery to check each business rule. To run the tests you will need to run the following command:

```
dotnet test -l "console;verbosity=normal"
```

## Credits

- https://stackoverflow.com/questions/45959605/inspect-defaulthttpcontext-body-in-unit-test-situation
- https://stackoverflow.com/questions/32459670/resolving-instances-with-asp-net-core-di-from-within-configureservices
- https://stackoverflow.com/questions/30557521/how-to-access-httpcontext-inside-a-unit-test-in-asp-net-5-mvc-6
- https://www.hanselman.com/blog/minimal-apis-in-net-6-but-where-are-the-unit-tests
- https://dotnetthoughts.net/dotnet-minimal-api-integration-testing/
