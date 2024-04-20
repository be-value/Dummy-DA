using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Data;
using Microsoft.Azure.Functions.Worker;

namespace Api
{
    public class ProductsPost(IProductData productData, ILogger<ProductsPost> logger)
    {
        [Function("ProductsPost")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")] HttpRequest req)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonSerializer.Deserialize<Product>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var newProduct = await productData.AddProduct(product!);
            return new OkObjectResult(newProduct);
        }
    }
}
