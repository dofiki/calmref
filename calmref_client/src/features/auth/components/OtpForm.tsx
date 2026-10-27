import { Button } from "@mui/material"
import { OtpField } from "./ui/OtpField"
import { type OtpFormProp } from "../types"

export const OtpForm = ({ email, setOtp, error, loading, onOtpSubmit }: OtpFormProp) => {
    return (
        <div className="w-75 flex flex-col pt-5 p-5 gap-2 h-75 ">
            <div className="text-xl font-semibold">Enter the OTP code.</div>
            <div className="text-sm text-gray-500">We have send an email to {email}</div>
            <OtpField setOtp={setOtp} />

            {error && (
                <p className="text-red-500 text-sm">{error}</p>
            )}

            <div className="pt-2">
                <Button
                    variant="contained"
                    onClick={(e) => onOtpSubmit(e)}
                    disabled={loading}
                >
                    Send
                </Button>
            </div>
        </div>

    )
}
