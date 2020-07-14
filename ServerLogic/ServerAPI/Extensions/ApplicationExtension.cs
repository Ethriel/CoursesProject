using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace ServerAPI.Extensions
{
    public static class ApplicationExtension
    {
        /// <summary>
        /// An extension method for IApplicationBuilder <paramref name="app"/> that add all necessary Uses
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        public static void AddAppUses(this IApplicationBuilder app,
                                           IConfiguration configuration,
                                           IBackgroundJobClient backgroundJobClient)
        {
            app.UseStaticFiles();

            app.UseCors(configuration["CORS"]);

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHangfireDashboard();
        }
    }
}
