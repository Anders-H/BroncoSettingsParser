using BroncoSettingsParser;
using BroncoSettingsParser.ResponseModel;

namespace BroncoSettingsParserTests;

[TestClass]
public class ParserTests
{
    [TestMethod]
    public void DetectIncorrectBroncoFiles()
    {
        const string source1 = @"<<<Begin:Setting:MySetting)>>>
I am value
<<<End:Setting)>>> no";
        const string source2 = @"no <<<Begin:Setting:MySetting)>>>
I am value
<<<End:Setting)>>>";
        const string source3 = @"<<<Begin:Setting:MySetting)>>>
I am value
no <<<End:Setting)>>>";
        const string source4 = @"<<<Begin:Setting:MySetting)>>> no
I am value
<<<End:Setting)>>>";

        const string source5 = @"    /* yes */     <<<Begin:Setting:MySetting)>>>        /* yes */         
I am value
                         /* yes */           <<<End:Setting)>>>          /* yes */                 ";

        var parser = new Parser(source1);
        var response = parser.Parse();
        Assert.AreEqual(Status.Failed, response.Status);
        Assert.AreEqual("Data after closing tag is not allowed.", response.Message);

        parser = new Parser(source2);
        response = parser.Parse();
        Assert.AreEqual(Status.Failed, response.Status);
        Assert.AreEqual("Data before opening tag is not allowed.", response.Message);

        parser = new Parser(source3);
        response = parser.Parse();
        Assert.AreEqual(Status.Failed, response.Status);
        Assert.AreEqual("Data before closing tag is not allowed.", response.Message);

        parser = new Parser(source4);
        response = parser.Parse();
        Assert.AreEqual(Status.Failed, response.Status);
        Assert.AreEqual("Data after opening tag is not allowed.", response.Message);

        parser = new Parser(source5);
        response = parser.Parse();
        Assert.AreEqual(Status.Success, response.Status);
        Assert.AreEqual("", response.Message);
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