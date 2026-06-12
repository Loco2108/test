import * as signalR from "@microsoft/signalr";
import type { Poll } from "./api";

export class PollConnection {
    connection: signalR.HubConnection;

    constructor(url: string, onupdatereceived?: (res: Poll) => void) {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(url)
            .withAutomaticReconnect()
            .build();

        this.connection.on("NewPollCreated", (poll: Poll) => {
            onupdatereceived?.(poll);
        });
    }

    async start(pollId: string) {
        await this.connection.start();
        await this.connection.invoke("JoinRoom", pollId);
    }
}