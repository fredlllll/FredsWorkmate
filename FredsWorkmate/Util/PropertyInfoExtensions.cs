using System.Reflection;

namespace FredsWorkmate.Util
{
    public static class PropertyInfoExtensions
    {
        public static bool IsNullable(this PropertyInfo property)
        {
            NullabilityInfoContext nullabilityInfoContext = new NullabilityInfoContext();
            var info = nullabilityInfoContext.Create(property);
            if (info.WriteState == NullabilityState.Nullable || info.ReadState == NullabilityState.Nullable)
            {
                return true;
            }

            return false;
        }
    }
}
