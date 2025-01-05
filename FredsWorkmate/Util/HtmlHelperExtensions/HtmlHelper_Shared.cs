using FredsWorkmate.Database.Models;
using System.Reflection;

namespace FredsWorkmate.Util.HtmlHelperExtensions
{
    public static class HtmlHelper_Shared
    {
        public static List<PropertyInfo> GetSortedDisplayProperties(Type t)
        {
            List<PropertyInfo> properties = new(t.GetProperties());
            int index = properties.FindIndex(x => x.Name == nameof(Model.Id));
            properties.MoveItem(index, 0);
            index = properties.FindIndex(x => x.Name == nameof(Model.Created));
            properties.MoveItem(index, properties.Count);
            index = properties.FindIndex(x => x.Name == nameof(Model.Updated));
            properties.MoveItem(index, properties.Count);
            return properties;
        }
    }
}
