using Microsoft.Extensions.Options;
using Nest;

namespace Product.Persistance.Elasticsearch.Config;
public class ElasticContextProvider : IElasticContextProvider
{
    private IElasticClient Client { get; set; }
    private IOptions<ElasticSearchSetting> _options;
    public ElasticContextProvider(IOptions<ElasticSearchSetting> options) 
        => _options = options ?? throw new ArgumentNullException(nameof(options));

    public IElasticClient GetClient()  
        => Client == null
            ? new ElasticClient(new ConnectionSettings(new Uri(_options.Value.Host))
                           .BasicAuthentication(_options.Value.UserName, _options.Value.Password)
                           .DisablePing()
                           .DisableDirectStreaming(true)
                           .SniffOnStartup(false)
                           .SniffOnConnectionFault(false))
            : Client;

}