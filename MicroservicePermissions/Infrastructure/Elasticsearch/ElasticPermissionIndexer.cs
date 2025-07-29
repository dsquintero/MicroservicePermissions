using Elastic.Clients.Elasticsearch;
using MicroservicePermissions.Application.DTOs;
using MicroservicePermissions.Application.Interfaces;

namespace MicroservicePermissions.Infrastructure.Elasticsearch
{
    public class ElasticPermissionIndexer : IElasticPermissionIndexer
    {
        private readonly ElasticsearchClient _client;
        private readonly string _indexName;

        public ElasticPermissionIndexer(ElasticsearchClient client, IConfiguration configuration)
        {
            _client = client;
            _indexName = configuration["Elasticsearch:PermissionIndex"]!;
        }

        public async Task IndexAsync(PermissionElasticDto dto)
        {
            var response = await _client.IndexAsync(dto, idx => idx.Index(_indexName));

            if (!response.IsValidResponse)
            {
                // Puedes usar Serilog o lanzar excepción si lo deseas
                Console.WriteLine("Error al indexar en Elasticsearch.");
            }
        }
    }
}
