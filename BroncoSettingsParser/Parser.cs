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
        var rows = _source.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
    }
}