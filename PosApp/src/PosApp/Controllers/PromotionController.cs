using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PosApp.Services;

namespace PosApp.Controllers
{
    public class PromotionController : ApiController
    {
        readonly PromotionService m_promotionService;

        public PromotionController(PromotionService promotionService)
        {
            m_promotionService = promotionService;
        }

        [HttpPost]
        public HttpResponseMessage CreatePromotion(string type, string[] promotionBarcodes)
        {
            if (!IsVaild(promotionBarcodes))
            {
               return Request.CreateResponse(HttpStatusCode.BadRequest); 
            }
            try
            {
                m_promotionService.InsertPromotionBarcode(promotionBarcodes, type);
            }
            catch (ArgumentException)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        static bool IsVaild(string[] promotionBarcodes)
        {
            return promotionBarcodes.All(promotionBarcode => !promotionBarcode.Equals(""));
        }

        [HttpGet]
        public HttpResponseMessage GetPromotion(string type)
        {
            return m_promotionService.GetPromotionBarcode(type).Length != 0
                ? Request.CreateResponse(HttpStatusCode.OK, m_promotionService.GetPromotionBarcode(type))
                : Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [HttpDelete]
        public HttpResponseMessage DeletePromotion(string[] promotionBarcodes, string type)
        {
            m_promotionService.DelectPromotionBarcode(promotionBarcodes, type);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}