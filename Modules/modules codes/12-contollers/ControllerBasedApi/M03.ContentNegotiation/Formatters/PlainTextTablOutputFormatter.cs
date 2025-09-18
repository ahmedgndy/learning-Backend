using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace M03.ContentNegotiation.Formatters;

public class PlainTextTableOutputFormatter : TextOutputFormatter
{
    public PlainTextTableOutputFormatter()
    {
        SupportedMediaTypes.Add("text/primitives-table");
        SupportedEncodings.Add(Encoding.UTF8);
    }

    protected override bool CanWriteType(Type type)
    {
        if (type == null) return false;

        return typeof(IEnumerable<object>).IsAssignableFrom(type)
            || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>));
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var writer = new StreamWriter(response.Body, selectedEncoding);

        var items = ((IEnumerable<object>)context.Object).ToList();
        if (!items.Any()) return;

        var type = items[0].GetType();
        var props = type.GetProperties();

        // Calculate column widths
        var headers = props.Select(p => p.Name).ToArray();
        var colWidths = headers.Select((h, i) =>
            Math.Max(h.Length, items.Max(item => FormatValue(props[i].GetValue(item)).Length))
        ).ToArray();

        // Write header
        await writer.WriteLineAsync(FormatRow(headers, colWidths));
        await writer.WriteLineAsync(FormatSeparator(colWidths));

        // Write rows
        foreach (var item in items)
        {
            var values = props.Select(p => FormatValue(p.GetValue(item))).ToArray();
            await writer.WriteLineAsync(FormatRow(values, colWidths));
        }

        await writer.FlushAsync();
    }

    private string FormatRow(string[] values, int[] widths)
    {
        var cells = values.Select((val, idx) => val.PadRight(widths[idx]));
        return "| " + string.Join(" | ", cells) + " |";
    }

    private string FormatSeparator(int[] widths)
    {
        var bars = widths.Select(w => new string('-', w));
        return "|-" + string.Join("-|-", bars) + "-|";
    }

    private string FormatValue(object? val)
    {
        return val?.ToString() ?? "";
    }
}