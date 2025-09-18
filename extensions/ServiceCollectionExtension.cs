
using myblog.Repository.blog;
using myblog.Repository.global.blog;
using myblog.Repository.users;
using myblog.services.auth;
using myblog.services.blogs;
using myblog.services.global.post;
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
            services.AddScoped<IblogGlobalReposiotory, blogGlobalReposiotory>();
            services.AddScoped<IGlobalBlogServices , GlobalBlogServices>();
            return services;
        }
    }
}