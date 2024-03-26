using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Route.C41.G01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G01.DAL.Data.Configurations
{
    internal class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Fluent APIs for "Employee" Domain

            builder.Property(E => E.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();

            builder.Property(E => E.Address).IsRequired();

            builder.Property(E => E.Salary).HasColumnType("decimal(12,2)");

            builder.Property(E => E.Gender)
                .HasConversion(
                (gender) => gender.ToString(),
                (gender) => (Gender) Enum.Parse(typeof(Gender), gender, true)
                );

            builder.Property(E => E.EmployeeType)
                .HasConversion(
                (EmployeeType) => EmployeeType.ToString(),
                (EmployeeType) => (EmpType) Enum.Parse(typeof(EmpType), EmployeeType, true)
                );
        }
    }
}
