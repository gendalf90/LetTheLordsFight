namespace UsersService.Queries
{
    public interface IFactory
    {
        IQuery<string> CreateGetTokenQuery();
    }
}
