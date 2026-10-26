
export function validateEmail(email: string): string {
    if (email.trim().length === 0) {
        return "Email is required";
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailRegex.test(email.trim())) {
        return "Please enter a valid email address";
    }

    return "";
}


export function validatePassword(password: string): string {
    if (password.length < 6) {
        return "Password must be at least 6 characters";
    }

    if (!/\d/.test(password)) {
        return "Password must contain at least one number";
    }

    return "";
}