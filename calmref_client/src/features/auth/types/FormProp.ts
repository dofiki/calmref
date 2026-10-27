export interface FormProp {
    formType: string,
    email: string,
    password: string,
    setEmail: React.Dispatch<React.SetStateAction<string>>
    setPassword: React.Dispatch<React.SetStateAction<string>>
    onSubmit: (e: React.MouseEvent<HTMLButtonElement>) => Promise<void>
    loading: boolean
    error: string
    usernameError: string,
    setUsernameError: React.Dispatch<React.SetStateAction<string>>,
    passwordError: string,
    setPasswordError: React.Dispatch<React.SetStateAction<string>>
}