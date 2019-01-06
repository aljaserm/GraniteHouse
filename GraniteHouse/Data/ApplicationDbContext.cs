using System;
using System.Collections.Generic;
using System.Text;
using GraniteHouse.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductTypes> productTypes { get; set; }
        public DbSet<SpecialTags> specialTags { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Appointments> appointments { get; set; }
        public DbSet<AppointmentsProducts> appointmentsProducts { get; set; }
        public DbSet<Users> users { get; set; }
    }
}
