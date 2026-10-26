import { TextField } from "@mui/material"

interface InputFieldProp {
    email: string,
    handleEmailChange: (e: React.ChangeEvent<HTMLInputElement>) => void,
    usernameError: string,
}

export const InputField = ({ email, handleEmailChange, usernameError }: InputFieldProp) => {
    return (
        <TextField
            required
            id="outlined-required"
            label="Email"
            defaultValue=""
            value={email}
            onChange={handleEmailChange}
            error={!!usernameError}
            helperText={usernameError}
        />
    )
}


