# SampleAspNetApplication

[README](../README.md)

## Problem details page

Problem details are way to better handle exceptions in ASP.NET Web App.
In order to add it it's enough to `builder.Services.AddProblemDetails();`
and `app.UseStatusCodePages();` extension methods.

## Integration tests with `Microsoft.AspNetCore.Mvc.Testing`

In `Microsoft.AspNetCore.Mvc.Testing` namespace, there's `WebApplicationFactory` which allows for 
easy writing of integration tests.  
Documentation: [Integration tests in ASP.NET Core](learn.microsoft.com/en-us/aspnet/core/test/integration-tests)