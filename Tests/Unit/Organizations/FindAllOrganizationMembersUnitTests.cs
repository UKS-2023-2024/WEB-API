using Application.Organizations.Queries.FindOrganizationMembers;
using Application.Repositories.Queries.FindAllUsersThatStarredRepository;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Organizations;

public class FindAllOrganizationMembersUnitTests
{
    private readonly Mock<IOrganizationMemberRepository> _organizationMemberRepositoryMock = new ();
    private readonly User _user1;
    private readonly User _user2;
    private readonly User _user3;
    private readonly Organization _organization;

    public FindAllOrganizationMembersUnitTests()
    {
        _user1 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "dusanjanosevic007@gmail.com", "full name", "username1", "password", UserRole.USER);
        _user2 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "dusan.janosevic123@gmail.com", "full name", "username2", "password", UserRole.USER);
        _user3 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92b9"), "dusan.janosevicehh@gmail.com", "full name", "username2", "password", UserRole.USER);
        
        _organization = Organization.Create(new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"),"orgName", "dusanjanosevic007@gmail.com",
            new List<User>());

        var orgMember1 = OrganizationMember.Create(_user2.Id, _organization.Id, OrganizationRole.Create("some", "some"));
        var orgMember2 = OrganizationMember.Create(_user1.Id, _organization.Id, OrganizationRole.Create("some", "some"));
        _organization.AddMember(orgMember1);
        _organization.AddMember(orgMember2);

        _organizationMemberRepositoryMock.Setup(x => x.FindByUserIdAndOrganizationId(_user1.Id, _organization.Id))
            .ReturnsAsync(orgMember1);
        _organizationMemberRepositoryMock.Setup(x => x.FindByUserIdAndOrganizationId(_user2.Id, _organization.Id))
            .ReturnsAsync(orgMember2);
        _organizationMemberRepositoryMock.Setup(x => x.FindOrganizationMembers(_organization.Id))
            .ReturnsAsync(_organization.Members);
    }
    
    [Fact]
    public void Handle_ShouldReturn2_WhenRepositoryPublic()
    {
        //Arrange
        var command = new FindOrganizationMembersQuery(_user1.Id,
            _organization.Id);

        //Act
        var result = new FindOrganizationMembersQueryHandler(_organizationMemberRepositoryMock.Object)
            .Handle(command,default);

        //Assert
        result.Result.Count().ShouldBe(2);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenOrganizationMemberNotFound()
    {
        //Arrange
        var command = new FindOrganizationMembersQuery(_user3.Id,
            _organization.Id);
        
        //Act
        var handler = new FindOrganizationMembersQueryHandler(_organizationMemberRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<CantAccessOrganizationMembers>(Handle);
    }
}