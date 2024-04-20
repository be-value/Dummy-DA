using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Data;
using Microsoft.Azure.Functions.Worker;

namespace Api
{
    public class ProductsPut(IProductData productData, ILogger<ProductsPut> logger)
    {
        [Function("ProductsPut")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "products")] HttpRequest req)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonSerializer.Deserialize<Product>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var updatedProduct = await productData.UpdateProduct(product!);
            return new OkObjectResult(updatedProduct);
        }
    }
}
