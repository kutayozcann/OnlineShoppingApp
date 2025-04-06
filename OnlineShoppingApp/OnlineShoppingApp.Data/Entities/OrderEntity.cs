using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineShoppingApp.Data.Entities;

public class OrderEntity : BaseEntity
{
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    //Foreign Key
    public int CustomerId { get; set; }
    public UserEntity Customer { get; set; }

    // Relational Property
    public ICollection<OrderProductEntity> OrderProducts { get; set; }

    public OrderEntity()
    {
        OrderProducts = new List<OrderProductEntity>(); // Cant be null
    }
}

public class OrderConfiguration : BaseConfiguration<OrderEntity>
{
    public override void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.Property(x => x.TotalAmount)
            .IsRequired();

        builder.HasOne(o => o.Customer)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        base.Configure(builder);
    }
}