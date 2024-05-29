using DataAccess.DAO;
using DataAccess.Repositories;
using DataAccess.Repositories.RepositoryInterfaces;
using GeorgiaTechLibrary.Services;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using DataAccess.DAO.DAOIntefaces;
using GeorgiaTechLibrary.Automappers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(host => true)
            .AllowCredentials();
    });
});

// Dependency Injection
//services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookInstanceService, BookInstanceService>();
builder.Services.AddScoped<IDigitalItemService, DigitalItemService>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(typeof(BookInstanceProfile), typeof(DigitalItemProfile), typeof(LoanProfile));
//repos
builder.Services.AddTransient<IBookRepository, BookRepository>();
builder.Services.AddTransient<IBookInstanceRepository, BookInstanceRepository>();
builder.Services.AddTransient<IDigitalItemRepository, DigitalItemRepository>();
builder.Services.AddTransient<ILibraryRepository, LibraryRepository>();
builder.Services.AddTransient<ILoanRepository, LoanRepository>();
builder.Services.AddTransient<IMemberRepository, MemberRepository>();
builder.Services.AddTransient<IStaffRepository, StaffRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddScoped<IDatabaseConnectionFactory, DatabaseConnectionFactory>(_ => new DatabaseConnectionFactory(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GeorgiaTechLibrary", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
