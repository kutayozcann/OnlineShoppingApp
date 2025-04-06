using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineShoppingApp.Data.Entities;

public class OrderProductEntity : BaseEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    
    // Relational Property
    public OrderEntity Order { get; set; }
    public ProductEntity Product { get; set; }
}

public class OrderProductConfiguration : BaseConfiguration<OrderProductEntity>
{
    public override void Configure(EntityTypeBuilder<OrderProductEntity> builder)
    {
        builder.Ignore(x => x.Id); //Ignoring Id property
        builder.HasKey("OrderId", "ProductId");
        //Composite Key as a new Primary Key

        builder.HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(op => op.Product)
            .WithMany()
            .HasForeignKey(op => op.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasDefaultValue(1)
            .HasAnnotation("MinValue", 1);
        
        base.Configure(builder);
    }
}