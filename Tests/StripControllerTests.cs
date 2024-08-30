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
}
