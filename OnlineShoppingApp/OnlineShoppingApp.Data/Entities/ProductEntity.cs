using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineShoppingApp.Data.Entities;

public class ProductEntity : BaseEntity
{
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    // Relational Property

    public ICollection<OrderProductEntity> OrderProducts { get; set; }
}

public class ProductConfiguration : BaseConfiguration<ProductEntity>
{
    public override void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.Property(x => x.ProductName)
               .IsRequired()
               .HasMaxLength(80);

        builder.Property(x => x.Price)
            .IsRequired()
            .HasPrecision(18,2)
            .HasAnnotation("MinValue", 0.01);
        
        builder.Property(x => x.StockQuantity)
            .IsRequired()
            .HasAnnotation("MinValue", 0);
        
        base.Configure(builder);
    }
}