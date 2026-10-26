import React, { useState } from "react";
import {
    Button,
    Divider,
} from "@mui/material";

import { useNavigate } from "react-router";
import { validateEmail, validatePassword } from "../utils/validate";
import { InputField } from "../components/ui/InputField";
import PersonIcon from '@mui/icons-material/Person';
import { PasswordField } from "../components/ui/PasswordField";
import { BackgroundSVG } from "../components/ui/BackgroundSVG";

const Login = () => {
    const navigate = useNavigate();
    const outlinedPasswordId = React.useId();
    //const { user, login, register } = useAuth();
    const [showPassword, setShowPassword] = useState(false);
    const handleClickShowPassword = () => setShowPassword((show) => !show);
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [usernameError, setUsernameError] = useState("");
    const [passwordError, setPasswordError] = useState("");
    const [error, setError] = useState("")
    const [loading, setLoading] = useState(false)

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

    async function handleSubmit(e: React.MouseEvent<HTMLButtonElement, MouseEvent>) {
        e.preventDefault();

        const nameCheck = validateEmail(email);
        const passwordCheck = validatePassword(password);

        setUsernameError(nameCheck);
        setPasswordError(passwordCheck);

        if (nameCheck || passwordCheck) {
            return;
        }

        setError("")
        setLoading(true)

        try {
            // await register({ username, password });
            console.log("registering");

        } catch (error: unknown) {
            if (error instanceof Error) {
                setError(error.message)

            } else {
                setError(error as string);
            }
        } finally {

            setLoading(false)
        }
    }

    return (
        <>
            <BackgroundSVG />
            <div className=" flex flex-col items-center p-2 pt-15 h-auto ">
                <div className="outline-1 outline-black h-95 bg-white md:h-90 md:w-125
             flex flex-col md:flex-row 
                                justify-center gap-5 rounded-sm">
                    <div className="bg-blue-500 md:w-50 h-15 md:h-auto "></div>

                    <div className="flex flex-col gap-3 m-5">
                        <div className="flex items-center gap-1">
                            <PersonIcon />
                            <div className="text-xl font-semibold">Login</div>
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
                            {loading ? "Loading..." : "Login"}
                        </Button>

                        {error && (
                            <p className="text-red-500 text-sm text-center">{error}</p>
                        )}

                        <Divider />

                        <div className="flex gap-2 justify-center"><p>Don't have an account ?</p>
                            <span className="underline text-blue-500 cursor-pointer hover:text-blue-800
                        transition-colors delay-75"
                                onClick={() => navigate("/register")}>Register</span></div>
                    </div>
                </div>
            </div>

        </>

    );
};
export default Login;