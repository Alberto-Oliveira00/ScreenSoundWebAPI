﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ScreenSound.Modelos;
using ScreenSound.Shared.Modelos.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSound.Banco;
public class ScreenSoundContext: DbContext
{
    public DbSet<Artista> Artistas { get; set; }
    public DbSet<Musica> Musicas { get; set; }
    public DbSet<Genero> Generos { get; set; }

    private string connectionString = "Data Source=Bebeto\\SQLSERVER;Initial Catalog=ScreenSoundV0; Integrated Security=True;Trust Server Certificate=True;Application Name=vscode-mssql;Application Intent=ReadWrite";

    public ScreenSoundContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;
        optionsBuilder
            .UseSqlServer(connectionString)
            .UseLazyLoadingProxies(); 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Musica>()
            .HasMany(c => c.Generos)
            .WithMany(c => c.Musicas);
    }
}
