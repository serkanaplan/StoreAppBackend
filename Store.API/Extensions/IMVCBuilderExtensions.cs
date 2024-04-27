
using Store.API.Formatters;

namespace Store.API.Extensions;

public static class IMVCBuilderExtensions
{
    public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder builder) =>
           builder.AddMvcOptions(config =>
               config.OutputFormatters
           .Add(new CSVOutputFormatter()));
}
