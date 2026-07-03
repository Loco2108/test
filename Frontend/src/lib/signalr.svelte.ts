import * as signalR from '@microsoft/signalr';
import type {
	ISessionHub,
	ISessionHubClient,
} from './wsClient/TypedSignalR.Client/Backend.Hubs.Interfaces';
import type { JoinSessionDto, RestoreStateDto } from './wsClient/Backend.Dto';
import { getHubProxyFactory, getReceiverRegister } from './wsClient/TypedSignalR.Client';

export class SessionConnection {
	private readonly baseUrl = import.meta.env.VITE_API_URL || 'http://localhost:5202';
	private readonly hubUrl = `${this.baseUrl}/defaulthub`;

	private currentRoomCode: string | null = null;
	private readonly subscription: { dispose: () => void };

	readonly connection: signalR.HubConnection;
	readonly sessionHub: ISessionHub;

	connected = $state(false);
	state = $state<RestoreStateDto | null>(null);

	constructor() {
		this.connection = new signalR.HubConnectionBuilder()
			.withUrl(this.hubUrl, { withCredentials: true })
			.withAutomaticReconnect()
			.build();

		this.sessionHub = getHubProxyFactory('ISessionHub').createHubProxy(this.connection);

		const receiver: ISessionHubClient = {
			participantJoined: async (data) => {
				this.state?.participants.push(data);
			},
		};

		this.subscription = getReceiverRegister('ISessionHubClient').register(
			this.connection,
			receiver
		);
	}

	async init() {
		if (this.connected) return;

		await this.connection.start();
		this.connected = true;
	}

	async joinSession(roomCode: string): Promise<boolean> {
		if (!this.connected) {
			await this.init();
		}

		const data: JoinSessionDto = {
			roomCode,
			playerId: localStorage.getItem('anonymousUserId') ?? undefined,
		};

		const result = await this.sessionHub.joinSession(data);

		if (!result) return false;

		this.currentRoomCode = roomCode;
		this.state = result;

		if (result.userInformation) {
			localStorage.setItem('anonymousUserId', result.userInformation.id);
		}

		return true;
	}

	async leaveSession() {
		if (this.currentRoomCode && this.connected) {
			await this.sessionHub.leaveRoom(this.currentRoomCode);
			this.currentRoomCode = null;
			this.state = null;
		}
	}

	destroy() {
		this.subscription.dispose();
		this.connection.stop();
		this.connected = false;
	}
}
