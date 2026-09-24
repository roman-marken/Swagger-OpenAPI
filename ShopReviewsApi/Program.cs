using Asp.Versioning;
using Scalar.AspNetCore;
using ShopReviewsApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IReviewRepository, InMemoryReviewRepository>();

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "Shop Reviews API";
        document.Info.Version = "v1";
        document.Info.Description =
            "Публічне API відгуків до предметів каталогу: список, створення, часткове редагування та добірка хороших відгуків.";
        return Task.CompletedTask;
    });
});

builder.Services.AddOpenApi("v2", options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "Shop Reviews API";
        document.Info.Version = "v2";
        document.Info.Description =
            "Друга версія API: агрегована статистика (середня оцінка та кількість відгуків) по предмету каталогу. Список відгуків v1 не змінюється.";
        return Task.CompletedTask;
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
