using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WeatherAPI.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<ExternalServicesOptions>(builder.Configuration.GetSection("ExternalServices"));



// Get JWT settings from appsettings.json (e.g., Issuer, Audience, Secret)
var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);

// Bind those settings to JwtOptions class and validate them
builder.Services.AddOptions<JwtOptions>()
    .Bind(jwtSection) // Map config values → JwtOptions
    .ValidateDataAnnotations() // Check required fields (like [Required])
    .ValidateOnStart(); // Validate immediately when app starts

// Read the JWT values into an object
// If missing throw error so app doesn't run with bad config
var jwtOptions = jwtSection.Get<JwtOptions>()
    ?? throw new InvalidOperationException("JWT configuration is missing.");

// Create a security key using the secret (used to sign and verify JWT tokens)
var signingKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(jwtOptions.Secret)
);



builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin() // Use AllowAnyOrigin() with caution
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});


// Configure Authentication using JWT
builder.Services.AddAuthentication(options =>
{
    // Set JWT as default authentication method
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Define how incoming JWT tokens should be validated
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Check who created the token
        ValidateAudience = true, // Check who the token is for
        ValidateLifetime = true, // Check if token is expired
        ValidateIssuerSigningKey = true, // Verify token signature

        ValidIssuer = jwtOptions.Issuer, // Must match Issuer in token
        ValidAudience = jwtOptions.Audience, // Must match Audience in token
        IssuerSigningKey = signingKey, // Key used to verify token

        ClockSkew = TimeSpan.Zero  // No extra time after token expiry
    };
});
builder.Services.AddAuthorization();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Customer", "Vendor" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.CanConnect();
    if (db.Database.CanConnect())
    {
        Console.WriteLine("Database connected successfully!");
    }
    else
    {
        Console.WriteLine(" Database connection failed!");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();