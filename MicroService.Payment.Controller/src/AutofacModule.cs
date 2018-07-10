using Autofac;
using Microsoft.Extensions.Configuration;
using MicroService.Common.Core.Databases.Repository;
using MicroService.Common.Core.Databases.Repository.MongoDb;
using MicroService.Common.Core.Web;
using MicroService.Payment.Service.Steam;
using MicroService.Payment.Service.Steam.Services;

namespace MicroService.Payment.Controller
{
    public class AutofacModule : Module
    {
        private readonly IConfiguration _configuration;

        public AutofacModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            #region Services

            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));

            builder.RegisterType<PricingService>().As<IPricingService>();

            #endregion

            #region HTTP

            builder.RegisterType<HttpRequestParser>().As<IHttpRequestParser>();

            #endregion

            #region SQL

            #endregion

            #region Mongo

            var mongoConnectionString = _configuration["MongoDB:ConnectionString"];
            var mongoDatabaseName = _configuration["MongoDB:Database"];

            
            builder.RegisterGeneric(typeof(MongoRepository<,>)).As(typeof(IRepository<,>));
            builder.RegisterType<MongoPricingRepository>().As<IMongoPricingRepository>();

            builder.Register(c => new MongoDbConnectionFacotry(mongoConnectionString, mongoDatabaseName)).As<IMongoDbConnectionFacotry>();
            #endregion
        }
    }
}