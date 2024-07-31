using System.Reflection;

namespace Core.Application.Extensions
{
    public static class ObjectExtension
    {
        private static Dictionary<Tuple<Type, Type>, Dictionary<string, PropertyInfo>> propertyCache = new Dictionary<Tuple<Type, Type>, Dictionary<string, PropertyInfo>>();

        public static TSource CopyPropertiesFrom<TSource>(this TSource target, object source)
        {
            if (source == null || target == null)
            {
                throw new ArgumentNullException();
            }

            Type sourceType = source.GetType();
            Type targetType = typeof(TSource);

            Dictionary<string, PropertyInfo> sourceProperties;
            Tuple<Type, Type> key = Tuple.Create(sourceType, targetType);

            if (!propertyCache.TryGetValue(key, out sourceProperties))
            {
                sourceProperties = sourceType.GetProperties().ToDictionary(p => p.Name);
                propertyCache[key] = sourceProperties;
            }

            PropertyInfo[] targetProperties = targetType.GetProperties();

            foreach (var targetProperty in targetProperties)
            {
                PropertyInfo sourceProperty;
                if (sourceProperties.TryGetValue(targetProperty.Name, out sourceProperty) &&
                    sourceProperty.PropertyType == targetProperty.PropertyType &&
                    sourceProperty.CanRead && targetProperty.CanWrite)
                {
                    object value = sourceProperty.GetValue(source);


                    targetProperty.SetValue(target, value);
                }
            }

            return target;
        }

        public static TTarget CopyPropertiesTo<TTarget>(this object source, TTarget target)
        {
            if (source == null || target == null)
            {
                throw new ArgumentNullException();
            }

            Type sourceType = source.GetType();
            Type targetType = typeof(TTarget);

            PropertyInfo[] sourceProperties = sourceType.GetProperties();
            PropertyInfo[] targetProperties = targetType.GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                foreach (var targetProperty in targetProperties)
                {
                    if (sourceProperty.Name == targetProperty.Name &&
                        sourceProperty.PropertyType == targetProperty.PropertyType &&
                        sourceProperty.CanRead && targetProperty.CanWrite)
                    {
                        object value = sourceProperty.GetValue(source);
                        targetProperty.SetValue(target, value);
                        break;
                    }
                }
            }

            return target;
        }

        public static TSource OldCopyPropertiesFrom<TSource>(this TSource target, object source)
        {
            if (source == null || target == null)
            {
                throw new ArgumentNullException();
            }

            Type sourceType = source.GetType();
            Type targetType = typeof(TSource);

            PropertyInfo[] sourceProperties = sourceType.GetProperties();
            PropertyInfo[] targetProperties = targetType.GetProperties();

            foreach (var targetProperty in targetProperties)
            {
                foreach (var sourceProperty in sourceProperties)
                {
                    if (sourceProperty.Name == targetProperty.Name &&
                        sourceProperty.PropertyType == targetProperty.PropertyType &&
                        sourceProperty.CanRead && targetProperty.CanWrite)
                    {
                        object value = sourceProperty.GetValue(source);
                        targetProperty.SetValue(target, value);
                        break;
                    }
                }
            }

            return target;
        }
    }
}
