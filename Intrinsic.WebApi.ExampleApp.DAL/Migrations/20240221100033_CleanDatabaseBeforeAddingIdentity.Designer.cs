﻿// <auto-generated />
using Intrinsic.WebApi.ExampleApp.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Intrinsic.WebApi.ExampleApp.DAL.Migrations
{
    [DbContext(typeof(ExampleWebApiContext))]
    [Migration("20240221100033_CleanDatabaseBeforeAddingIdentity")]
    partial class CleanDatabaseBeforeAddingIdentity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);
#pragma warning restore 612, 618
        }
    }
}