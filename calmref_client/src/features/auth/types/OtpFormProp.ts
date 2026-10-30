export interface OtpFormProp {
    email: string
    setOtp: React.Dispatch<React.SetStateAction<string>>
    error: string
    loading: boolean
    onOtpSubmit: (e: React.MouseEvent<HTMLButtonElement>) => Promise<void>
}