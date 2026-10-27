const BASE_URL = "http://localhost:5024/api/auth";
import { useState } from "react";
import type { ReactNode } from "react"
import { getUserFromToken } from "../utils/jwt";
import { AuthContext } from "../context/AuthContext"
import type { User, RequestPayload, VerfiyOtpRequest, TokenResponse } from "../types/index";

const AuthProvider = ({ children }: { children: ReactNode }) => {

    const [user, setUser] = useState<User | null>(() => {
        const token = localStorage.getItem("accessToken");
        return token ? getUserFromToken(token) : null;
    });

    async function register(payload: RequestPayload): Promise<Response> {
        const res = await fetch(`${BASE_URL}/register`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(payload),
        });

        if (!res.ok) {
            const data = await res.json().catch(() => null);

            throw new Error(
                data?.message ||
                data?.title ||
                `Registration failed (${res.status})`
            );
        }

        return res;
    }
    async function verifyOtp({
        userId,
        otp,
    }: VerfiyOtpRequest) {
        return fetch(BASE_URL + "/verify-otp", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ userId, otp }),
        });
    }
    async function login(payload: RequestPayload): Promise<void> {

        const res = await fetch(`${BASE_URL}/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        })

        if (!res.ok) {
            throw new Error("Invalid Credential")
        }

        const tokens: TokenResponse = await res.json();

        const decodedUser = getUserFromToken(tokens.accessToken);

        if (!decodedUser) {
            throw new Error("Received an invalid or expired token")
        }

        localStorage.setItem("accessToken", tokens.accessToken);
        localStorage.setItem("refreshToken", tokens.refreshToken);

        setUser(decodedUser);

    }
    function logout() {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        setUser(null);
    }
    return (
        <AuthContext.Provider value={{ user, register, verifyOtp, login, logout }}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthProvider