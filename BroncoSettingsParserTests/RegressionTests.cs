using BroncoSettingsParser;
using BroncoSettingsParser.ResponseModel;

namespace BroncoSettingsParserTests;

[TestClass]
public class RegressionTests
{
    [DataTestMethod]
    [DataRow("\n")]
    [DataRow("\r\n")]
    [DataRow("\r")]
    public void SupportsLineEndingsAndCaseInsensitiveNames(string newline)
    {
        var result = new Parser(string.Join(newline,
            "<<<begin:setting:MyName>>>", "Hello", "<<<END:SETTING>>>" )).Parse();
        Assert.AreEqual(Status.Success, result.Status);
        Assert.IsTrue(result.Settings.HasSetting("MYNAME"));
        Assert.AreEqual("Hello", result.Settings.GetSetting("myname")!.Value);
        Assert.AreEqual("Hello", result.Settings.TryGetValue("myname"));
        Assert.AreEqual("Hello", result.TryGetValue<string>("myname"));
    }

    [TestMethod]
    public void IgnoresMultilineCommentsInsideAndOutsideSettings()
    {
        const string source = """
            /*
            <<<Begin:Setting:Ignored>>>
            <<<End:Setting>>>
            */
            <<<Begin:Setting:Value>>>
            before /* hidden
            <<<End:Setting>>>
            still hidden */ middle /* another
            hidden */ after
            <<<End:Setting>>>
            """;
        var result = new Parser(source).Parse();
        Assert.AreEqual(Status.Success, result.Status);
        Assert.AreEqual(1, result.Settings.Count);
        Assert.AreEqual("before middle after", result.Settings.GetValue("Value"));
    }

    [DataTestMethod]
    [DataRow("<<<Begin:Setting:MissingEnd>>>\nvalue")]
    [DataRow("/* missing end")]
    [DataRow("<<<Begin:Setting:A>>>\n1\n<<<End:Setting>>>\n<<<Begin:Setting:a>>>\n2\n<<<End:Setting>>>")]
    public void RejectsIncompleteOrDuplicateSettings(string source)
    {
        Assert.AreEqual(Status.Failed, new Parser(source).Parse().Status);
    }

    [TestMethod]
    public void MappingUpdatesStructAndSkipsNonPublicOrNonInstanceProperties()
    {
        var result = new Parser("<<<Begin:Setting:Value>>>\nhello\n<<<End:Setting>>>").Parse();
        Assert.AreEqual("hello", result.Map<StructSettings>().Value);
        var mapped = result.Map<ClassSettings>();
        Assert.AreEqual("hello", mapped.Value);
        Assert.AreEqual("unchanged", mapped.PrivateSetter);
        Assert.AreEqual("unchanged", ClassSettings.Global);
    }

    [TestMethod]
    public void TypedLookupSupportsCustomParsersAndInvalidValues()
    {
        var result = new Parser("<<<Begin:Setting:Value>>>\n42\n<<<End:Setting>>>").Parse();
        Assert.AreEqual(0, result.TryGetValue<int>("Value"));
        result.SetValueParser(new IntParser());
        Assert.AreEqual(42, result.TryGetValue<int>("Value"));
        Assert.AreEqual(0, result.TryGetValue<int>("Missing"));
        Assert.IsNull(result.TryGetValue<string>("Missing"));
    }

    public struct StructSettings
    {
        public string Value { get; set; }
    }

    public class ClassSettings
    {
        public string Value { get; set; } = "";
        public string PrivateSetter { get; private set; } = "unchanged";
        public static string Global { get; set; } = "unchanged";
        public string this[int index] { get => ""; set { } }
    }
}
