using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;
using PosApp.Domain;

namespace PosApp.Repositories
{
    public class PromotionRespository : IPromotionRepository
    {
        readonly ISession m_session;

        public PromotionRespository(ISession session)
        {
            m_session = session;
        }

        public IList<Promotion> GetByBarcodes(params string[] barcodes)
        {
            return m_session.Query<Promotion>()
                .Where(p => barcodes.Contains(p.Barcode))
                .ToArray();
        }

        public IList<string> GetByType(string type)
        {
            return m_session.Query<Promotion>()
                .Where(p => p.Type == type).Select(p => p.Barcode).ToArray();
        }

        public bool IsInPromotions(params string[] barcodes)
        {
            List<Promotion> promotions = m_session.Query<Promotion>().ToList();
            foreach (string barcode in barcodes)
                foreach (Promotion promotion in promotions)
                {
                    if (barcode.Equals(promotion.Barcode)) return true;
                }
            return false;
        }

        public int CountByBarcodes(IList<string> barcodes)
        {
            return m_session.Query<Promotion>()
                .Count(p => barcodes.Contains(p.Barcode));
        }

        public void Save(IEnumerable<Promotion> promotions)
        {
            promotions.ForEach(p => m_session.Save(p));
            m_session.Flush();
        }

        public void Save(Promotion promotion, bool flushOnSave = true)
        {
            m_session.Save(promotion);
            if (flushOnSave)
            {
                m_session.Flush();
            }
        }

        public void Delect(string[] barcodes, string type)
        {
            IList<Promotion> promotions = GetByBarcodes(barcodes);
            foreach (Promotion promotion in promotions)
            {
                m_session.Delete(promotion);
            }
             
            m_session.Flush();
        }
    }
}