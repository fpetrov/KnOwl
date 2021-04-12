import wretch from 'wretch';
import { refreshTokenPair, decodeJwt } from './helperFunctions';
import { user } from './storage';

let userData;

const userSub = user.subscribe(value => {
    userData = value;
});

export const httpClient = wretch()
    .catcher(401, async (error, request) => {
        let expirationDate = decodeJwt(userData.Jwt).exp;

        if (Date.now() >= expirationDate * 1000) {
            const jwt = refreshTokenPair();
          }

        return request.auth(`Bearer ${ userData.Jwt }`).replay().unauthorized(err => { console.log("Ooops... something went wrong") }).json();
    });