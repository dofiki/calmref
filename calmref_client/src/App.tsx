import { BrowserRouter, Route, Routes } from "react-router";
import { Home } from "./pages/Home";
import Register from "./pages/Register";
import Login from "./pages/Login";
import Navbar from "./components/Navbar";
import { History } from "@mui/icons-material";
import AuthProvider from "./context/AuthProvider";

const App = () => {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Navbar />
        <Routes>

          <Route path="/" element={<Home />}></Route>
          <Route path="/:slug" element={<Home />}></Route>
          {/*need reserve these so, they can't be used as slug */}
          <Route path="/register" element={<Register />}></Route>
          <Route path="/login" element={<Login />}></Route>
          <Route path="/history" element={<History />}></Route>
        </Routes>
      </AuthProvider>

    </BrowserRouter>
  )
}

export default App;