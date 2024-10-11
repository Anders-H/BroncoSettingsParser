namespace BroncoSettingsParserTests;

[TestClass]
public class ParserTests
{
    [TestMethod]
    public void DetectIncorrectBroncoFiles()
    {

    }

    [TestMethod]
    public void CanParseBasicBroncoFile1()
    {
        const string source = @"<<<Begin:Setting:MySetting)>>>
I am value
<<<End:Setting)>>>";

    }

    [TestMethod]
    public void CanParseBasicBroncoFile2()
    {
        const string source = @"<<<Begin:Setting:MySetting)>>>
/* Here comes the value: */
I /* Hello */ am /* World! */ value
/* That's it */
<<<End:Setting)>>>";

    }

    [TestMethod]
    public void CanParseBasicBroncoFile3()
    {
        const string source = @"<<<Begin:Setting:Setting 1)>>>

    /* The first setting */
    I am value!

<<<End:Setting)>>>
<<<Begin:Setting:The Second Setting)>>>

    /* The 2:nd setting */
    I am also
    value.

<<<End:Setting)>>>";

    }
}