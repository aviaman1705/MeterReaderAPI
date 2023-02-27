using MeterReaderAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MeterReaderAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        public DbSet<Notebook> Notebooks { get; set; }
        public DbSet<Track> Tracks { get; set; }
    }
}
