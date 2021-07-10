using Ciclope.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Ciclope.Data
{
    public class ApplicationDbContext : IdentityDbContext<CiclopeUser, ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {}

        public DbSet<Ciclope.Models.Empresa> Empresa { get; set; }
        public DbSet<Ciclope.Models.ProjectRole> ProjectRole { get; set; }
        public DbSet<Ciclope.Models.TrabalhadorUser> TrabalhadorUser { get; set; }
        public DbSet<Ciclope.Models.Faturas> Faturas { get; set; }
        public DbSet<Ciclope.Models.DocIUC> DocIUC { get; set; }
        public DbSet<Ciclope.Models.DocCertificadoPME> DocCertificadoPME { get; set; }
        public DbSet<Ciclope.Models.PMEClassificacao> PMEClassificacao { get; set; }
        public DbSet<Ciclope.Models.CertidaoPermanente> CertidaoPermanente { get; set; }
        public DbSet<Ciclope.Models.DocDMR> DocDMR { get; set; }
        public DbSet<DocsIVA> DocsIVA { get; set; }
        public DbSet<DocDRI> DocDRI { get; set; }
        public DbSet<Ciclope.Models.DocFundosCompensacao> DocFundosCompensacao { get; set; }
        public DbSet<Ciclope.Models.DocRecibosVerdes> DocRecibosVerdes { get; set; }
        public DbSet<Ciclope.Models.DocCertidaoAT> DocCertidaoAT { get; set; }
        public DbSet<Ciclope.Models.DocCertidaoSS> DocCertidaoSS { get; set; }
        public DbSet<Ciclope.Models.DocM22> DocM22 { get; set; }
        public DbSet<Ciclope.Models.DocIES> DocIES { get; set; }
        public DbSet<Ciclope.Models.DocRendas> DocRendas { get; set; }
        public DbSet<Ciclope.Models.DocRelatorioUnico> DocRelatorioUnico { get; set; }
        public DbSet<Ciclope.Models.DocIMI> DocIMI { get; set; }
        public DbSet<Ciclope.Models.DocCRC> DocCRC { get; set; }
        public DbSet<Ciclope.Models.DividaAT> DividaAT { get; set; }
        public DbSet<Ciclope.Models.DividaSS> DividaSS { get; set; }
        public DbSet<Ciclope.Models.DocBCB> DocBCB { get; set; }
        public DbSet<Ciclope.Models.CashFlow> CashFlow { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }

    }

    
}

