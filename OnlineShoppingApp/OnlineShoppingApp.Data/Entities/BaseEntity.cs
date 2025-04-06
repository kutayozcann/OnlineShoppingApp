using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineShoppingApp.Data.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsDeleted { get; set; }
}

public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasQueryFilter(x => x.IsDeleted == false);
        //Filtering that will be valid for all queries and other linq operations to be written on this database
        //Never have to deal with “soft delete” data.
        
        builder.Property(x => x.ModifiedDate)
            .IsRequired(false);
    }
}