using Coravel;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StocksApi.Data;
using StocksApi.Models;
using StocksApi.Repositories.Implementations;
using StocksApi.Repositories.Interfaces;
using StocksApi.Services.Implementations;
using StocksApi.Services.Interfaces;
using StocksApi.Workers;
using StocksApi.Workers.Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add User Secrets to Configuration
//builder.Configuration.AddUserSecrets<Program>();


// -------------------------------
// Configure Swagger/OpenAPI
// -------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Add Hangfire Services
builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
          {
              CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
              SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
              QueuePollInterval = TimeSpan.Zero,
              UseRecommendedIsolationLevel = true,
              DisableGlobalLocks = true
          });
});

// Add Hangfire Server
builder.Services.AddHangfireServer();

// Add Coravel Services
builder.Services.AddScheduler();
builder.Services.AddQueue();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// ----------------------------
// Configure Database Context
// ----------------------------
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ----------------------------
// Configure Identity
// ----------------------------
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 10;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
}).AddEntityFrameworkStores<ApplicationDBContext>();

// ----------------------------
// Configure Authentication
// ----------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultScheme =
    options.DefaultForbidScheme =
    options.DefaultChallengeScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        //ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


// ------------------------------
// Register Application Services
// ------------------------------
builder.Services.AddSingleton<CommentWorkerHangfire>();
builder.Services.AddSingleton<CommentWorkerCoravel>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();


// ----------------------------------
// Add FluentValidation on pipeline
// ----------------------------------
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters(); 
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard(); // Enable Hangfire Dashboard
}

// Ebable Scheduler
app.Services.UseScheduler(scheduler =>
{
    // Scheduling tasks 
    scheduler.Schedule(() => Console.WriteLine("Scheduled Task Executed"))
             .EveryMinute();
});

// Add Queue
app.Services.ConfigureQueue();

// Enable CORS if allowed origins are configured in appsettings.json
//var allowedOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>();

//if (!allowedOrigins.IsNullOrEmpty() && allowedOrigins.Any())
//{
//    app.UseCors(builder =>
//        builder.WithOrigins(allowedOrigins)
//               .AllowAnyMethod()
//               .AllowAnyHeader());
//}

app.UseHttpsRedirection();

app.UseAuthentication();

app.Use(async (context, next) =>
{
    var user = context.User;
    //For debbuging: place a breakpoint on the line below
    //System.Diagnostics.Debugger.Break();

    //Optionally log the claims
    var claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList();
    if (!claims.IsNullOrEmpty())
    {
        foreach (var claim in claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        }
    }
    await next.Invoke();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
