using Application.Milestones.Queries.FindCompletionPercentageOfMilestone;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Milestones;

[Collection("Sequential")]
public class FindMilestoneCompletionPercentageIntegrationTest :BaseIntegrationTest
{
    public FindMilestoneCompletionPercentageIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    
    [Fact]
    public async void Handle_ShouldReturn0()
    {
        //Arrange
        var command = new FindCompletionPercentageOfMilestoneQuery(new Guid("9e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"));

        //Act
        async Task<double> Handle() => await _sender.Send(command);
        

        //Assert
        var res = await Handle();
        res.ShouldBe((double)0);
    }

    [Fact]
    public async void Handle_ShouldReturn50()
    {
        //Arrange
        var command = new FindCompletionPercentageOfMilestoneQuery(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"));

        //Act
        async Task<double> Handle() => await _sender.Send(command);


        //Assert
        var res = await Handle();
        res.ShouldBe((double)50);
    }


}