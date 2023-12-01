using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechStoreEL.Entities;
using TechStoreEL.IdentityModels;

namespace TechStoreDL.AddContext
{
    
  public class TechStoreContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public TechStoreContext(DbContextOptions<TechStoreContext> opt)
            : base(opt)
        {

        }

        public virtual DbSet<Category> CategoryTable { get; set; }
        public virtual DbSet<CategoryProductProperty> CategoryProductPropertyTable { get; set; }
        public virtual DbSet<Order> OrderTable { get; set; }
        public virtual DbSet<OrderDetail> OrderDetailTable { get; set; }
        public virtual DbSet<Product> ProductTable { get; set; }
        public virtual DbSet<ProductDiscount> ProductDiscountTable { get; set; }
        public virtual DbSet<ProductPicture> ProductPictureTable { get; set; }
        public virtual DbSet<ProductProperty> ProductPropertyTable { get; set; }

		public virtual DbSet<SystemDefaultParameter> SystemDefaultParameterTable { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           builder.Entity<ContactMessage>().ToTable("ContactMessageTable");
            //builder.Entity<ContactMessage>(x =>
            //{
            //    x.ToTable("ContactMessageTablo");
            //});

        }
    }
}
