# SampleAspNetApplication

[README](../README.md)

## Problem details page

Problem details are way to better handle exceptions in ASP.NET Web App.
In order to add it it's enough to `builder.Services.AddProblemDetails();`
and `app.UseStatusCodePages();` extension methods.
