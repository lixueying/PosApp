using System.Collections.Generic;
using PosApp.Domain;

namespace PosApp.Repositories
{
    public interface IPromotionRepository
    {
        IList<Promotion> GetByBarcodes(params string[] barcodes);
        int CountByBarcodes(IList<string> barcodes);
        void Save(Promotion promotion, bool saveOnFlush = true);
        void Save(IEnumerable<Promotion> promotions);
        bool IsInPromotions(params string[] barcodes);
        IList<string> GetByType(string type);
        void Delect(string[] barcode, string type);
    }
    
}