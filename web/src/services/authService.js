import http from './httpService';

const endpoint = 'Auth/';

const tokenObject = 'usuario';

function getLocalStorageItem(key) {
    return localStorage.getItem(key);
}

export function getJwt() {
    return JSON.parse(getLocalStorageItem(tokenObject)).token;
}

export const _authServiceLogin = async (request) => {
    const response = await http.post(endpoint + 'login', request);
    return response;
}

export default {
    _authServiceLogin
};