namespace Backend.Hubs;

public interface IPollClient
{
    Task NewPollCreated(Poll newPoll);
}