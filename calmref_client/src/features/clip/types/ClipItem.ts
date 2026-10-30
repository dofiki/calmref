export interface clipItem {
    id: string;
    content: string;
    updatedAt: string;
    expiresAt: string;
    userId: string | null;
    user: null;
    title: string;
    readOnly: boolean | null
}