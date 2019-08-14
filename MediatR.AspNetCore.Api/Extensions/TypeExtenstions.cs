using System.Collections.Generic;

namespace System.Reflection
{
    internal static class TypeExtenstions
    {
        public static bool IsNullable(this Type type)
        {
            if (ReferenceEquals(type, null))
            {
                throw new ArgumentNullException(nameof(type));
            }
            return (((type != null) && type.IsGenericType) &&
                (type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }

        public static Type GetNonNullableType(this Type type)
        {
            if (ReferenceEquals(type, null))
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (IsNullable(type))
            {
                return type.GetGenericArguments()[0];
            }
            return type;
        }
        public static bool IsEnumerableType(this Type enumerableType)
        {
            if (ReferenceEquals(enumerableType, null))
            {
                throw new ArgumentNullException(nameof(enumerableType));
            }

            return (FindGenericType(enumerableType, typeof(IEnumerable<>)) != null);
        }

        public static bool IsKindOfGeneric(this Type type, Type definition)
        {
            if (ReferenceEquals(type, null))
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (ReferenceEquals(definition, null))
            {
                throw new ArgumentNullException(nameof(definition));
            }
            return (FindGenericType(type, definition) != null);
        }

        public static Type FindGenericType(this Type type, Type definition)
        {
            if (ReferenceEquals(type, null))
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (ReferenceEquals(definition, null))
            {
                throw new ArgumentNullException(nameof(definition));
            }
            while ((type != null) && (type != typeof(object)))
            {
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == definition))
                {
                    return type;
                }
                if (definition.IsInterface)
                {
                    foreach (Type type2 in type.GetInterfaces())
                    {
                        Type type3 = FindGenericType(type2, definition);
                        if (type3 != null)
                        {
                            return type3;
                        }
                    }
                }
                type = type.BaseType;
            }
            return null;
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            if (ReferenceEquals(type, null))
            {
                throw new ArgumentNullException(nameof(type));
            }

            type = type.BaseType();

            while (type != null)
            {
                yield return type;

                type = type.BaseType();
            }
        }

        public static Type BaseType(this Type type)
        {
            if (ReferenceEquals(type, null))
            {
                throw new ArgumentNullException(nameof(type));
            }
            return type.GetTypeInfo().BaseType;
        }

    }
}