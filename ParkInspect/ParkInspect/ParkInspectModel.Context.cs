﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParkInspect
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class ParkInspectEntities : DbContext
    {
        public ParkInspectEntities()
            : base("name=ParkInspectEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Absence> Absences { get; set; }
        public virtual DbSet<Asignment> Asignments { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Datatype> Datatypes { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Employee_Status> Employee_Status { get; set; }
        public virtual DbSet<Field> Fields { get; set; }
        public virtual DbSet<Form> Forms { get; set; }
        public virtual DbSet<Formfield> Formfields { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Inspection> Inspections { get; set; }
        public virtual DbSet<Parkinglot> Parkinglots { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ReportFieldType> ReportFieldTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Template> Templates { get; set; }
        public virtual DbSet<Contactperson> Contactpersons { get; set; }
    }
}
