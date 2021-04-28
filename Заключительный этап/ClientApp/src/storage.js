import { writable } from 'svelte/store';

let userData = { Name: "", Role: "", Jwt: "", IsAuth: false };

export const user = writable(userData);
export const lowLevelApi = writable("http://localhost:7001");
export const highLevelApi = writable("http://localhost:7002");