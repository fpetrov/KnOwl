import Cookies from 'js-cookie';
import { user } from './storage';
import { httpClient } from './httpClient';

export function decodeJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
};

export function generateString(length) {
    var result           = '';
    var characters       = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;

    for ( var i = 0; i < length; i++ )
    {
       result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }

    return result;
 }

export function isEmpty(content) {
    return (!content || 0 === content.length);
}

export function saveImage(photos)
{
    // Create new image.
    httpClient.url("/api/Storage/image")
    .formData({ file: photos })
        .post()
        .json(json => {
            return true;
        });
}

export function saveUser(jwt, refreshToken)
{
    Cookies.set('refreshToken', refreshToken, { expires: 15 });

    var decodedJwt = decodeJwt(jwt);

    user.set({ Jwt: jwt, Name: decodedJwt.name, Role: decodedJwt.role, IsAuth: true });
}

export function refreshTokenPair()
{
    var refreshToken = Cookies.get('refreshToken');

	if (!isEmpty(refreshToken))
	{
		// Refresh token pair.
		httpClient.url("/api/Authentication/refreshToken")
            .post({ token: refreshToken, fingerprint: navigator.userAgent })
            .json(json => {
                saveUser(json.jwt, json.refreshToken);
            })
			.catch(error => {
                signOut();
				console.log(error);
			});
	}
}

export function signOut()
{
    var refreshToken = Cookies.get('refreshToken');
    //revokeRefreshToken(refreshToken);

    Cookies.remove('refreshToken');

    user.set({ Jwt: "", Name: "", Role: "", IsAuth: false });
}

export function redirect(href)
{
    location.href = href;   
}

export function delayedRedirect(href, delay)
{
    setTimeout(() => redirect(href), delay);
}

export function revokeRefreshToken(refreshToken)
{
    httpClient.url("/api/Authentication/revokeToken")
        .post({ token: refreshToken, fingerprint: navigator.userAgent });
}