/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import type { ParticipantRole } from "./participant-role";
import type { GameState } from "./game-state";
import type { AnonymousUserDto } from "./anonymous-user-dto";

export interface RestoreStateDto {
    sessionName: string;
    sessionDescription?: string;
    role: ParticipantRole;
    gameState: GameState;
    userInformation?: AnonymousUserDto;
}
