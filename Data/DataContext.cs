using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext // an instance with dbcontex means a session of database
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill { Id = 1, Name = "Fireball", Damage = 60},
                new Skill { Id = 2, Name = "Thunder", Damage = 80},
                new Skill { Id = 3, Name = "Blizzard", Damage = 30}
            );
        }

        public DbSet<Character> Characters => Set<Character>(); // Ekhane je name deya hobe, oi name er table create hobe
        public DbSet<User> Users => Set<User>();
        public DbSet<Weapon> Weapons => Set<Weapon>();
        public DbSet<Skill> Skills => Set<Skill>();
    }
}
