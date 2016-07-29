using System;
using FluentNHibernate.Mapping;

namespace PosApp.Domain
{
    public class Promotion
    {
        public virtual Guid Id { get; set; }
        public virtual string Type { get; set; }
        public virtual string Barcode { get; set; }
    }

    class PromotionMap: ClassMap<Promotion>
    {
        public PromotionMap()
        {
            Table("promotions");
            Id(p => p.Id);
            Map(p => p.Type);
            Map(p => p.Barcode);
        }
    }
}