using MaxRumsey.OzStripsPlugin.Gui;
using Moq;
using Pose;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.Tests;

/// <summary>
/// Tests the StripController class.
/// </summary>
[TestClass]
public class StripControllerTests
{
    private readonly Mock<ISocketConn> _socketConn = new();
    private readonly Mock<IBayManager> _bayManager = new();

    [TestMethod]
    public void TestArrDepTypeArrival()
    {
        var fdr = new FDP2.FDR();
        fdr.DesAirport = "YMML";

        _bayManager.Setup(x => x.AerodromeName).Returns("YMML");

        var sut = new StripController(fdr, _bayManager.Object, _socketConn.Object);
        var type = sut.ArrDepType;

        Assert.AreEqual(StripArrDepType.ARRIVAL, type);
    }

    [TestMethod]
    public void TestArrDepTypeDeparture()
    {
        var fdr = new FDP2.FDR();
        fdr.DepAirport = "YMML";

        _bayManager.Setup(x => x.AerodromeName).Returns("YMML");

        var sut = new StripController(fdr, _bayManager.Object, _socketConn.Object);
        var type = sut.ArrDepType;

        Assert.AreEqual(StripArrDepType.DEPARTURE, type);
    }

    [TestMethod]
    public void TestDefaultBayAssignment()
    {
        var fdr = new FDP2.FDR();
        fdr.DepAirport = "YSSY";
        fdr.DesAirport = "YMML";

        _bayManager.Setup(x => x.AerodromeName).Returns("YMML");

        var sut = new StripController(fdr, _bayManager.Object, _socketConn.Object);
        var bay = sut.CurrentBay;

        Assert.AreEqual(StripBay.BAY_ARRIVAL, bay);
    }

    [TestMethod]
    public void TestHeadingParsing()
    {
        var fdr = new FDP2.FDR();
        fdr.GlobalOpData = "H180";

        var sut = new StripController(fdr, _bayManager.Object, _socketConn.Object);

        var heading = sut.HDG;

        Assert.AreEqual("180", heading);
    }

    [TestMethod]
    public void TestFirstRouteWpt()
    {
        var fdr = new FDP2.FDR();
        fdr.RouteNoParse = "DOSEL1/27 DCT MANDA";

        var sut = new StripController(fdr, _bayManager.Object, _socketConn.Object);

        var firstwpt = sut.FirstWpt;

        Assert.AreEqual("MANDA", firstwpt);
    }
}
