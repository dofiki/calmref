import React, { useState } from "react";
import {
    Button,
    Divider,
} from "@mui/material";

import {
    InputOTP,
    InputOTPGroup,
    InputOTPSeparator,
    InputOTPSlot,
} from "../components/ui/input-otp"

import { useNavigate } from "react-router";
import { validateEmail, validatePassword } from "../utils/validate";
import { InputField } from "../components/ui/InputField";
import { PasswordField } from "../components/ui/PasswordField";
import { BackgroundSVG } from "../components/ui/BackgroundSVG";
import PersonAddAltIcon from '@mui/icons-material/PersonAddAlt';
import { useAuth } from "../context/useAuth";

const Register = () => {
    const navigate = useNavigate();
    const outlinedPasswordId = React.useId();
    const { register, verifyOtp } = useAuth();
    const [showPassword, setShowPassword] = useState(false);
    const handleClickShowPassword = () => setShowPassword((show) => !show);
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [usernameError, setUsernameError] = useState("");
    const [passwordError, setPasswordError] = useState("");
    const [error, setError] = useState("")
    const [loading, setLoading] = useState(false)
    const [isOtp, setIsOtp] = useState(false);
    const [otp, setOtp] = useState("");
    const [userId, setUserId] = useState<string | null>(null);

    const handleMouseDownPassword = (event: React.MouseEvent<HTMLButtonElement>) => {
        event.preventDefault();
    };

    const handleMouseUpPassword = (event: React.MouseEvent<HTMLButtonElement>) => {
        event.preventDefault();
    };

    function handleEmailChange(e: React.ChangeEvent<HTMLInputElement>) {
        const value = e.currentTarget.value;
        setEmail(value);
        setUsernameError(validateEmail(value));
    }

    function handlePasswordChange(e: React.ChangeEvent<HTMLInputElement>) {
        const value = e.currentTarget.value;
        setPassword(value);
        setPasswordError(validatePassword(value));
    }

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

    async function handleSubmit(
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

            <div className=" flex flex-col items-center p-2 pt-15">

                <div className="z-1 outline-1 outline-black bg-white h-95 md:h-90 md:w-125
                flex flex-col md:flex-row justify-center gap-5 rounded-sm">

                    <div className="bg-blue-500 md:w-50 h-15 md:h-auto">
                    </div>

                    {isOtp ? (
                        <div className="w-75 flex flex-col pt-5 p-5 gap-2 h-75 ">
                            <div className="text-xl font-semibold">Enter the OTP code.</div>
                            <div className="text-sm text-gray-500">We have send an email to {email}</div>
                            <InputOTP maxLength={6} onChange={(value) => setOtp(value)}>
                                <InputOTPGroup>
                                    <InputOTPSlot index={0} />
                                    <InputOTPSlot index={1} />
                                    <InputOTPSlot index={2} />
                                </InputOTPGroup>
                                <InputOTPSeparator />
                                <InputOTPGroup>
                                    <InputOTPSlot index={3} />
                                    <InputOTPSlot index={4} />
                                    <InputOTPSlot index={5} />
                                </InputOTPGroup>
                            </InputOTP>

                            {error && (
                                <p className="text-red-500 text-sm">{error}</p>
                            )}

                            <div className="pt-2">
                                <Button
                                    type="submit"
                                    variant="contained"
                                    onClick={(e) => handleOtpSubmit(e)}
                                    disabled={loading}
                                >
                                    Send
                                </Button>
                            </div>
                        </div>

                    ) : (
                        <div className="flex flex-col gap-3 m-5 bg-white">
                            <div className="flex items-center gap-1">
                                <PersonAddAltIcon />
                                <div className="text-xl font-semibold">Register</div>
                            </div>
                            <InputField
                                email={email}
                                handleEmailChange={handleEmailChange}
                                usernameError={usernameError} />

                            <PasswordField
                                passwordError={passwordError}
                                outlinedPasswordId={outlinedPasswordId}
                                showPassword={showPassword}
                                onClickShowPassword={handleClickShowPassword}
                                onMouseDownPassword={handleMouseDownPassword}
                                onMouseUpPassword={handleMouseUpPassword}
                                password={password}
                                onPasswordChange={handlePasswordChange}
                            />

                            <Button
                                type="submit"
                                variant="contained"
                                onClick={(e) => handleSubmit(e)}
                                disabled={loading}
                            >
                                {loading ? "Loading..." : "Register"}
                            </Button>

                            {error && (
                                <p className="text-red-500 text-sm text-center">{error}</p>
                            )}

                            <Divider />

                            <div className="flex gap-2 justify-center"><p>Already have an account ?</p>
                                <span className="underline text-blue-500 cursor-pointer hover:text-blue-800
                            transition-colors delay-75"
                                    onClick={() => navigate("/login")}>Login</span></div>
                        </div>
                    )}


                </div>


            </div>

        </>

    );
};
export default Register;