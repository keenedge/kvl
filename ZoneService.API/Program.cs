using Zone.Controllers;
using Zone.Services;
using Zone.Models;
using alpha;
using Alpha.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddControllersAsServices();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(
    options =>
    {
        options.AddPolicy("AllowAllOrigins",
             builder =>
             {
                 builder.AllowAnyOrigin();
             });
    }
);

builder.Services.AddSingleton<IConfigRepoService, ConfigRepoService>();
builder.Services.AddSingleton<IThinkLogicalManagerService, ThinkLogicalManagerService>();
builder.Services.AddSingleton<IZoneManagerService, ZoneManagerService>();
builder.Services.AddSingleton<IWindowCommandGeneratorService, WindowCommandGeneratorService>();                        
builder.Services.AddSingleton<IWindowCommandExecutorService, WindowCommandExecutorService>();                        
builder.Services.AddSingleton<IWindowManagerService, WindowManagerService>();                        



builder.Services.Configure<KVLConfiguration>(options => builder.Configuration.GetSection("KVLConfiguration").Bind(options));
builder.Services.Configure<alpha.Models.AlphaServiceConfiguration>(options => builder.Configuration.GetSection("AlphaServiceConfiguration").Bind(options));


builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));
builder.Host.UseWindowsService();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging();
//}

app.UseHttpsRedirection();
app.UseDeveloperExceptionPage();
app.Run();
