using EPAT.Core.Interfaces;
using EPAT.Core.Interfaces.Base;
using EPAT.Core.Services;
using EPAT.Infrasctructure.Repository;
using Microsoft.Extensions.FileProviders;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:51426",
                                              "http://localhost:8080", "http://localhost:8082")
                          .WithMethods("GET", "POST","PUT","DELETE")
                          .AllowAnyHeader();
                          
                      });
});


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//xử lý về DI - Dependency Injection
//inject base
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

//account
builder.Services.AddScoped <IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();




var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();
