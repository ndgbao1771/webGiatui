using App.Models.Product;
using App.Models.Contacts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using App.Models.Orders;
using App.Models.ServiceStores;
using App.Models;

namespace App.AppContext 
{
    // App.Models.AppDbContext
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            modelBuilder.Entity<Category> (entity => {
                entity.HasIndex(c => c.Slug)
                .IsUnique();
            });

            modelBuilder.Entity<ProductCategory> (entity => {
                entity.HasKey(c => new {c.ProductId, c.CategoryId});
            });

            modelBuilder.Entity<Products> (entity => {
                entity.HasIndex(c => c.Slug)
                .IsUnique();
            });

            modelBuilder.Entity<Material>(entity => {
                // Thiết lập cho bảng MonHocTienQuyet
                entity.HasOne(e => e.unitIngredientWeight)                          // Chỉ ra Entity là phía một (bảng MonHoc)
                        .WithMany()                                 // Chỉ ra Collection tập MonHocTienQuyet lưu ở phía một
                        .HasForeignKey("UnitIngredientWeightID")                     // Chỉ ra tên FK nếu muốn
                        .OnDelete(DeleteBehavior.Cascade)            // Ứng xử khi MonHoc bị xóa (Hoặc chọn DeleteBehavior.Cascade)
                        .HasConstraintName("FK_UnitIngredientWeightID");   // Tự đặt tên Constrain (ràng buốc)

            });

            modelBuilder.Entity<Material>(entity => {
                // Thiết lập cho bảng MonHocTienQuyet
                entity.HasOne(e => e.unitIngredientAmount)                       // Chỉ ra Entity là phía một (bảng MonHoc)
                        .WithMany()                              // Chỉ ra Collection tập Product lưu ở phía một
                        .HasForeignKey("UnitIngredientAmountID")                 // Chỉ ra tên FK nếu muốn
                        .OnDelete(DeleteBehavior.NoAction)            // Ứng xử khi MonHoc bị xóa (Hoặc chọn DeleteBehavior.Cascade)
                        .HasConstraintName("FK_UnitIngredientAmountID"); // Tự đặt tên Constrain (ràng buốc)

            });





        }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Category> categories {get; set;}

        public DbSet<Products> products {get; set;}

        public DbSet<ProductCategory> productCategories {get; set;}

        public DbSet<ProductPhoto> productPhotos {get; set;}

        public DbSet<Order> orders {get; set;}

        public DbSet<ServiceStore> serviceStores { get; set;}

        public DbSet<ElectricalEquipment> electricalEquipments { get; set;}

        public DbSet<CategoryElectricEquipment> categoryElectricEquipments { get; set;}

        public DbSet<Brand> brands { get; set;}

        public DbSet<CategoryIngredient> cateIngredients { get; set;}

        public DbSet<Material> materials { get; set;}

        public DbSet<OrderDetails> orderDetails { get; set;}

        public DbSet<WashService> washServices { get; set;}

        public DbSet<UnitIngredient> unitIngredients { get; set;}

        public DbSet<IssueAnInvoice> issueAnInvoices { get; set;}

        public DbSet<InfoShop> infoShops { get; set;}

    }
}
