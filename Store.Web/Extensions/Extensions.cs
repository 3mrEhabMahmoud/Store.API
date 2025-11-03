using Microsoft.AspNetCore.Mvc;
using Store.Domain.Contracts;
using Store.Shard.ErrorModels;
using Store.Web.Middlewares;
using Store.Persistence;
using Store.Services;
using Store.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;




namespace Store.Web.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection RegsterAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBuiltInServices();

            services.AddSwaggerServices();



            services.AddInfrastructureServices(configuration);
            services.ApplicationServices(configuration);


            services.AddIdentityServices();
           

            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                                 .Select(m => new ValidationError()
                                 {
                                     Field = m.Key,
                                     Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                                 });
                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);

                };


            });

            return services;

        }
        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>(Options =>
            {
                Options.User.RequireUniqueEmail = true;
            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityStoreDbContext>();
            return services;
        }
        private static async Task<WebApplication> SeedData(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDblnitializer>(); //Ask ClR to create object from IDbInitializer
            await dbInitializer.InitializeAsync();
            await dbInitializer.InitialieIdentityAsync();
            return app;
        }




        public static async Task<WebApplication> ConfigureMiddlewares(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDblnitializer>();//Ask CLR to create Object from IDbInitializer
            await dbInitializer.InitializeAsync();

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            SeedData(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            return app;
        }
    }
}
