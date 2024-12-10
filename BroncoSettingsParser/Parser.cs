using System.Text;
using System.Text.RegularExpressions;
using BroncoSettingsParser.Comments;
using BroncoSettingsParser.ResponseModel;

namespace BroncoSettingsParser;

public class Parser
{
    private readonly string _source;
    private List<string>? _rows;
    
    public Parser(FileInfo sourceFile)
    {
        _source = File.ReadAllText(sourceFile.FullName);
    }

    public Parser(string source)
    {
        _source = source;
    }

    public static Parser LoadSettingsFromApplicationDirectory(string filename)
    {
        var path = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
        return new Parser(new FileInfo(Path.Combine(path.FullName, filename)));
    }

    public ParseResult Parse()
    {
        _rows = _source.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries).ToList();
        var settings = new SettingCollection();
        Setting? currentSetting = null;
        var currentValue = new StringBuilder();
        var isInCommentScope = false;

        foreach (var row in _rows)
        {
            var trimmed = row.Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
                continue;

            trimmed = new Remover(trimmed).Remove(out var commentScope);

            if (!isInCommentScope && commentScope && currentSetting != null)
            {
                currentValue.Append($"{trimmed} ");
                isInCommentScope = true;
                continue;
            }
            
            if (isInCommentScope && !commentScope && currentSetting != null)
            {
                isInCommentScope = false;

                if (ClosesCommentScope(row, out var closeIndex))
                    trimmed = trimmed.Substring(closeIndex + 2).Trim();
                else
                    continue;
            }

            var trimRegex = new Regex(@"\s+");
            trimmed = trimRegex.Replace(trimmed, " ").Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
                continue;

            if (currentSetting == null)
            {
                var beginSetting = Regex.Match(trimmed, "^<<<Begin:Setting:(.*)>>>$");

                if (trimmed.LastIndexOf("<<<", StringComparison.Ordinal) > trimmed.IndexOf("<<<", StringComparison.Ordinal))
                    return new ParseResult(Status.Failed, "Opening blocks and closing blocks be alone on a row.", new SettingCollection());

                if (beginSetting.Success)
                {
                    var settingName = beginSetting.Groups[1].Value.Trim();

                    if (string.IsNullOrWhiteSpace(settingName))
                        return new ParseResult(Status.Failed, "Missing setting name.", settings);

                    currentSetting = new Setting(settingName, "");
                    currentValue.Clear();
                }
                else
                {
                    beginSetting = Regex.Match(trimmed, "^<<<.*>>>");

                    if (beginSetting.Success)
                        return new ParseResult(Status.Failed, "Data after opening tag is not allowed.", settings);

                    beginSetting = Regex.Match(trimmed, "<<<.*>>>");

                    if (beginSetting.Success)
                        return new ParseResult(Status.Failed, "Data before opening tag is not allowed.", settings);
                }
            }
            else
            {
                if (trimmed == "<<<End:Setting>>>")
                {
                    var value = trimRegex.Replace(currentValue.ToString(), " ").Trim();
                    currentSetting.Value = value;
                    settings.Set(currentSetting);
                    currentSetting = null;
                    currentValue.Clear();
                }
                else
                {
                    var endSetting = Regex.Match(trimmed, "^<<<.*>>>");

                    if (trimmed.LastIndexOf("<<<", StringComparison.Ordinal) > trimmed.IndexOf("<<<", StringComparison.Ordinal))
                        return new ParseResult(Status.Failed, "Opening blocks and closing blocks be alone on a row.", new SettingCollection());

                    if (endSetting.Success)
                        return new ParseResult(Status.Failed, "Data after closing tag is not allowed.", settings);

                    endSetting = Regex.Match(trimmed, "<<<.*>>>");

                    if (endSetting.Success)
                        return new ParseResult(Status.Failed, "Data before closing tag is not allowed.", settings);

                    currentValue.Append($"{trimmed} ");
                }
            }
        }

        var status = settings.Count > 0 ? Status.Success : Status.NoData;
        return new ParseResult(status, "Ok.", settings);
    }

    private bool ClosesCommentScope(string row, out int closeIndex)
    {
        var openIndex = row.IndexOf("/*", StringComparison.Ordinal);
        closeIndex = row.IndexOf("*/", StringComparison.Ordinal);

        return openIndex switch
        {
            >= 0 when closeIndex >= 0 => openIndex > closeIndex,
            >= 0 => false,
            _ => closeIndex >= 0
        };
    }
}