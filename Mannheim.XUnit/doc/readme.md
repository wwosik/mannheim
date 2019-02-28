Mannheim.XUnit
===================

Provides common functionality for tests for functionality using dotnet dependency injection.

The core functionality is TestServicesBase which should be subclassed for the test.
As a parameter it takes the xunit's ITestOutputLogger and automatically redirects all
loggin to the xunit output.