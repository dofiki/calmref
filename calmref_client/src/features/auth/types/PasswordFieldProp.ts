export interface PasswordFieldProp {
    passwordError: string,
    outlinedPasswordId: string,
    showPassword: boolean,
    onClickShowPassword: () => void,
    onMouseDownPassword: (e: React.MouseEvent<HTMLButtonElement>) => void,
    onMouseUpPassword: (e: React.MouseEvent<HTMLButtonElement>) => void,
    password: string,
    onPasswordChange: (e: React.ChangeEvent<HTMLInputElement>) => void,
}