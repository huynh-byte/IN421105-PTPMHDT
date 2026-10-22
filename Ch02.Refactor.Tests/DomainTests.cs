
using EduHub.Ch02;
using EduHub.Ch02.Domain;

namespace Ch02.Refactor.Tests;

// Mẫu AAA: Arrange - Act - Assert.
public class DomainTests
{
    [Fact]
    public void CreateClub_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        var manager = new ClubManager();

        // Act
        var act = () => manager.CreateClub("", "mô tả", "p@gmail.com");

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Club_Create_WithValidName_ReturnsClub()
    {
        // Arrange
        string name = "Câu lạc bộ Lập trình";
        string description = "Câu lạc bộ dành cho người yêu lập trình";

        // Act
        var club = Club.Create(name, description);

        // Assert
        Assert.Equal(name, club.Name);
        Assert.Equal(description, club.Description);
        Assert.NotEqual(Guid.Empty, club.Id);
    }

    [Fact]
    public void Club_Create_WithInvalidName_ThrowsArgumentException()
    {
        // Arrange
        string emptyName = "   ";
        string longName = new('A', Club.MaxNameLength + 1);

        // Act
        var emptyNameAct = () => Club.Create(emptyName, "Mô tả");
        var longNameAct = () => Club.Create(longName, "Mô tả");

        // Assert
        Assert.Throws<ArgumentException>(emptyNameAct);
        Assert.Throws<ArgumentException>(longNameAct);
    }

    [Fact]
    public void Club_HasRoom_AtMaxMembers_ReturnsFalse()
    {
        // Arrange
        var club = Club.Create("CLB Lập trình", "Mô tả");

        // Act
        bool hasRoomAt49 = club.HasRoom(49);
        bool hasRoomAt50 = club.HasRoom(50);

        // Assert
        Assert.True(hasRoomAt49);
        Assert.False(hasRoomAt50);
    }

    [Fact]
    public void Event_Register_NewMember_ReturnsTrue()
    {
        // Arrange
        var ev = new Event
        {
            Id = Guid.NewGuid(),
            ClubId = Guid.NewGuid(),
            Title = "Buổi sinh hoạt CLB",
            Date = DateTime.Today
        };
        Guid memberId = Guid.NewGuid();

        // Act
        bool result = ev.Register(memberId);

        // Assert
        Assert.True(result);
        Assert.Contains(memberId, ev.RegisteredMemberIds);
    }

    [Fact]
    public void Event_Register_SameMemberTwice_ReturnsFalse()
    {
        // Arrange
        var ev = new Event
        {
            Id = Guid.NewGuid(),
            ClubId = Guid.NewGuid(),
            Title = "Buổi sinh hoạt CLB",
            Date = DateTime.Today
        };
        Guid memberId = Guid.NewGuid();
        ev.Register(memberId);

        // Act
        bool result = ev.Register(memberId);

        // Assert
        Assert.False(result);
        Assert.Single(ev.RegisteredMemberIds);
    }

    [Fact]
    public void Event_CheckIn_UnregisteredMember_ThrowsInvalidOperationException()
    {
        // Arrange
        var ev = new Event
        {
            Id = Guid.NewGuid(),
            ClubId = Guid.NewGuid(),
            Title = "Buổi sinh hoạt CLB",
            Date = DateTime.Today
        };
        Guid memberId = Guid.NewGuid();

        // Act
        var act = () => ev.CheckIn(memberId);

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }
}
