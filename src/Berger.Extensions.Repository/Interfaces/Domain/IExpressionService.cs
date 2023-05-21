namespace Berger.Extensions.Repository
{
    public interface IExpressionService<T> where T : class
    {
        ExpressionService<T> Get();
    }
}