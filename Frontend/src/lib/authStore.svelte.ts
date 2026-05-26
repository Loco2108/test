type AuthData = {
    isLoggedIn: boolean;
    username: string | null;
    profilePictureUrl: string | null;
}

export const authStore = $state<AuthData>({
    isLoggedIn: false,
    username: null,
    profilePictureUrl: null,
});

export function logoutUser() {
    authStore.isLoggedIn = false;
    authStore.username = null;
    authStore.profilePictureUrl = null;
}

