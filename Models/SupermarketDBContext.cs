using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Supermarket.Models
{
    public class SupermarketDBContext : DbContext
    {
        public SupermarketDBContext() : base("name=supermarketDB")
        {
            Database.Log = sql => Debug.WriteLine(sql);
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Stock> Inventory { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ProductReceipt> ProductReceipts { get; set; }
        public DbSet<Offer> Offers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
              .HasRequired(product => product.Category)
              .WithMany(category => category.Products)
              .HasForeignKey(product => product.CategoryId);

            modelBuilder.Entity<Product>()
                .HasRequired(product => product.Manufacturer)
                .WithMany(manufacturer => manufacturer.Products)
                .HasForeignKey(product => product.ManufacturerId);

            modelBuilder.Entity<Stock>()
                .HasRequired(stock => stock.Product)
                .WithMany(product => product.Inventory)
                .HasForeignKey(stock => stock.ProductId);

            modelBuilder.Entity<Receipt>()
                .HasRequired(receipt => receipt.Cashier)
                .WithMany(user => user.Receipts)
                .HasForeignKey(receipt => receipt.CashierId);

            modelBuilder.Entity<ProductReceipt>()
                .HasRequired(productReceipt => productReceipt.Receipt)
                .WithMany(receipt => receipt.ProductReceipts)
                .HasForeignKey(productReceipt => productReceipt.ReceiptId);

            modelBuilder.Entity<ProductReceipt>()
                .HasRequired(productReceipt => productReceipt.Product)
                .WithMany(product => product.ProductReceipts)
                .HasForeignKey(productReceipt => productReceipt.ProductId);

            modelBuilder.Entity<Offer>()
                .HasRequired(offer => offer.Product)
                .WithMany(product => product.Offers)
                .HasForeignKey(offer => offer.ProductId);
        }
    }
}
