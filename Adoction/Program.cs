var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Adoction.Application.Options.JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddSingleton<Adoction.Domains.Interfaces.IPetRepository, Adoction.Infrastructure.Repos.InMemoryPetRepository>();
builder.Services.AddSingleton<Adoction.Domains.Interfaces.IUserRepository, Adoction.Infrastructure.Repos.InMemoryUserRepository>();
builder.Services.AddSingleton<Adoction.Application.Services.IPetService, Adoction.Application.Services.PetService>();
builder.Services.AddSingleton<Adoction.Application.Services.IAuthService, Adoction.Application.Services.AuthService>();
builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, Adoction.Application.Auth.PermissionAuthorizationHandler>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
        ValidAudience = jwtSettings.GetValue<string>("Audience"),
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("SecretKey")!))
    };
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration.GetValue<string>("Google:ClientId")!;
    options.ClientSecret = builder.Configuration.GetValue<string>("Google:ClientSecret")!;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Adoction.Application.Auth.PermissionConstants.PetsRead, policy =>
        policy.Requirements.Add(new Adoction.Application.Auth.PermissionRequirement(Adoction.Application.Auth.PermissionConstants.PetsRead)));
    options.AddPolicy(Adoction.Application.Auth.PermissionConstants.PetsWrite, policy =>
        policy.Requirements.Add(new Adoction.Application.Auth.PermissionRequirement(Adoction.Application.Auth.PermissionConstants.PetsWrite)));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
