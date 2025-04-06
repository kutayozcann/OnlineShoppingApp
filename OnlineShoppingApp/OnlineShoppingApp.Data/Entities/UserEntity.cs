using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShoppingApp.Data.Enums;

namespace OnlineShoppingApp.Data.Entities;

public class UserEntity : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public UserType  UserType { get; set; }
    
    public ICollection<OrderEntity> Orders { get; set; }
}

public class UserConfiguration : BaseConfiguration<UserEntity>
{
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(40);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(40);
        
        base.Configure(builder);
    }
}