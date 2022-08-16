using Assignment9.Context;
using Assignment9.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAssignment9Repo, Assignment9Repo>();
builder.Services.AddDbContext<Assignment9Context>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConn")));
builder.Services.AddControllers(setupAction =>
        {
            setupAction.ReturnHttpNotAcceptable = false;
        })
        
        .AddNewtonsoftJson(setupAction =>
        {
            setupAction.SerializerSettings.ContractResolver
            = new CamelCasePropertyNamesContractResolver();
        })
        .AddXmlDataContractSerializerFormatters()

        .ConfigureApiBehaviorOptions(setupAction =>
        {
            setupAction.InvalidModelStateResponseFactory = context =>
            {
                // create a problem details object
                var problemDetailsFactory = context.HttpContext.RequestServices
                    .GetRequiredService<ProblemDetailsFactory>();
                var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(

                    context.HttpContext,
                    context.ModelState);
                // add additionnal info
                problemDetails.Detail = "See the errors field for details.";
                problemDetails.Instance = context.HttpContext.Request.Path;

                // to get the status code to use
                var actionExecutingContext = context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                // validation error
                if ((context.ModelState.ErrorCount > 0) && (actionExecutingContext?.ActionArguments.Count ==
                context.ActionDescriptor.Parameters.Count))
                {
                    problemDetails.Type = "https://courselibrary.com/modelvalidationproblem";
                    problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                    problemDetails.Title = "One or more validation errors occured.";

                    return new UnprocessableEntityObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                }
                // if the Argument was't correctly found
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "One or more erros on input occured.";
                return new BadRequestObjectResult(problemDetails)
                {
                        ContentTypes = { "application/problem+json" }
                };
                
                
            };
        });
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(appBuilder =>
    {
        appBuilder.Run(async context =>
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
        });
    });    
    
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
