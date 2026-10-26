
import { useNavigate } from 'react-router';
import AssignmentIcon from '@mui/icons-material/Assignment';
// import Setting from './Setting';
// import type { User } from "../context/AuthProvider";
// { user }: { user: User | null }
const Navbar = () => {

    const navigate = useNavigate();
    // const user = false;
    return (
        <div className='flex justify-around h-15 items-center'>
            <button className='flex items-center gap-1 cursor-pointer
                             hover:text-blue-800 transition-colors
                delay-75' onClick={() => navigate("/")}>
                <AssignmentIcon />
                <span className='font-semibold text-xl'> Cref </span>
            </button>
            <div className='flex gap-5'>
                <button className='underline hover:text-blue-600 cursor-pointer'
                    onClick={() => navigate("/login")}>Login</button>
                <button className='underline hover:text-blue-600 cursor-pointer'
                    onClick={() => navigate("/register")}>Register</button></div>
        </div >
    )
}

export default Navbar