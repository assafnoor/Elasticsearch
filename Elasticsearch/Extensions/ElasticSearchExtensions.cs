using Elasticsearch.Models;
using Nest;

namespace Elasticsearch.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(this IServiceCollection services,IConfiguration configuration)
        {
            var Uri = configuration["ELKConfiguration:Uri"];
            var defaultIndex = configuration["ELKConfiguration:index"];
            var settings = new ConnectionSettings(new Uri(Uri)).PrettyJson().DefaultIndex(defaultIndex);
            AddDefaultMappings(settings);
            var cliean= new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(cliean);
            CreateIndex(cliean,defaultIndex);
        }
        private static  void AddDefaultMappings(ConnectionSettings settings)
        {
            settings.DefaultMappingFor<Product>(p => p.Ignore(x => x.Price).Ignore(x => x.Id).Ignore(x => x.Quntity));
        }
        private static void CreateIndex(IElasticClient client,string indexName)
        {
            client.Indices.Create(indexName,i=>i.Map<Product>(x=>x.AutoMap()));
        }
    }
   
}
