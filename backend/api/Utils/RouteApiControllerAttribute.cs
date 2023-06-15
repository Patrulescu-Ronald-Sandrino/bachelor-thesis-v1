using Microsoft.AspNetCore.Mvc.Routing;

namespace api.Utils;

// https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-6.0#custom-route-attributes-using-iroutetemplateprovider
[AttributeUsage(AttributeTargets.Class)]
public class RouteApiControllerAttribute : Attribute, IRouteTemplateProvider
{
    public string Template => "api/[controller]";
    public int? Order => null;
    public string Name { get; set; } = string.Empty;
}