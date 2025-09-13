
using myblog.Repository.blog;
using myblog.Repository.users;
using myblog.services.auth;
using myblog.services.blogs;
using myblog.services.users;

namespace myblog.extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBlogServices(this IServiceCollection services)
        {
            services.AddScoped<IblogRepository, BlogRepository>();
            services.AddScoped<IblogService, blogServices>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}