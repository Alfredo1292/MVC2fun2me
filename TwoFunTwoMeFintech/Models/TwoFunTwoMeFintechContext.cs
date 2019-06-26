using System.Data.Entity;

using TwoFunTwoMeFintech.Models.DTO;

namespace TwoFunTwoMeFintech.Models
{
    public class TwoFunTwoMeFintechContext : DbContext
    {
        // Puede agregar código personalizado a este archivo. Los cambios no se sobrescribirán.
        // 
        // Si desea que Entity Framework lo omita y regenere la base de datos
        // automáticamente siempre que cambie el esquema de modelo, agregue
        // el siguiente código al método Application_Start en el archivo Global.asax.
        // Nota: esta operación destruirá y volverá a crear la base de datos con cada cambio de modelo.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<TwoFunTwoMeFintech.Models.TwoFunTwoMeFintechContext>());

        public TwoFunTwoMeFintechContext() : base("name=TwoFunTwoMeFintechContext")
        {
        }

        public DbSet<agente> agentes { get; set; }

    
        public DbSet<dto_login> dto_login { get; set; }

        public DbSet<AsignacionBuckets> AsignacionBuckets { get; set; }

        public DbSet<Solicitudes> Solicitudes { get; set; }

        public DbSet<ScoreCardExperto> ScoreCardExpertoes { get; set; }

        public DbSet<Roles> Roles { get; set; }

        public DbSet<BucketCobros> BucketCobros { get; set; }

		public DbSet<Rules> Rules { get; set; }

		public DbSet<clsSUBMENU> clsSUBMENUs { get; set; }
<<<<<<< HEAD

		public DbSet<clsMAINMENU> clsMAINMENUs { get; set; }

		public DbSet<AgenteMenu> AgenteMenus { get; set; }

		public DbSet<GestionGeneral> GestionGenerals { get; set; }

=======
>>>>>>> 02077533187183e7a76adbfd15db5d101424f851
	}
}