import {
    FormControl,
    InputLabel,
    OutlinedInput,
    InputAdornment,
    IconButton,
    FormHelperText
} from "@mui/material"

import { Visibility, VisibilityOff } from "@mui/icons-material";

interface PasswordFieldProps {
    passwordError: string,
    outlinedPasswordId: string,
    showPassword: boolean,
    onClickShowPassword: () => void,
    onMouseDownPassword: (e: React.MouseEvent<HTMLButtonElement>) => void,
    onMouseUpPassword: (e: React.MouseEvent<HTMLButtonElement>) => void,
    password: string,
    onPasswordChange: (e: React.ChangeEvent<HTMLInputElement>) => void,
}

export const PasswordField = (
    {
        passwordError,
        outlinedPasswordId,
        showPassword,
        onClickShowPassword,
        onMouseDownPassword,
        onMouseUpPassword,
        password,
        onPasswordChange,
    }: PasswordFieldProps
) => {
    return (
        <FormControl variant="outlined" error={!!passwordError}>
            <InputLabel htmlFor={`${outlinedPasswordId}-input`}>Password</InputLabel>
            <OutlinedInput
                id={`${outlinedPasswordId}-input`}
                type={showPassword ? 'text' : 'password'}
                endAdornment={
                    <InputAdornment position="end">
                        <IconButton
                            aria-label={
                                showPassword ? 'hide the password' : 'display the password'
                            }
                            onClick={onClickShowPassword}
                            onMouseDown={onMouseDownPassword}
                            onMouseUp={onMouseUpPassword}
                            edge="end"
                        >
                            {showPassword ? <VisibilityOff /> : <Visibility />}
                        </IconButton>
                    </InputAdornment>
                }
                label="Password"
                value={password}
                onChange={onPasswordChange}
            />
            {passwordError && <FormHelperText>{passwordError}</FormHelperText>}
        </FormControl>
    )
}
