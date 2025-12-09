// ReSharper disable once CheckNamespace
public static class PipeOperator
{
    extension<TInput, TResult>(TInput)
    {
        public static TResult operator |(TInput source, Func<TInput, TResult> func)
        {
            return func(source);
        }
    }
}
