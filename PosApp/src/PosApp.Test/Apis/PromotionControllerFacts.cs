using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PosApp.Domain;
using PosApp.Test.Common;
using Xunit;

namespace PosApp.Test.Apis
{
    public class PromotionControllerFacts : ApiFactBase
    {
        [Fact]
        public async Task should_return_badRequest_when_input_is_not_vaild()
        {
            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "promotions/BUY_TWO_GET_ONE",
                new[] {""});

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task should_return_OK_when_barcode_is_not_in_promotion()
        {
            CreateProductFixture(
                new Product { Barcode = "barcode001", Price = 1M, Name = "Coca Cola" });
            CreatePromotionFixture(new Promotion { Barcode = "barcode002", Type = "BUY_TWO_GET_ONE" });
            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                $"promotions/{"BUY_TWO_GET_ONE"}",
                new[] { "barcode001" });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task should_return_ok_when_promotion_have_the_barcode_of_input()
        {
            CreateProductFixture(
                new Product { Barcode = "barcode001", Price = 1M, Name = "Coca Cola" });
            CreatePromotionFixture(new Promotion { Barcode = "barcode001", Type = "BUY_TWO_GET_ONE" });

            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                $"promotions/{"BUY_TWO_GET_ONE"}",
                new[] { "barcode001" });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task should_return_400_when_product_dont_have_the_barcode_of_input()
        {
            CreateProductFixture(
                new Product {Barcode = "barcode001", Price = 1M, Name = "Coca Cola"});

            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                $"promotions/{"BUY_TWO_GET_ONE"}",
                new[] { "barcode000" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task should_return_all_promotion_barcodes()
        {
            CreatePromotionFixture(new Promotion { Barcode = "barcode001", Type = "BUY_TWO_GET_ONE" });

            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(
                $"promotions/{"BUY_TWO_GET_ONE"}");

            string[] barcodes = await response.Content.ReadAsAsync<string[]>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("barcode001", barcodes[0]);
        }

        [Fact]
        public async Task should_return_400_when_the_type_is_not_exited()
        {
            CreatePromotionFixture(new Promotion { Barcode = "barcode001", Type = "BUY_TWO_GET_ONE" });

            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(
                $"promotions/{"BUY_ONE_GET_ONE"}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task should_return_ok_when_delect_promotion_barcode_success()
        {
            CreatePromotionFixture(new Promotion { Barcode = "barcode001", Type = "BUY_TWO_GET_ONE" });

            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response =
                await httpClient.DeleteAsync($"promotions/{"BUY_ONE_GET_ONE"}"
                    ,new [] {"barcode001"});

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        void CreateProductFixture(params Product[] products)
        {
            Array.ForEach(products, p => Fixtures.Products.Create(p));
        }

        void CreatePromotionFixture(params Promotion[] promotions)
        {
            Array.ForEach(promotions, p => Fixtures.Promotions.Create(p));
        }

    }
}