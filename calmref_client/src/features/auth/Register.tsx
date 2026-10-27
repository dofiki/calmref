import React, { useState } from "react";
import { useNavigate } from "react-router";
import { validateEmail, validatePassword } from "../../utils/validate"
import { BackgroundSVG } from "../../components/BackgroundSVG";
import { useAuth } from "../../context/useAuth";
import { Form } from "../auth/components/Form";
import { OtpForm } from "../auth/components/OtpForm";

const Register = () => {
    const navigate = useNavigate();
    const { register, verifyOtp } = useAuth();
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [error, setError] = useState("")
    const [loading, setLoading] = useState(false)
    const [isOtp, setIsOtp] = useState(false);
    const [otp, setOtp] = useState("");
    const [userId, setUserId] = useState<string | null>(null);
    const [usernameError, setUsernameError] = useState("");
    const [passwordError, setPasswordError] = useState("");

    async function handleOtpSubmit(
        e: React.MouseEvent<HTMLButtonElement, MouseEvent>
    ) {
        e.preventDefault();

        if (!userId) {
            setError("User ID is missing. Please register again.");
            return;
        }

        if (otp.length !== 6) {
            setError("Please enter the complete 6-digit OTP.");
            return;
        }

        setError("");
        setLoading(true);

        try {
            const res = await verifyOtp({
                userId,
                otp,
            });

            if (res.ok) {
                navigate("/login");
            } else {
                setError("Invalid or expired OTP. Please try again.");
            }
        } catch (error: unknown) {
            setError(
                error instanceof Error
                    ? error.message
                    : "OTP verification failed. Please try again."
            );
        } finally {
            setLoading(false);
        }
    }

    async function handleRegisterSubmit(
        e: React.MouseEvent<HTMLButtonElement, MouseEvent>
    ) {
        e.preventDefault();

        const nameCheck = validateEmail(email);
        const passwordCheck = validatePassword(password);

        setUsernameError(nameCheck);
        setPasswordError(passwordCheck);

        if (nameCheck || passwordCheck) return;

        setError("");
        setLoading(true);

        try {
            const res = await register({ email, password });
            const data = await res.json();

            if (!data.userId) {
                setError("Registration response is missing userId.");
                return;
            }

            setUserId(data.userId);
            setIsOtp(true);
        } catch (error: unknown) {
            setError(
                error instanceof Error
                    ? error.message
                    : "Registration failed. Please try again."
            );
        } finally {
            setLoading(false);
        }
    }

    return (

        <>
            <BackgroundSVG />

            <div className=" flex flex-col items-center p-4 md:p-2 pt-15">

                <div className="outline-1 outline-black bg-white h-90 w-full md:h-100 md:w-130
                                flex flex-col md:flex-row justify-center gap-5 rounded-sm">
                    <div className="bg-blue-500 md:w-65 h-auto hidden md:flex "></div>

                    {isOtp ? (

                        <OtpForm
                            email={email}
                            setOtp={setOtp}
                            error={error}
                            loading={loading}
                            onOtpSubmit={handleOtpSubmit}
                        />

                    ) : (

                        <Form
                            formType="Register"
                            email={email}
                            password={password}
                            setEmail={setEmail}
                            setPassword={setPassword}
                            onSubmit={handleRegisterSubmit}
                            loading={loading}
                            usernameError={usernameError}
                            setUsernameError={setUsernameError}
                            passwordError={passwordError}
                            setPasswordError={setPasswordError}
                            error={error} />
                    )}
                </div>
            </div>
        </>
    );
};
export default Register;