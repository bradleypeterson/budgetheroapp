using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;
using Web_API.Contracts.Data;
using Web_API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options 
    => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
);
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(x =>
//{
//    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//    x.IncludeXmlComments(xmlPath);
//});
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IDataStore, DataStore>();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
