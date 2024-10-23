using BroncoSettingsParser.ResponseModel;

namespace BroncoSettingsParser;

public class Parser
{
    private string _source;
    private List<string> _rows;

    public Parser() : this("")
    {
    }

    public Parser(string source)
    {
        _source = source;
    }

    public ParseResult Parse()
    {
        _rows = _source.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries).ToList();
        var status = Status.Failed;
        var message = "";
        var settings = new SettingCollection();
        Setting? currentSetting = null;

        foreach (var row in _rows)
        {
            
        }

        return new ParseResult(status, message, settings);
    }
}