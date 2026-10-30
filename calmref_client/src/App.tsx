import { BrowserRouter, Route, Routes } from "react-router";
import { Home } from "./features/home/Home";
import Register from "./features/auth/Register"
import Login from "./features/auth/Login";
import Navbar from "./components/Navbar";
import { History } from "@mui/icons-material";
import AuthProvider from "./context/AuthProvider";
import Clip from "./features/clip/Clip";
import NotFound from "./features/notfound/NotFound";

const App = () => {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Navbar />
        <Routes>
          <Route path="/" element={<Home />}></Route>
          <Route path="/clip/:slug" element={<Clip />}></Route>
          <Route path="/register" element={<Register />}></Route>
          <Route path="/login" element={<Login />}></Route>
          <Route path="/history" element={<History />}></Route>
          <Route path="*" element={<NotFound />} />
        </Routes>
      </AuthProvider>

    </BrowserRouter >
  )
}

export default App;