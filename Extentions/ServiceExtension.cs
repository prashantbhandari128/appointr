using Appointr.Persistence.Repository.Implementation;
using Appointr.Persistence.Repository.Interface;
using Appointr.Persistence.UnitOfWork.Implementation;
using Appointr.Persistence.UnitOfWork.Interface;

namespace Appointr.Extentions
{
    public static class ServiceExtension
    {
        //-----------------------------[ Inject Helper Here ]-----------------------------------
        //public static void AddHelpers(this IServiceCollection services)
        //{

        //}
        //--------------------------------------------------------------------------------------

        //---------------------------[ Inject Repository Here ]---------------------------------
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IPostRepository, PostRepository>();
        }
        //--------------------------------------------------------------------------------------

        //--------------------------[ Inject Unit Of Work Here ]--------------------------------
        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        //--------------------------------------------------------------------------------------
    }
}
