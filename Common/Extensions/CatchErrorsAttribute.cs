using AspectInjector.Broker;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Common.Extensions;

/// <summary>
/// Catches any uncaught exceptions and logs given that the class has a public ILogger property named "Logger"
/// </summary>
[Injection(typeof(CatchErrorsAttribute))]
[Aspect(Scope.Global)]
public class CatchErrorsAttribute : Attribute
{
    private static MethodInfo _asyncErrorHandler = typeof(CatchErrorsAttribute).GetMethod(nameof(WrapAsync), BindingFlags.NonPublic | BindingFlags.Static);
    private static MethodInfo _syncErrorHandler = typeof(CatchErrorsAttribute).GetMethod(nameof(Wrap), BindingFlags.NonPublic | BindingFlags.Static);

    [Advice(Kind.Around)]
    public object Around([Argument(Source.Instance)] object instance, [Argument(Source.Type)] Type type, [Argument(Source.Arguments)] object[] args,
        [Argument(Source.Target)] Func<object[], object> target, [Argument(Source.ReturnType)] Type returnType)
    {
        if (typeof(Task).IsAssignableFrom(returnType))
        {
            if (returnType.IsConstructedGenericType)
            {
                var syncResultType = returnType.IsConstructedGenericType ? returnType.GenericTypeArguments[0] : typeof(object);
                return _asyncErrorHandler.MakeGenericMethod(syncResultType).Invoke(this, new object[] { instance, type, target, args });
            }
            else
                return WrapAsyncVoid(instance, type, target, args);
        }
        else if (returnType != typeof(void))
        {
            return _syncErrorHandler.MakeGenericMethod(returnType).Invoke(this, new object[] { instance, type, target, args });
        }
        else
        {
            try
            {
                return target(args);
            }
            catch (Exception e)
            {
                LogError(instance, type, e.ToString());
                return null;
            }
        }
    }

    private static T Wrap<T>(object instance, Type type, Func<object[], object> target, object[] args)
    {
        try
        {
            return (T)target(args);
        }
        catch (Exception e)
        {
            LogError(instance, type, e.ToString());

            if (typeof(T) == typeof(void))
                return default;

            // Return default value if non-nullable type
            if (typeof(T).IsValueType)
                return (T)Activator.CreateInstance(typeof(T));

            return default;
        }
    }

    private static async Task<T> WrapAsync<T>(object instance, Type type, Func<object[], object> target, object[] args)
    {
        try
        {
            return await (Task<T>)target(args);
        }
        catch (Exception e)
        {
            LogError(instance, type, e.ToString());

            if (typeof(T) == typeof(void))
                return default;

            // Return default value if non-nullable type
            if (typeof(T).IsValueType)
                return (T)Activator.CreateInstance(typeof(T));

            return default;
        }
    }

    private static async Task WrapAsyncVoid(object instance, Type type, Func<object[], object> target, object[] args)
    {
        try
        {
            await (Task)target(args);
        }
        catch (Exception e)
        {
            LogError(instance, type, e.ToString());
        }
    }

    private static void LogError(object instance, Type type, string error)
    {
        PropertyInfo p_logger = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(x => x.Name.Equals("Logger", StringComparison.OrdinalIgnoreCase));

        if (p_logger != null)
        {
            var pv_logger = p_logger.GetValue(instance);

            ILogger logger = pv_logger as ILogger;
            logger.LogError(error);
        }
    }
}
