import type { UpdateClipItem } from "../../features/clip/types/UpdateClipItem";

const IMG_AUTH_URL = "http://localhost:5024/imgauth";
const IMG_KIT_PUBLIC_KEY = "public_jhFzX93Kxa6jWHo/J+Hs+bipke4=";
const IMG_KIT_UPLOAD_URL = "https://upload.imagekit.io/api/v1/files/upload";
const token = localStorage.getItem("accessToken");


export interface ImgAuthProp {
    token: string,
    expire: number,
    signature: string
}

export async function GetImgAuth() {
    const res = await fetch(IMG_AUTH_URL);
    const data = await res.json();
    return data;
}

export async function SendImage(imgObj: File): Promise<string> {

    const ImgAuthObj: ImgAuthProp = await GetImgAuth();

    const formData = new FormData();

    formData.append("file", imgObj);
    formData.append("fileName", imgObj.name);
    formData.append("token", ImgAuthObj.token);
    formData.append("expire", String(ImgAuthObj.expire));
    formData.append("signature", ImgAuthObj.signature);
    formData.append("publicKey", IMG_KIT_PUBLIC_KEY)

    const res = await fetch(IMG_KIT_UPLOAD_URL, {
        method: "POST",
        body: formData
    })

    if (!res.ok) {
        throw new Error("Image Upload Failed : ImageKit")
    }

    const data = await res.json();

    return data.url;
}

export async function GetClip(slug: string) {
    const res = await fetch(
        `http://localhost:5024/api/clip/${slug}`,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        }
    );

    return res;
}

export async function CreateClip(slug: string) {
    const res = await fetch(
        `http://localhost:5024/api/clip/${slug}`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({ title: slug })

        }
    );

    return res
}

export async function UpdateClip(slug: string, updatedClip: UpdateClipItem) {
    const res = await fetch(
        `http://localhost:5024/api/clip/${slug}`,
        {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify(updatedClip)

        }
    );

    return res
}

export async function GetClipEditors(slug: string) {
    const res = await fetch(
        `http://localhost:5024/api/clip/${slug}/editors`,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            }

        }
    );

    return res;
}