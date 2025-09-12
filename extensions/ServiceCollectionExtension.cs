
using myblog.Repository.blog;
using myblog.services.auth;
using myblog.services.blogs;
using myblog.Services;

namespace myblog.extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBlogServices(this IServiceCollection services)
        {
            services.AddScoped<IblogRepository, BlogRepository>();
            services.AddScoped<IblogService, blogServices>();
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}