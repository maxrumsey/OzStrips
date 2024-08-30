using System.Runtime.CompilerServices;
using System.Windows.Forms;
using MaxRumsey.OzStripsPlugin.Gui;
using MaxRumsey.OzStripsPlugin.Gui.DTO;
using Moq;
using Pose;
using vatsys;

namespace MaxRumsey.OzStripsPlugin.Tests;

/// <summary>
/// Tests the StripController class.
/// </summary>
[TestClass]
public static class BayManagerTests
{
    /*
    [TestMethod]
    public void TestUpdateOrder()
    {
        var dto = new BayDTO();
        dto.bay = StripBay.BAY_TAXI;
        dto.list = new List<string>
        {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
        };

        var expectedList = new List<StripListItem>();
        dto.list.ForEach(x =>
        {
            var stripController = new Mock<IStripController>();
            stripController.Setup(x => x.Remark).Returns(x);
            expectedList.Add(
                new StripListItem()
                {
                    Type = Gui.StripItemType.STRIP,
                    StripController = stripController.Object,
                });
            });

        var sut = CreateBayManagerClass();

        var taxibay = new Bay(new List<StripBay> { StripBay.BAY_TAXI }, );
        taxibay.SetupProperty(x => x.Strips, new List<StripListItem>());
        taxibay.Setup(x => x.GetListItemByStr(It.IsAny<string>())).Returns((string s) => expectedList.Find(x => x.StripController?.Remark == s));

        sut.AddBay(taxibay.Object, 0);

        sut.UpdateOrder(dto);

        Assert.AreEqual(expectedList, sut.Bays[0].Strips);
    }
    */
    private static void DummyAction(object sender, EventArgs e)
    {
    }

    private static BayManager CreateBayManagerClass()
    {
        var flp = new FlowLayoutPanel();
        var sut = new BayManager(flp, DummyAction);
        return sut;
    }

    private static Shim CreateSCListShim(List<StripController> list)
    {
        return Shim.Replace(() => StripController.StripControllers).With(() => list);
    }
}
