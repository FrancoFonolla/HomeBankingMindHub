﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Threading.Tasks;

namespace HomeBankingMindHub.Models
{
    public class HomeBankingContext : DbContext
    {
        public HomeBankingContext(DbContextOptions<HomeBankingContext> options) : base(options) { }
        //dbsets
        public DbSet<Client> Clients { get; set; }
    }
}
