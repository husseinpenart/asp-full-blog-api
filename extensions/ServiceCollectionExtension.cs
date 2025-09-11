
using myblog.Repository.blog;
using myblog.services.blogs;

namespace myblog.extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBlogServices(this IServiceCollection services)
        {
            services.AddScoped<IblogRepository, blogRepository>();
            services.AddScoped<IblogService, blogServices>();
            return services;
        }
    }
}