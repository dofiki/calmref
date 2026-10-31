import Quill from "quill";
import { CreateClip, GetClip, GetClipEditors, SendImage, UpdateClip } from '../../utils/lib/api';
import { useRef, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router";
import { validateUrl } from "../../utils/validate";
import { BackgroundSVG } from "../../components/BackgroundSVG";
import { IoMdArrowRoundBack } from "react-icons/io";
import type { clipItem } from "./types/ClipItem";
import { FaSave } from "react-icons/fa";
import Timeline from '@mui/lab/Timeline';
import TimelineItem from '@mui/lab/TimelineItem';
import TimelineSeparator from '@mui/lab/TimelineSeparator';
import TimelineConnector from '@mui/lab/TimelineConnector';
import TimelineContent from '@mui/lab/TimelineContent';
import TimelineDot from '@mui/lab/TimelineDot';
import { Typography } from "@mui/material";
import { formatEditedAt } from "../../utils/formatEditedAt";

export interface editor {
    username: string
    editedAt: string
}

export type editors = editor[];

const Clip = () => {
    const { slug } = useParams();
    const [result, setResult] = useState<clipItem | null>(null);
    const [loading, setLoading] = useState(false);
    const [content, setContent] = useState<string>("");
    const [switchStatus, setSwitchStatus] = useState<boolean | null>(false);
    const [error, setError] = useState("");
    const [editors, setEditors] = useState<editors | null>(null);
    const editorHostRef = useRef<HTMLDivElement | null>(null);
    const quillRef = useRef<Quill | null>(null);
    const navigate = useNavigate();

    // initialize text editor
    useEffect(() => {

        const host = editorHostRef.current;
        if (!host) return;

        let imgUrl: string;

        host.replaceChildren();

        const editorElement = document.createElement("div");
        host.appendChild(editorElement);

        const quill = new Quill(editorElement, {
            theme: "snow",
            placeholder: "Compose an epic",
            modules: {
                toolbar: {
                    container: [
                        [{ header: 1 }, { header: 2 }],
                        ["bold", "italic", "underline", "strike", "link", "image"],
                        ["blockquote", "code-block"],
                        [
                            { list: "ordered" },
                            { list: "bullet" },
                            { list: "check" },
                        ],
                    ],
                    handlers: {
                        image: function () {
                            console.log("Image button clicked");
                            let imgObj;

                            // open file selector
                            const inputField = document.createElement("input");
                            inputField.setAttribute("type", "file");
                            inputField.setAttribute("accept", "image/*")

                            inputField.click();

                            // image restrictions

                            // select image 
                            inputField.addEventListener("change", async () => {

                                imgObj = inputField.files?.[0];

                                if (!imgObj) return;

                                try {
                                    // get image url
                                    imgUrl = await SendImage(imgObj)

                                    // remember the cursor position before uploading
                                    const range = quill.getSelection(true);
                                    const index = range?.index ?? quill.getLength() - 1;

                                    // insert into existing content
                                    quill.insertEmbed(index, "image", imgUrl, "user");

                                    // move cursor after the image
                                    quill.setSelection(index + 1, 0, "user");
                                } catch (error) {
                                    console.error("Image upload failed:", error);
                                }

                            });


                        },
                    },
                },

            },
        });

        quillRef.current = quill;
        quill.clipboard.dangerouslyPasteHTML(content);

        return () => {
            quillRef.current = null;
            host.replaceChildren();
        };
    }, []);

    console.log(loading, error, switchStatus);

    // fetch clip / create clip
    useEffect(() => {
        if (!slug) return;

        let cancelled = false;

        async function loadClip() {
            setLoading(true);
            setError("");
            if (!slug) return;
            const validationError = validateUrl(slug);

            if (validationError) {
                if (!cancelled) {
                    setError(validationError);
                    setLoading(false);
                }
                return;
            }

            try {
                const res = await GetClip(slug);
                let data;

                if (res.status === 404) {
                    if (cancelled) return;

                    const createResponse = await CreateClip(slug);

                    if (!createResponse.ok) {
                        throw new Error("Failed to create clip");
                    }

                    data = await createResponse.json();

                } else {

                    if (!res.ok) {
                        throw new Error("Failed to fetch clip");
                    }
                    data = await res.json();
                }

                if (cancelled) return;

                setResult(data);
                setContent(data.content);
                setSwitchStatus(data.readOnly);
            } catch (err: unknown) {
                if (!cancelled) {
                    setError(
                        err instanceof Error
                            ? err.message
                            : "Something went wrong. Please try again."
                    );
                }
            } finally {
                if (!cancelled) {
                    setLoading(false);
                }
            }
        }

        loadClip();

        return () => {
            cancelled = true;
        };
    }, [slug]);

    // load content
    useEffect(() => {
        if (!result || !quillRef.current) return;

        if (!result.content) {
            quillRef.current.setText("");
            return;
        }

        try {
            const delta = JSON.parse(result.content);
            quillRef.current.setContents(delta);
        } catch (error) {
            console.error("Invalid clip Delta:", error);
        }
    }, [result]);

    useEffect(() => {

        async function loadClipEditors() {
            if (!slug) return;
            const res = await GetClipEditors(slug);
            const data = await res.json();
            setEditors(data);
        }

        loadClipEditors();
    }, [])

    async function handleSave() {
        const newClip = {
            title: result?.title,
            content: JSON.stringify(quillRef.current?.getContents()),
            readOnly: result?.readOnly
        }

        if (!slug) return

        const res = await UpdateClip(slug, newClip);

        if (!res.ok) {
            throw new Error("Failed to create clip");
        }

        const data = await res.json();

        setResult(data);
        setContent(data.content);
        setSwitchStatus(data.readOnly);
    }

    return (
        <>
            <BackgroundSVG />
            <div className="z-4 pl-5 pr-5 pt-8 w-full md:pt-5 md:w-250 mx-auto flex flex-col md:flex-row justify-center">
                <div className="flex flex-col items-center gap-4 w-full">
                    <div className="flex justify-around w-full">
                        <div className="w-full flex items-start gap-5">
                            <button onClick={() => navigate("/")} className="pt-1 cursor-pointer">
                                <IoMdArrowRoundBack size={18} /></button>

                            <div className="flex flex-col">
                                <div className="font-semibold">{result?.title}</div>
                                <a href={`localhost:5173/clip/${slug}`} target="_blank"
                                    className="text-sm text-blue-500 hover:text-blue-800 underline"
                                >localhost:5173/clip/{slug}</a>
                            </div>
                        </div>
                        <button className="flex items-center justify-center pl-2 pr-2 h-8 mt-2 bg-blue-500 hover:bg-blue-700
                         text-white gap-2 rounded-sm transition-colors delay-75 cursor-pointer"
                            onClick={handleSave}><FaSave />Save</button>
                    </div>

                    <div ref={editorHostRef} className="w-full h-100 md:h-120 bg-white
                            [&_.ql-toolbar]:bg-white [&_.ql-container]:flex-1 [&_.ql-container]:min-h-0
                            [&_.ql-container]:bg-white" />
                </div>

                <div className="w-55 mt-10 hidden md:block h-100 overflow-x-hidden">
                    {editors?.map((editor, index) => (
                        <Timeline key={index}>
                            <TimelineItem>
                                <TimelineSeparator>
                                    <TimelineDot />
                                    <TimelineConnector />
                                </TimelineSeparator>

                                <TimelineContent>
                                    <Typography variant="body2" className="w-20">
                                        {editor.username}
                                    </Typography>

                                    <Typography variant="caption" color="text.secondary">
                                        {formatEditedAt(editor.editedAt)}
                                    </Typography>
                                </TimelineContent>
                            </TimelineItem>
                        </Timeline>
                    ))}
                </div>
            </div>
        </>


    )
}

export default Clip

