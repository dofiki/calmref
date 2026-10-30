
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
    if (password.length < 8) {
        return "Password must be at least 8 characters";
    }

    if (!/\d/.test(password)) {
        return "Password must contain at least one number";
    }

    return "";
}

export function validateUrl(value: string) {
    if (!value.trim()) {
        return 'This field is required';
    }

    if (!/^[a-zA-Z0-9]+$/.test(value)) {
        return 'Only letters and numbers are allowed; no symbols or spaces';
    }

    return "";
}