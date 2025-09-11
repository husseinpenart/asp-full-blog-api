
    using myblog.models.Private.blog;

    namespace myblog.Repository.blog
    {
        public interface IblogRepository
        {
            Task<blogModel> GetByIdAsync(Guid id);
            Task<List<blogModel>> GetAllAsync();
            Task AddAsync(blogModel blog);
            Task UpdateAsync(blogModel blog);
            Task DeleteAsync(Guid id);

        }
    }