using CleanArchitecture.Api;
using CleanArchitecture.CrossCutting;
using CleanArchitecture.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Host.Serilog();

builder.Services.AddContextInMemoryDatabase();
builder.Services.AddJsonStringLocalizer();
builder.Services.AddClassesMatchingInterfaces();
builder.Services.AddMediator();
builder.Services.AddResponseCompression();
builder.Services.AddControllers().AddJsonOptions();
builder.Services.AddSwaggerGen();

var application = builder.Build();

application.UseException();
application.UseLocalization();
application.UseSwagger();
application.UseSwaggerUI();
application.UseHttpsRedirection();
application.UseResponseCompression();
application.MapControllers();

application.Run();
