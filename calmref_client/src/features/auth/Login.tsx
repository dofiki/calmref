import React, { useState } from "react";
import { validateEmail, validatePassword } from "../../utils/validate";
import { BackgroundSVG } from "../../components/BackgroundSVG";
import { Form } from "../auth/components/Form";

const Login = () => {
    //const { user, login, register } = useAuth();
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [usernameError, setUsernameError] = useState("");
    const [passwordError, setPasswordError] = useState("");
    const [error, setError] = useState("")
    const [loading, setLoading] = useState(false)

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
            <div className=" flex flex-col items-center p-4 md:p-2 pt-15 ">
                <div className="outline-1 outline-black bg-white h-90 w-full md:h-100 md:w-130 
                                flex flex-col md:flex-row justify-center gap-5 rounded-sm">
                    <div className="bg-blue-500 md:w-65 h-auto hidden md:flex "></div>

                    <Form
                        formType="Login"
                        email={email}
                        password={password}
                        setEmail={setEmail}
                        setPassword={setPassword}
                        onSubmit={handleSubmit}
                        loading={loading}
                        usernameError={usernameError}
                        setUsernameError={setUsernameError}
                        passwordError={passwordError}
                        setPasswordError={setPasswordError}
                        error={error} />
                </div>
            </div>

        </>

    );
};
export default Login;