using BroncoSettingsParser;
using BroncoSettingsParser.Comments;
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

    /* The first setting */
    I am value!

<<<End:Setting>>> /*
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
}