using Backend.Dto;
using TypedSignalR.Client;

namespace Backend.Hubs.Interfaces;

[Hub]
public interface ISessionHub
{
    Task<RestoreStateDto?> JoinSession(JoinSessionDto data);
    Task LeaveRoom(string roomId);
}

[Receiver]
public interface ISessionHubClient
{
    Task ParticipantJoined(ParticipantDto data);
}