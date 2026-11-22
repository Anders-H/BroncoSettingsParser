using System.Text;
using System.Text.RegularExpressions;
using BroncoSettingsParser.ResponseModel;

namespace BroncoSettingsParser;

public class Parser
{
    private readonly string _source;
    
    public Parser(FileInfo sourceFile)
    {
        _source = File.ReadAllText(sourceFile.FullName);
    }

    public Parser(string source)
    {
        ArgumentNullException.ThrowIfNull(source);
        _source = source;
    }

    public static Parser LoadSettingsFromApplicationDirectory(string filename)
    {
        return new Parser(new FileInfo(Path.Combine(Tools.ExeFolder.FullName, filename)));
    }

    public ParseResult Parse()
    {
        var source = RemoveComments(_source, out var unterminatedComment);
        if (unterminatedComment)
            return new ParseResult(Status.Failed, "Unterminated comment.", new SettingCollection());

        using var reader = new StringReader(source);
        var settings = new SettingCollection();
        Setting? currentSetting = null;
        var currentValue = new StringBuilder();
        var trimRegex = new Regex(@"\s+");

        while (reader.ReadLine() is { } row)
        {
            var trimmed = row.Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
                continue;

            trimmed = trimRegex.Replace(trimmed, " ").Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
                continue;

            if (currentSetting == null)
            {
                var beginSetting = Regex.Match(trimmed, "^<<<Begin:Setting:(.*)>>>$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

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
                if (string.Equals(trimmed, "<<<End:Setting>>>", StringComparison.OrdinalIgnoreCase))
                {
                    var value = trimRegex.Replace(currentValue.ToString(), " ").Trim();
                    currentSetting.Value = value;
                    if (!settings.Set(currentSetting))
                        return new ParseResult(Status.Failed, $"Duplicate setting: {currentSetting.Name}", settings);
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

        if (currentSetting != null)
            return new ParseResult(Status.Failed, $"Missing closing tag for setting: {currentSetting.Name}", settings);

        var status = settings.Count > 0 ? Status.Success : Status.NoData;
        return new ParseResult(status, "Ok.", settings);
    }

    private static string RemoveComments(string source, out bool unterminatedComment)
    {
        var result = new StringBuilder(source.Length);
        var inComment = false;

        for (var i = 0; i < source.Length; i++)
        {
            if (!inComment && source[i] == '/' && i + 1 < source.Length && source[i + 1] == '*')
            {
                inComment = true;
                i++;
            }
            else if (inComment && source[i] == '*' && i + 1 < source.Length && source[i + 1] == '/')
            {
                inComment = false;
                i++;
            }
            else if (!inComment || source[i] is '\r' or '\n')
            {
                result.Append(source[i]);
            }
        }

        unterminatedComment = inComment;
        return result.ToString();
    }
}
