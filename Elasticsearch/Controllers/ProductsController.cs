using Elasticsearch.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Elasticsearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IElasticClient elasticClient,ILogger<ProductsController>logger)
        {
           _elasticClient = elasticClient;
           _logger = logger;
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get(string KeyWord)
        {
            var result = await _elasticClient.SearchAsync<Product>(
                s => s.Query(
                    q => q.QueryString(
                        d => d.Query('*' + KeyWord + '*'))).Size(50));
            return Ok(result.Documents.ToList());
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(Product product)
        {
            await _elasticClient.IndexDocumentAsync(product);
            return Ok();
        }
    }
}
