namespace IdentityService.Config.DbSettings;
public interface IIdentityDatabaseSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}