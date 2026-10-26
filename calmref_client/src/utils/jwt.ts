const CLAIM_NAME = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
const CLAIM_NAMEID = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

interface JwtClaims {
    exp: number;
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": string;
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": string;
    iss: string;
    aud: string;
}

function decodeToken(token: string): JwtClaims | null {
    try {
        const payload = token.split(".")[1];
        // base64url enoding to base64 standard encoding
        let base64 = payload.replace(/-/g, "+").replace(/_/g, "/");

        while (base64.length % 4) {
            base64 = base64 + "=";
        }

        return JSON.parse(atob(base64));
    } catch {
        return null;
    }
}

export function getUserFromToken(token: string) {
    if (!token) return null;

    const decoded = decodeToken(token)

    if (!decoded || Date.now() >= decoded.exp * 1000) {
        return null;
    }

    const id = decoded[CLAIM_NAMEID];
    const username = decoded[CLAIM_NAME];

    if (!id || !username) return null;

    return { id, username }

}