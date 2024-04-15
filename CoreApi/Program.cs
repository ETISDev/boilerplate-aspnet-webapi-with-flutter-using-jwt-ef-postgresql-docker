using CoreApi.Data;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<CoreContext>(x =>
    {
        x.UseNpgsql(builder.Configuration.GetConnectionString("DevelopmentConnection"));
        x.UseLazyLoadingProxies();
    });
else
    builder.Services.AddDbContext<CoreContext>(x =>
    {
        x.UseNpgsql(builder.Configuration.GetConnectionString("ProductionConnection"));
        x.UseLazyLoadingProxies();
        // o => o.UseNetTopologySuite());
    });


builder.Services.AddHttpClient<ApiService>();

var builder1 = builder.Services.AddIdentityCore<UserInfo>(
    opt =>
    {
        opt.Password.RequireDigit = true;
        opt.Password.RequireNonAlphanumeric = true;
        opt.Password.RequireUppercase = true;
        opt.Password.RequireLowercase = true;
        opt.Password.RequiredUniqueChars = 1;
        opt.Password.RequiredLength = 8;

        opt.Lockout.AllowedForNewUsers = true;
        opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        opt.Lockout.MaxFailedAccessAttempts = 5;
    }
);

// Role Access Setting
builder1 = new IdentityBuilder(builder1.UserType, typeof(Role), builder.Services);
builder1.AddEntityFrameworkStores<CoreContext>();
builder1.AddRoleValidator<RoleValidator<Role>>();
builder1.AddRoleManager<RoleManager<Role>>();
builder1.AddSignInManager<SignInManager<UserInfo>>();
builder1.AddDefaultTokenProviders();
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(2));

// Token Decoding
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value ?? "")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization(options =>
{
    // options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    // options.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
    // options.AddPolicy("VipOnly", policy => policy.RequireRole("VIP"));
});

builder.Services.AddControllers(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        options.Filters.Add(new AuthorizeFilter(policy));
    })
    .AddNewtonsoftJson(opt => { opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// builder.Services.AddScoped<IVendorAddressRepository, VendorAddressRepository>();
// builder.Services.AddScoped<IVendorBankRepository, VendorBankRepository>();
// builder.Services.AddScoped<IVendorContactRepository, VendorContactRepository>();
// builder.Services.AddScoped<IVendorInfoRepository, VendorInfoRepository>();
builder.Services.AddScoped<IAuthHelperRepository, AuthHelperRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CoreContext>();
        context.Database.Migrate();
        var userManager = services.GetRequiredService<UserManager<UserInfo>>();
        var roleManager = services.GetRequiredService<RoleManager<Role>>();
        DataSeeder.SeedSettings(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during migration!");
    }
}

app.Run();