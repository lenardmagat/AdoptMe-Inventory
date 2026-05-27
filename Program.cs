using inventory.Middleware;
using inventory.DataBase;
using Serilog;
using Microsoft.EntityFrameworkCore;
var app = Configuration.webApplication();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    int retry =0;
    var services = scope.ServiceProvider;
    while(true)
    {
        try
        {
            var context = services.GetRequiredService<DbManager>();
            context.Database.Migrate(); 
            break;
        }
        catch (Exception ex)
        {
            
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogWarning($"Database connection failed. Database might still be initializing. Retrying in 5 seconds...");
        
        if (retry == 4) // Final attempt failed
        {
            logger.LogCritical(ex, "Could not connect to database after multiple attempts.");
            throw; 
        }

        // 📢 CRITICAL: Give PostgreSQL 5 seconds to finish initializing!
        
        }
        Thread.Sleep(5000);
        retry ++;
    }
}
app.Run();
