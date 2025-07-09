namespace SetlistPlaylistCreator.Wpf.UnitTests
{
    [TestClass]
    public sealed class CommandLineArgumentParserTests
    {
        [TestMethod]
        public void Parse_CorrectlyParsesToken()
        {
            var argument = "medkspc://tokenredirect/?code=JHKUJHKJ--JU(&JJI7987u4rwejlkjl";
            var parser = new CommandLineArgumentsParser();

            var result = parser.Parse(["SetlistPlaylistCreator.Wpf.exe", argument]);

            Assert.AreEqual("JHKUJHKJ--JU(&JJI7987u4rwejlkjl", result.Token);
        }
    }
}
