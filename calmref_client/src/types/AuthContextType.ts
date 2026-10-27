import type { User, RequestPayload, VerfiyOtpRequest } from "./index";

export interface AuthContextType {
    user: User | null,
    register: (payload: RequestPayload) => Promise<Response>,
    verifyOtp: ({ userId, otp }: VerfiyOtpRequest) => Promise<Response>,
    login: (payload: RequestPayload) => Promise<void>,
    logout: () => void,

}