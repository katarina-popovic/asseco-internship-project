using System.Reflection;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.ServiceContracts;
using Asseco.REST.Utilities.Serialization;
using BankProject.Implementations.PersonalFinanceManagementApiTransactionQueryServiceMock;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using TransactionAPI.Database;
using TransactionAPI.Database.Repositories;
using TransactionAPI.Implementations;

namespace BankProject;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddMvcWithDynamicCaseResolvers();
        builder.Services.AddScoped<IPersonalFinanceManagementAPITransactionsQueryService, PersonalFinanceManagementAPITransactionsQueryServiceEF>();
        builder.Services.AddScoped<ITransactionsRepository, TransactionsRepository>();
        builder.Services.AddScoped<IPersonalFinanceManagementAPITransactionsCommandService, PersonalFinanceManagementAPITransactionsCommandServiceEF>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IPersonalFinanceManagementAPICategoriesCommandService, PersonalFinanceManagementAPICategoriesCommandService>();
        builder.Services.AddScoped<ICategorizedTransactionRepository, CategorizedTransactionRepository>();
        builder.Services.AddScoped<IPersonalFinanceManagementAPICategoriesQueryService, PersonalFinanceManagementAPICategoriesQueryServiceEF>();
        builder.Services.AddScoped<IPersonalFinanceManagementAPIAnalyticsQueryService, PersonalFinanceManagementAPIAnalyticsQueryServiceEF>();
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        builder.Services.AddDbContext<PfmDbContext>(options => {
            options.UseNpgsql(CreateConnectionString(builder.Configuration));
        });

        /*builder.Services.AddDbContext<CategoryDbContext>(options => {
            options.UseNpgsql(CreateConnectionString(builder.Configuration));
        });*/

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseAuthorization();

        InitializeDatabase(app);

        app.MapControllers();
        
        app.Run();
    }

    private static void InitializeDatabase(WebApplication app)
    {
        if(app.Environment.IsDevelopment()){
            using var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope();

            //scope.ServiceProvider.GetRequiredService<TransactionsDbContext>().Database.Migrate();
        }
    }

    private static string CreateConnectionString(IConfiguration configuration)
    {
        var username = Environment.GetEnvironmentVariable("DATABASE_USERNAME") ?? configuration["Database:Username"];
        var password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? configuration["Database:Password"];
        var database = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? configuration["Database:Name"];
        var host = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? configuration["Database:Host"];
        var port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? configuration["Database:Port"];
        
        var builder = new NpgsqlConnectionStringBuilder(){
            Host = host,
            Port = int.Parse(port),
            Username = username,
            Password = password,
            Database = database,
            Pooling = true
        };
        return builder.ConnectionString;
    }
}
