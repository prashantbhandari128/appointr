using Appointr.Helper.Implementation;
using Appointr.Helper.Interface;
using Appointr.Persistence.Repository.Implementation;
using Appointr.Persistence.Repository.Interface;
using Appointr.Persistence.UnitOfWork.Implementation;
using Appointr.Persistence.UnitOfWork.Interface;
using Appointr.Service.Implementation;
using Appointr.Service.Interface;

namespace Appointr.Extentions
{
    public static class ServiceExtension
    {
        //---------------------------[ Inject Repository Here ]---------------------------------
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IActivityRepository, ActivityRepository>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<IOfficerRepository, OfficerRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IVisitorRepository, VisitorRepository>();
            services.AddTransient<IWorkDayRepository, WorkDayRepository>();
        }
        //--------------------------------------------------------------------------------------

        //--------------------------[ Inject Unit Of Work Here ]--------------------------------
        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        //--------------------------------------------------------------------------------------

        //-----------------------------[ Inject Service Here ]----------------------------------
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IActivityService, ActivityService>();
            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddTransient<IOfficerService, OfficerService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IVisitorService, VisitorService>();
            services.AddTransient<IWorkDayService, WorkDayService>();
        }
        //--------------------------------------------------------------------------------------

        //-----------------------------[ Inject Helper Here ]-----------------------------------
        public static void AddHelpers(this IServiceCollection services)
        {
            services.AddTransient<IToastrHelper, ToastrHelper>();
            services.AddTransient<IConsoleHelper, ConsoleHelper>();
        }
        //--------------------------------------------------------------------------------------
    }
}
