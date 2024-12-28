using System.Reflection;

namespace FredsWorkmate.Util
{
    public static class TypeExtensions
    {
        /// <summary>
        /// checks if the type is this generic type
        /// </summary>
        /// <example>t.IsGenericType(typeof(ICollection<>))</example>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsGenericType(this Type type, Type genericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
        }

        /// <summary>
        /// Get only properties that can be edited
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetEditableProperties(this Type t)
        {
            foreach (PropertyInfo property in t.GetProperties())
            {
                if (!Attribute.IsDefined(property, typeof(AutoParameterAttribute)) &&
                    !property.PropertyType.IsGenericType(typeof(ICollection<>)))
                {
                    yield return property;
                }
            }
        }
    }
}
