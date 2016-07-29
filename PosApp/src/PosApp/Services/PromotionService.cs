using System;
using System.Collections.Generic;
using System.Linq;
using PosApp.Domain;
using PosApp.Repositories;

namespace PosApp.Services
{
    public class PromotionService
    {
        readonly IPromotionRepository m_promotionRepository;
        readonly IProductRepository m_productRepository;
        const string PROMOTION_TYPE_IS_BUY_TWO_GET_ONE = "BUY_TWO_GET_ONE";

        public PromotionService(IPromotionRepository promotionRepository, IProductRepository productRepository)
        {
            m_promotionRepository = promotionRepository;
            m_productRepository = productRepository;
        }

        public void ApplyPromotion(ReceiptItem receiptItem)
        {
            decimal subPromoted = (receiptItem.Amount / 3) * receiptItem.Product.Price;
            receiptItem.SubPromoted = subPromoted;
        }

        public ReceiptItem[] CalculateSubPromoted(ReceiptItem[] receiptItems)
        {
            foreach (ReceiptItem receiptItem in receiptItems)
            {
                IList<Promotion> promotionProducts = m_promotionRepository.GetByBarcodes(receiptItem.Product.Barcode);
                foreach (Promotion promotionProduct in promotionProducts)
                {
                    if (promotionProduct.Type.Equals(PROMOTION_TYPE_IS_BUY_TWO_GET_ONE))
                    {
                        ApplyPromotion(receiptItem);
                    }
                }
            }

            return receiptItems;
        }

        public void InsertPromotionBarcode(string[] promotionBarcodes, string type)
        {
            if (!m_productRepository.GetByBarcodes(promotionBarcodes)
                .Count.Equals(promotionBarcodes.Length))
            {
                throw new ArgumentException();
            }

            if (!m_promotionRepository.IsInPromotions(promotionBarcodes))
            {
                foreach (string promotionBarcode in promotionBarcodes)
                {
                    m_promotionRepository.Save(new Promotion { Barcode = promotionBarcode, Type = type });
                }
            }      
        }
        
        public string[] GetPromotionBarcode(string type)
        {
            return m_promotionRepository.GetByType(type).ToArray();
        }

        public void DelectPromotionBarcode(string[] promotionBarcodes, string type)
        {
            if (m_promotionRepository.IsInPromotions(promotionBarcodes))
            {
                m_promotionRepository.Delect(promotionBarcodes, type);
            }             
        }
    }
}