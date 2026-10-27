import { TextField } from "@mui/material"
import { type InputFieldProp } from "../../types/index"

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


