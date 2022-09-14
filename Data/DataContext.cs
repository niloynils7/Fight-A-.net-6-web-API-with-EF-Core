using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext // an instance with dbcontex means a session of database
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {
            
        }
        public DbSet<Character> Characters => Set<Character>(); // Ekhane je name deya hobe, oi name er table create hobe
        public DbSet<User>  Users => Set<User>();
        public DbSet<Weapon> Weapons => Set<Weapon>();
    }
}