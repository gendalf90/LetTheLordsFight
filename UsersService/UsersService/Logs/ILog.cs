namespace UsersService.Logs
{
    public interface ILog
    {
        void Information(string message);

        void Warning(string message);
    }
}
