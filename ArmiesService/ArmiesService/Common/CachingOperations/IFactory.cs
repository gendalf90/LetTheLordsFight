namespace ArmiesService.Common.CachingOperations
{
    interface IFactory
    {
        IGetEntityStrategy<T> CreateGetEntityStrategy<T>(SearchEntityParams searchParams) where T : class;

        IInsertEntityStrategy<T> CreateInsertEntityStrategy<T>(SearchEntityParams searchParams) where T : class;

        IGetAllStrategy<T> CreateGetAllStrategy<T>(SearchAllParams searchParams) where T : class;
    }
}
