import {
    InputOTP,
    InputOTPGroup,
    InputOTPSeparator,
    InputOTPSlot,
} from "./input-otp"

interface OtpFieldProp {
    setOtp: React.Dispatch<React.SetStateAction<string>>;
}

export const OtpField = ({ setOtp }: OtpFieldProp) => {
    return (
        <InputOTP maxLength={6} onChange={(value) => setOtp(value)}>
            <InputOTPGroup>
                <InputOTPSlot index={0} />
                <InputOTPSlot index={1} />
                <InputOTPSlot index={2} />
            </InputOTPGroup>
            <InputOTPSeparator />
            <InputOTPGroup>
                <InputOTPSlot index={3} />
                <InputOTPSlot index={4} />
                <InputOTPSlot index={5} />
            </InputOTPGroup>
        </InputOTP>
    )
}
