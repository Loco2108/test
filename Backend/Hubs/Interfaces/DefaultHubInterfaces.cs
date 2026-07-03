using Backend.Dto;
using TypedSignalR.Client;

namespace Backend.Hubs.Interfaces;

[Hub]
public interface ISessionHub
{
    Task<RestoreStateDto?> JoinSession(JoinSessionDto data);
    Task LeaveRoom(string roomId);
    Task<bool> UpdateParticipantData(ParticipantUpdateDto data);
}

[Receiver]
public interface ISessionHubClient
{
    Task ParticipantJoined(ParticipantDto data);
    Task ParticipantUpdated(ParticipantUpdateResponseDto data);
}