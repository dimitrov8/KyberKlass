using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace KyberKlass.Web.Infrastructure.Extensions;
public class EndpointConfigurationExtension
{
    public static void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
            "adminSchool",
            "Admin/School/{action=Index}",
            new { controller = "School" }
        );

        endpoints.MapControllerRoute(
            "adminClassroom",
            "Admin/Classroom/{action=Index}",
            new { controller = "Classroom" }
        );

        endpoints.MapControllerRoute(
            "adminUser",
            "Admin/User/{action=Index}",
            new { controller = "User" }
        );

        endpoints.MapControllerRoute(
            "adminTeacher",
            "Admin/Teacher/{action=Index}",
            new { controller = "Teacher" }
        );

        endpoints.MapControllerRoute(
            "adminStudent",
            "Admin/Student/{action=Index}",
            new { controller = "Student" }
        );

        endpoints.MapControllerRoute(
            "default",
            "{controller=Home}/{action=Index}/{id?}");
    }

    public static void ConfigureRazorPages(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRazorPages();
    }
}