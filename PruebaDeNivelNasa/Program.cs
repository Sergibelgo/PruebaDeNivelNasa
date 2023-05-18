using PruebaDeNivelNasa.Services.Classes;
using PruebaDeNivelNasa.Services.Interfaces;

namespace PruebaDeNivelNasa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<INasaService, NasaService>();
            builder.Services.AddTransient<IJSONService, JSONService>();
            builder.Services.AddTransient<IDateService, DateService>();
            builder.Services.AddHttpClient();
            builder.Services.AddAutoMapper(typeof(Program));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}