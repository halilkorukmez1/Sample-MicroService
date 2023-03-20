namespace IdentityService.Config.DbSettings;
public class IdentityDatabaseSettings : IIdentityDatabaseSettings
{
    public required string ConnectionString { get; set; }
    public required string DatabaseName { get; set; }
}
