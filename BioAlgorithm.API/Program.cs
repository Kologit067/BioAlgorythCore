using BioAlgorithm.Data.Contract.Representatives.Data.Contract.Interfaces;
using BioAlgorithm.Data.Representatives.Data;
using BioAlgorithm.Services.Contract;
using RepresentativeServices;

var builder = WebApplication.CreateBuilder(args);
if ( builder == null)
{
    Console.WriteLine("can not create builder");
    return;
}

// Add services to the container.
builder.Services.AddOutputCache(o =>
{
    o.MaximumBodySize = 4 * 1024 * 1024;    // 4 
    o.SizeLimit = 64 * 1024 * 1024;    // 64 
    o.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(12);  // 
});
builder.Services.AddMemoryCache();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IRepresentativesRepository>(sp => new RepresentativesRepository(connection));
builder.Services.AddScoped<IRepresentativeService, RepresentativeService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddCors(options => options.AddPolicy("AllowAngular", builder => builder
//                    .WithOrigins("https://localhost:4200")
//                    .AllowAnyHeader()
//                    .AllowAnyMethod())
//               );
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseOutputCache();
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); 

app.MapControllers();

app.Run();
