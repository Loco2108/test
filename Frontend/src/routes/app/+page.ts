import type { Poll } from "$lib/api";
import { apiClient } from "$lib/apiClient";
import type { PageLoad } from "./$types";

export const load: PageLoad = async () => {
    const questions = await _fetchQuestions();

    return {
        initialQuestions: questions,
    }
};

export async function _fetchQuestions(): Promise<Poll[]> {
    let result: Poll[] = [];

    const fetchRes = await apiClient.api.v1TestList();

    if (fetchRes.status === 200) {
        result = fetchRes.data;
    }

    return result;
}