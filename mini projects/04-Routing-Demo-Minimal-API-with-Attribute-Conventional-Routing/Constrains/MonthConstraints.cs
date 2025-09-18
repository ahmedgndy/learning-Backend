
public class MonthConstraints : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.TryGetValue(routeKey, out var RouteValue))
            return false;
        if (int.TryParse(RouteValue?.ToString(), out int month))
            return month >= 1 && month <= 12;

        return false; 
    }
}