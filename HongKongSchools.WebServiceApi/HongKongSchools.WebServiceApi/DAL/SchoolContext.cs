using HongKongSchools.WebServiceApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace HongKongSchools.WebServiceApi.DAL
{
    public class SchoolContext : DbContext
    {
        public SchoolContext() : base("SchoolContext")
        {
            Database.Initialize(false);
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<FinanceType> FinanceTypes { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<SchoolName> Names { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<SchoolSession> SchoolSessions { get; set; }
        public DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}