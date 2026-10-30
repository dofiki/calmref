import { Link, useLocation } from "react-router";
import { BackgroundSVG } from "../../components/BackgroundSVG";

const NotFound = () => {
    const { pathname } = useLocation();

    return (
        <>
            <BackgroundSVG />

            <div className="relative z-10 w-full 
            px-5 flex flex-col items-center justify-center gap-6 text-center">
                <h1 className="text-6xl md:text-8xl font-semibold text-black">404 :/</h1>

                <div className="flex flex-col gap-2 max-w-md">
                    <p className="text-xl font-semibold text-black">Page not found</p>
                    <p className="text-gray-600">
                        Nothing exists at{" "}
                        <span className="break-all text-black">localhost:5173{pathname}</span>.
                        Check the link for typos. Clips can also expire.
                    </p>
                </div>

                <Link
                    to="/"
                    className="border py-2 px-5 rounded-full bg-white hover:bg-blue-100 text-black
                        outline-1 outline-black cursor-pointer transition-all delay-75"
                >
                    Go to search
                </Link>
            </div>
        </>
    );
};

export default NotFound;