import { useState } from 'react';
import { validateUrl } from '../../utils/validate';
import { BackgroundSVG } from '../../components/BackgroundSVG';
import "quill/dist/quill.snow.css";
import { useNavigate, useParams } from 'react-router';


export function Home() {
    const { slug } = useParams();
    const [url, setUrl] = useState(slug ?? "");
    const [error, setError] = useState("");
    const navigate = useNavigate();

    function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
        const value = e.currentTarget.value;
        setUrl(value);
        setError(validateUrl(value));
    }

    async function handleSubmit(e: React.SubmitEvent<HTMLFormElement>) {
        e.preventDefault();

        if (error || !url.trim()) return;

        navigate("/clip/" + url);
    }

    function url_placeholder() {
        if (url) {
            if (url.length > 15) {
                return url.slice(0, 15) + "..."
            } else return url
        } else return "your_url";
    }

    return (
        <>
            <BackgroundSVG />

            <div className='flex-row gap-15'>

                <div className='flex flex-col w-full h-55 justify-center items-center gap-8'>

                    <div className='text-black mr-2 font-semibold text-xl md:text-4xl'>cref.vercel.app/clip/
                        <span className="text-gray-400">{url_placeholder()}</span>
                    </div>

                    <div className='flex flex-col gap-5 md:flex-row h-8 items-center justify-center relative'>

                        <div className='relative'>
                            <form
                                onSubmit={handleSubmit}
                                className="relative"
                            >
                                <input
                                    type="text"
                                    maxLength={74}
                                    onChange={handleChange}
                                    value={url}
                                    className="p-4 outline-2 hover:outline-4 hover:outline-blue-500
        rounded-full w-70 sm:w-80 md:w-100 lg:w-150 transition-all delay-75"
                                    placeholder="Enter here."
                                />

                                <button
                                    type="submit"
                                    className="border p-2 rounded-full absolute right-2 top-2
        bg-white hover:bg-blue-100 text-black outline-1
        outline-black cursor-pointer transition-all delay-75"
                                >
                                    Submit
                                </button>
                            </form>
                        </div>

                        <div className='text-red-500 absolute top-10 text-sm 
                                        text-center pt-6 '>{error}</div>
                    </div>

                </div>
            </div>
        </>
    )
}