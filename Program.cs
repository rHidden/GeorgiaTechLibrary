using DbContextNamespace;
using GeorgiaTechLibrary.DbContext;
using GeorgiaTechLibrary.Repositories;
using GeorgiaTechLibrary.Repositories.RepositoryInterfaces;
using GeorgiaTechLibrary.Services;
using GeorgiaTechLibrary.Services.ServiceInterfaces;

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

builder.Services.AddDbContext<GTLDbContext>();
// Dependency Injection
//services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMemberService, MemberService>();
//repos
builder.Services.AddTransient<IBookRepository, BookRepository>();
builder.Services.AddTransient<ILibraryRepository, LibraryRepository>();
builder.Services.AddTransient<ILoanRepository, LoanRepository>();
builder.Services.AddTransient<IMemberRepository, MemberRepository>();
builder.Services.AddTransient<IStaffRepository, StaffRepository>();

builder.Services.AddScoped<IDatabaseConnectionFactory, DatabaseConnectionFactory>(_ => new DatabaseConnectionFactory(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
