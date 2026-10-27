import {
    FormControl,
    InputLabel,
    OutlinedInput,
    InputAdornment,
    IconButton,
    FormHelperText
} from "@mui/material"

import { Visibility, VisibilityOff } from "@mui/icons-material";
import { type PasswordFieldProp } from "../../types/index";

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
    }: PasswordFieldProp
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
