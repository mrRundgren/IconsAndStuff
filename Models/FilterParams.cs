using Microsoft.JSInterop;
using BlazorMaterialSymbols;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace IconsAndStuff.Models;

public sealed class FilterParams : FilterParamsData
{
    private readonly IJSRuntime JSRuntime;
    private readonly Dictionary<string, string> _all = new();
    [JsonIgnore]
    public EventHandler<KeyValuePair<string, string>>? OnCopyIconToClipboard { get; set; }
    [JsonIgnore]
    public EventHandler? OnCopyDefaultsToClipboard { get; set; }
    [JsonIgnore]
    public string Name { get; set; } = "";
    [JsonIgnore]
    public string? SelectedIcon { get; set; }
    [JsonIgnore]
    public Dictionary<string, string> Data { get => _all.Where(_ => _.Key.ToLower().Contains(Name.Replace(" ", "").ToLower())).ToDictionary(_ => _.Key, _ => _.Value); }
    [JsonIgnore]
    public string Title
    {
        get
        {
            if (Data.Count == 0)
            {
                return $"No result found for \"{Name}\"";
            }
            else if (Data.Count < _all.Count)
            {
                return $"Showing results for \"{Name}\"";
            }
            else
            {
                return $"Showing all {_all.Count} symbols";
            }
        }
    }
    [JsonIgnore]
    public string SubTitle
    {
        get
        {
            if (Data.Count == 0)
            {
                return $"Try again with a different spelling or keyword";
            }
            else if (Data.Count < _all.Count)
            {
                return $"Found {Data.Count} symbols out of {_all.Count}";
            }
            else
            {
                if(FontDownload)
                {
                    return "Click a symbol to download it as SVG";
                }
                return "Click a symbol to copy it to clipboard";
            }
        }
    }
    public FilterParams(IJSRuntime jsRuntime)
    {
        JSRuntime = jsRuntime;
        _all = Icons.All().Where(_ => _.Key.ToLower().Contains(Name.ToLower())).ToDictionary(_ => _.Key, _ => _.Value);
    }
    public FilterParams(IJSRuntime jsRuntime, FilterParamsData data) : this(jsRuntime)
    {
        DefaultFill = data.DefaultFill;
        DefaultGrade = data.DefaultGrade;
        DefaultSize = data.DefaultSize;
        DefaultType  = data.DefaultType;
        DefaultWeight = data.DefaultWeight;
        
        Fill = data.Fill;
        Grade = data.Grade;
        Size = data.Size;
        Type  = data.Type;
        Weight = data.Weight;
        FontDownload = data.FontDownload;
        DarkMode = data.DarkMode;
        UseAsStandard = data.UseAsStandard;
    }

    public string IconClipboardText(KeyValuePair<string, string> item) {
        var name = $"Name=\"@Icons.{item.Key}\"";

        if (UseAsStandard)
        {
            return $"<Icon {name} />";
        }
        else
        {
            var type = Type != DefaultType ? $" Type=\"@IconType.{Type}\"" : "";
            var fill = Fill != DefaultFill ? $" Fill=\"@IconFill.{Fill}\"" : "";
            var weight = Weight != DefaultWeight ? $" Weight=\"@IconWeight.{Weight}\"" : "";
            var grade = Grade != DefaultGrade ? $" Grade=\"@IconGrade.{Grade}\"" : "";
            var size = Size != DefaultSize ? $" Size=\"@IconSize.{Size}\"" : "";
            return $"<Icon {name}{type}{fill}{weight}{grade}{size} />";
        }
    }
    public string DefaultsClipboardText()
    {
        var type = Type != DefaultType ? $" Type=\"@IconType.{Type}\"" : "";
        var fill = Fill != DefaultFill ? $" Fill=\"@IconFill.{Fill}\"" : "";
        var weight = Weight != DefaultWeight ? $" Weight=\"@IconWeight.{Weight}\"" : "";
        var grade = Grade != DefaultGrade ? $" Grade=\"@IconGrade.{Grade}\"" : "";
        var size = Size != DefaultSize ? $" Size=\"@IconSize.{Size}\"" : "";
        return $"<IconManager {type}{fill}{weight}{grade}{size} />";
    }

    public async Task CopyDefaultsToClipboard()
    {
        var clipboardText = DefaultsClipboardText();
        OnCopyDefaultsToClipboard?.Invoke(this, EventArgs.Empty);
        await JSRuntime.InvokeVoidAsync("clipboardCopy.copyText", clipboardText);
    }

    public async Task CopyIconToClipboard(KeyValuePair<string, string> item)
    {
        var clipboardText = IconClipboardText(item);
        OnCopyIconToClipboard?.Invoke(this, item);
        await JSRuntime.InvokeVoidAsync("clipboardCopy.copyText", clipboardText);
    }
}