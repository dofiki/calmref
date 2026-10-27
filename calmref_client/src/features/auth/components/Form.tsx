import { InputField } from "./ui/InputField";
import { PasswordField } from "./ui/PasswordField";
import PersonAddAltIcon from '@mui/icons-material/PersonAddAlt';
import { Button, Divider, } from "@mui/material";
import { useState } from "react";
import React from "react";
import { validateEmail, validatePassword } from "../../../utils/validate";
import { Link } from "react-router";
import { type FormProp } from "../types/index";

export const Form = ({
    formType,
    email,
    password,
    setEmail,
    setPassword,
    onSubmit,
    loading,
    usernameError,
    setUsernameError,
    passwordError,
    setPasswordError,
    error
}: FormProp) => {

    const outlinedPasswordId = React.useId();
    const [showPassword, setShowPassword] = useState(false);
    const handleClickShowPassword = () => setShowPassword((show) => !show);

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

    return (
        <>
            <div className="flex flex-col gap-4 p-4 md:pt-5 md:pr-5 bg-white w-full">
                <div className="flex items-center gap-1">
                    <PersonAddAltIcon />
                    <div className="text-xl font-semibold">{formType}</div>
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

                {error && (
                    <p className="text-red-500 text-center">{error}</p>
                )}

                <Button
                    variant="contained"
                    onClick={(e) => onSubmit(e)}
                    disabled={loading}
                >

                    {loading ? "Loading..." : formType}
                </Button>


                <Divider />

                {formType == "Register" ? (
                    <div className="flex gap-2 justify-center">
                        <p>Already have an account ?</p>

                        <Link to="/login" className="underline text-blue-500 cursor-pointer
                                         hover:text-blue-800 transition-colors delay-75">
                            Login</Link>

                    </div>
                ) : (
                    <div className="flex gap-2 justify-center">
                        <p>Don't have an account ?</p>
                        <Link to="/register" className="underline text-blue-500 cursor-pointer
                                         hover:text-blue-800 transition-colors delay-75">
                            Register</Link>
                    </div>)}
            </div>
        </>
    )
}
