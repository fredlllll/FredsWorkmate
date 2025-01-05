using Microsoft.AspNetCore.Mvc.Rendering;

namespace FredsWorkmate.Util.HtmlHelperExtensions
{
    public static class HtmlHelper_EnumSelect
    {
        public static void EnumSelect<T>(this IHtmlHelper htmlHelper, string name, T? selected = default) where T : struct, Enum
        {
            Type t = typeof(T);
            var writer = htmlHelper.ViewContext.Writer;

            writer.WriteLine($"<select name=\"{name}\">");
            writer.WriteLine($"<option value=\"\">Not selected ({t.Name})</option>");
            var values = Enum.GetValues<T>();
            foreach (T e in values)
            {
                if (selected != null && Equals(selected.Value, e))
                {
                    writer.WriteLine($"<option selected value=\"{e}\">{e}</option>");
                }
                else
                {
                    writer.WriteLine($"<option value=\"{e}\">{e}</option>");
                }
            }
            writer.WriteLine("</select>");
        }
    }
}
