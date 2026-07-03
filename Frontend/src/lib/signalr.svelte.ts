import * as signalR from '@microsoft/signalr';
import type { RestoreStateDto } from './wsTypes/restore-state-dto';
import type { JoinSessionDto } from './wsTypes/join-session-dto';

export class SessionConnection {
	private readonly baseUrl = import.meta.env.VITE_API_URL || 'http://localhost:5202';
	private readonly hubUrl = this.baseUrl + '/defaulthub';
	private currentRoomCode: string | null = null;

	readonly connection: signalR.HubConnection;

	connected = $state(false);
	state = $state<RestoreStateDto | null>(null);

	constructor() {
		this.connection = new signalR.HubConnectionBuilder()
			.withUrl(this.hubUrl, { withCredentials: true })
			.withAutomaticReconnect()
			.build();

		this.connection.onclose(() => (this.connected = false));

		this.connection.onreconnected(async () => {
			this.connected = true;
			if (this.currentRoomCode) await this._join(this.currentRoomCode);
		});
	}

	async init() {
		await this.connection.start();
		this.connected = true;
	}

	async joinSession(roomCode: string): Promise<Boolean> {
		this.currentRoomCode = roomCode;
		return this._join(roomCode);
	}

	private async _join(roomCode: string): Promise<Boolean> {
		const data: JoinSessionDto = {
			roomCode,
			playerId: localStorage.getItem('anonymousUserId') ?? undefined,
		};

		const result: RestoreStateDto | null = await this.connection.invoke('JoinSession', data);
		if (!result) return false;

		this.state = result;
		if (result.userInformation) {
			localStorage.setItem('anonymousUserId', result.userInformation.id);
		}

		return true;
	}
}
