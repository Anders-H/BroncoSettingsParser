using System.Drawing;
using BroncoSettingsParser;
using BroncoSettingsParser.Comments;
using BroncoSettingsParser.Exceptions;
using BroncoSettingsParser.ResponseModel;

namespace BroncoSettingsParserTests;

[TestClass]
public class ParserTests
{
    [TestMethod]
    public void DetectIncorrectBroncoFiles()
    {
        const string source1 = @"<<<Begin:Setting:MySetting>>>
I am value
<<<End:Setting>>> no";
        const string source2 = @"no <<<Begin:Setting:MySetting>>>
I am value
<<<End:Setting>>>";
        const string source3 = @"<<<Begin:Setting:MySetting>>>
I am value
no <<<End:Setting>>>";
        const string source4 = @"<<<Begin:Setting:MySetting>>> no
I am value
<<<End:Setting>>>";

        const string source5 = @"    /* yes */     <<<Begin:Setting:MySetting>>>        /* yes */         
I am value
                         /* yes */           <<<End:Setting>>>          /* yes */                 ";

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
        Assert.AreEqual("Ok.", response.Message);
        Assert.AreEqual("I am value", response.Settings.GetValue("MySetting"));
    }

    [TestMethod]
    public void CanParseBasicBroncoFile1()
    {
        const string source = @"<<<Begin:Setting:MySetting>>>
I am value
<<<End:Setting>>>";

        var parser = new Parser(source);
        var response = parser.Parse();
        Assert.AreEqual(Status.Success, response.Status);
        Assert.AreEqual("I am value", response.Settings.GetValue("MySetting"));
    }

    [TestMethod]
    public void CanParseBasicBroncoFile2()
    {
        const string source = @"<<<Begin:Setting:MySetting>>>
/* Here comes the value: */
I /* Hello */ am /* World! */ value
/* That's it */
<<<End:Setting>>>";

        var parser = new Parser(source);
        var response = parser.Parse();
        Assert.AreEqual(Status.Success, response.Status);
        Assert.AreEqual("I am value", response.Settings.GetValue("MySetting"));
    }

    [TestMethod]
    public void CanParseBasicBroncoFile3()
    {
        const string source = @"<<<Begin:Setting:Setting 1>>>

    /* The first setting */
    I am value!

<<<End:Setting>>>
<<<Begin:Setting:The Second Setting>>>

    /* The 2:nd setting */
    I am also
    value.

<<<End:Setting>>>";

        var parser = new Parser(source);
        var response = parser.Parse();
        Assert.AreEqual(Status.Success, response.Status);
        Assert.AreEqual("I am value!", response.Settings.GetValue("Setting 1"));
        Assert.AreEqual("I am also value.", response.Settings.GetValue("The Second Setting"));
    }

    [TestMethod]
    public void CanRemoveComments()
    {
        Assert.AreEqual("ABC", new Remover("ABC").Remove(out _));
        Assert.AreEqual("ABC", new Remover("A/*a*/B/*a*/C").Remove(out _));
        Assert.AreEqual("AB", new Remover("AB/*CD").Remove(out _));
        Assert.AreEqual("CD", new Remover("AB*/CD").Remove(out _));
        Assert.AreEqual("CDGH", new Remover("AB*/CD/*EF*/GH/*IJ").Remove(out _));
    }

    [TestMethod]
    public void IdentifyCommentScope()
    {
        new Remover("/* Hello */").Remove(out var commentScope);
        Assert.IsFalse(commentScope);
        new Remover("Hello /* yes").Remove(out commentScope);
        Assert.IsTrue(commentScope);
        new Remover("yes */ Hello").Remove(out commentScope);
        Assert.IsFalse(commentScope);
    }

    [TestMethod]
    public void UseCommentScope()
    {
        const string source = @"/*<<<Begin:Setting:Setting 1>>>

    The first setting */
    I am value!
                      /*
<<<End:Setting>>>  */
<<<Begin:Setting:The Second Setting>>>

    /* The 2:nd setting */
    I am also
    value.

<<<End:Setting>>>";

        var parser = new Parser(source);
        var response = parser.Parse();
        Assert.AreEqual(Status.Success, response.Status);
        Assert.AreEqual(1, response.Settings.Count);
        Assert.AreEqual("I am also value.", response.Settings.GetValue("The Second Setting"));
    }

    [TestMethod]
    public void CommentScopeVariation()
    {
        const string source = @"<<<Begin:Setting:Setting>>>

I am /* a
very nice and
fine */ value!

<<<End:Setting>>>";

        var parser = new Parser(source);
        var response = parser.Parse();
        Assert.AreEqual(Status.Success, response.Status);
        Assert.AreEqual(1, response.Settings.Count);
        Assert.AreEqual("I am value!", response.Settings.GetValue("Setting"));
    }

    [TestMethod]
    public void CanRemoveCommentsFromTags()
    {
        const string source = @"    /* yes */     <<<Begin:Setting:MySetting>>>        /* yes */         
I am value
                         /* yes */           <<<End:Setting>>>          /* yes */                 ";
        var parser = new Parser(source);
        var response = parser.Parse();
        Assert.AreEqual(Status.Success, response.Status);
        Assert.AreEqual("Ok.", response.Message);
    }

    [TestMethod]
    public void CanHandleEmptyValues()
    {
        var parser = new Parser(@"
<<<Begin:Setting:MySetting>>>
<<<End:Setting>>>
");
        var response = parser.Parse();
        Assert.AreEqual(Status.Success, response.Status);
        Assert.AreEqual("", response.Settings.GetValue("MySetting"));
        parser = new Parser(@"
<<<Begin:Setting:MySetting>>><<<End:Setting>>>
");
        response = parser.Parse();
        Assert.AreEqual(Status.Failed, response.Status);
    }

    [TestMethod]
    public void BasicMapping()
    {
        var parser = new Parser(@"
<<<Begin:Setting:StringValue1>>>
    Hello 1
<<<End:Setting>>>

<<<Begin:Setting:StringValue2>>>
    Hello 2
<<<End:Setting>>>");
        var response = parser.Parse();
        var typedResponse = response.Map<BasicMappingTestClass>();
        Assert.AreEqual("Hello 1", typedResponse.StringValue1);
        Assert.AreEqual("Hello 2", typedResponse.StringValue2);
    }

    [TestMethod]
    public void MappingFailsIfPropertyIsNotFound()
    {
        var parser = new Parser(@"
<<<Begin:Setting:StringValue1>>>
    Hello 1
<<<End:Setting>>>

<<<Begin:Setting:NotFoundHere>>>
    Hello 2
<<<End:Setting>>>)");
        var response = parser.Parse();
        Assert.ThrowsException<PropertyMissingException>(() => response.Map<BasicMappingTestClass>());
    }

    [TestMethod]
    public void MappingHasDatatypeSupport()
    {
        var parser = new Parser(@"
<<<Begin:Setting:MyStringSetting>>>
    Hello!
<<<End:Setting>>>
<<<Begin:Setting:MyIntSetting>>>
    56
<<<End:Setting>>>
<<<Begin:Setting:MyPointSetting>>>
    45,81
<<<End:Setting>>>");
    }
}

public class BasicMappingTestClass
{
    public string StringValue1 { get; set; }
    public string StringValue2 { get; set; }

    public BasicMappingTestClass() : this("", "")
    {
    }

    public BasicMappingTestClass(string stringValue1, string stringValue2)
    {
        StringValue1 = stringValue1;
        StringValue2 = stringValue2;
    }
}

public class DataTypeSupport
{
    public string MyStringSetting { get; set; }
    public int MyIntSetting { get; set; }
    public Point MyPointSetting { get; set; }

    public DataTypeSupport() : this("", 0, Point.Empty)
    {
    }

    public DataTypeSupport(string myStringSetting, int myIntSetting, Point myPointSetting)
    {
        MyStringSetting = myStringSetting;
        MyIntSetting = myIntSetting;
        MyPointSetting = myPointSetting;
    }
}