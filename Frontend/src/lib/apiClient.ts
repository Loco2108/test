import { Api } from './api';

export const apiClient = new Api({
	baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5202',
	withCredentials: true,
});
