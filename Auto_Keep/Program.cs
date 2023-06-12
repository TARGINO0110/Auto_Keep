using Auto_Keep.Models.DbContextAutoKeep;
using Auto_Keep.Services.ServiceEstoqueMonetario;
using Auto_Keep.Services.ServiceEstoqueMonetario.Interfaces;
using Auto_Keep.Services.ServiceHistoricoVeiculos;
using Auto_Keep.Services.ServiceHistoricoVeiculos.Interfaces;
using Auto_Keep.Services.ServicePrecos;
using Auto_Keep.Services.ServicePrecos.Interfaces;
using Auto_Keep.Services.ServiceTiposVeiculos;
using Auto_Keep.Services.ServiceTiposVeiculos.Interfaces;
using Auto_Keep.Utils.Validations;
using Auto_Keep.Utils.Validations.Interfaces;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.IO.Compression;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//******** CONFIGURAÇÃO DO ENTITY-FRAMEWORK *******************

builder.Services.AddDbContext<AutoKeepContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ******************* SWAGGER CONFIG DOCS *******************

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API AUTO KEEP",
        Version = "v1",
        Description = "Projeto destinado ao gerenciamento de um estacionamento com pagamento em dinheiro automático",
        Contact = new OpenApiContact
        {
            Name = "Luciano Targino",
            Email = "lucianoptargino1@outlook.com.br",
        },
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

    c.EnableAnnotations();// ANOTAÇÃO HEADER

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// *********** NEWTONSOFT JSON ***********

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
builder.Services.AddControllers().AddNewtonsoftJson(options =>
               options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
           );

builder.Services.AddControllersWithViews().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

/***** COMPRESS REQUEST DATA *****/

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

// ********  UTILIZAÇÃO DE [TRANSIENT] PARA INTERFACE E [SINGLETON] PARA CONEXÃO AO BANCO  *******************

builder.Services.AddTransient<IEstoqueMonetarioRepository, EstoqueMonetarioRepository>();
builder.Services.AddTransient<IHistoricoVeiculosRepository, HistoricoVeiculosRepository>();
builder.Services.AddTransient<IPrecosRepository, PrecosRepository>();
builder.Services.AddTransient<ITiposVeiculosRepository, TiposVeiculosRepository>();
builder.Services.AddTransient<IValidationAtributes, ValidationAtributes>();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
