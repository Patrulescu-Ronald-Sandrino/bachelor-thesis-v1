namespace bll;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public JwtConfig JwtConfig { get; set; }
    
    public string FrontendUrl { get; set; }
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
}

public class JwtConfig
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}

