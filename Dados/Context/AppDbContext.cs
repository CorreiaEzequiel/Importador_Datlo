﻿using Dominio.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Dataset> Datasets { get; set; }
        public DbSet<Coluna> Colunas { get; set; }
        public DbSet<Registro> Registros { get; set; }
        public DbSet<ValorRegistro> ValorRegistros { get; set; }

    }
}
