using System;
using Autofac;
using PosApp.Domain;
using PosApp.Services;
using PosApp.Test.Common;
using Xunit;
using Xunit.Abstractions;


namespace PosApp.Test.Unit
{
    public class PromotionFacts : FactBase
    {
        public PromotionFacts(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public void should_return_ArgumentException_when_barcode_is_not_in_product()
        {
            CreateProductFixture(
                new Product { Barcode = "barcode000", Price = 10M, Name = "I do not care" });

            PromotionService promotionService = CreatePromotionService();
            var barcodes = new[] {"barcode001"};

            Assert.Throws<ArgumentException>(
                () => promotionService.InsertPromotionBarcode(barcodes, "BUY_TWO_GET_ONE"));
        }

        [Fact]
        public void should_get_promotion_when_insert_promotion()
        {
            CreateProductFixture(
                new Product { Barcode = "barcode001", Price = 10M, Name = "I do not care" });
            //CreatePromotionFixture(new Promotion { Barcode = "barcode000", Type = "BUY_TWO_GET_ONE" });

            PromotionService promotionService = CreatePromotionService();
            var barcodes = new[] { "barcode001" };

            promotionService.InsertPromotionBarcode(barcodes, "BUY_TWO_GET_ONE");
            string[] promotionBarcode = promotionService.GetPromotionBarcode("BUY_TWO_GET_ONE");

            Assert.Equal("barcode001", promotionBarcode[0]);
        }

        [Fact]
        public void should_cant_find_barcode_in_promotion_when_delete_it()
        {
            CreatePromotionFixture(new Promotion { Barcode = "barcode001", Type = "BUY_TWO_GET_ONE" }
            , new Promotion {Barcode = "barcode002", Type = "BUY_TWO_GET_ONE"});

            PromotionService promotionService = CreatePromotionService();
            var barcodes = new[] { "barcode001" };

            promotionService.DelectPromotionBarcode(barcodes, "BUY_TWO_GET_ONE");

            string[] promotionBarcode = promotionService.GetPromotionBarcode("BUY_TWO_GET_ONE");

            Assert.Equal("barcode002", promotionBarcode[0]);
        }


        PromotionService CreatePromotionService()
        {
            var promotionService = GetScope().Resolve<PromotionService>();
            return promotionService;
        }

        void CreatePromotionFixture(params Promotion[] promotions)
        {
            Array.ForEach(promotions, p => Fixtures.Promotions.Create(p));
        }

        void CreateProductFixture(params Product[] products)
        {
            Array.ForEach(products, p => Fixtures.Products.Create(p));
        }
    }
}