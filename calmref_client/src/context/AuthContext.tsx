import { createContext } from "react";
import type { User, verfiyOtpRequest } from "./AuthProvider";
import type { RequestPayload } from "./AuthProvider";

interface AuthContextType {
    user: User | null,
    register: (payload: RequestPayload) => Promise<Response>,
    verifyOtp: ({ userId, otp }: verfiyOtpRequest) => Promise<Response>,
    login: (payload: RequestPayload) => Promise<void>,
    logout: () => void,

}

export const AuthContext = createContext<AuthContextType | null>(null);