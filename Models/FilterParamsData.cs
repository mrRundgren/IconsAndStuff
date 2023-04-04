using BlazorMaterialSymbols;

namespace IconsAndStuff.Models;

public class FilterParamsData
{
    public bool DarkMode { get; set; }
    public bool FontDownload { get; set; }
    public bool UseAsStandard { get; set; } = true;
    public IconType DefaultType { get; set; } = IconType.Outlined;
    public IconFill DefaultFill { get; set; } = IconFill.None;
    public IconWeight DefaultWeight { get; set; } = IconWeight.Weight400;
    public IconGrade DefaultGrade { get; set; } = IconGrade.Medium;
    public IconSize DefaultSize { get; set; } = IconSize.Normal;
    public IconType Type { get; set; } = IconType.Outlined;
    public IconFill Fill { get; set; } = IconFill.None;
    public IconWeight Weight { get; set; } = IconWeight.Weight400;
    public IconGrade Grade { get; set; } = IconGrade.Medium;
    public IconSize Size { get; set; } = IconSize.Normal;
}
