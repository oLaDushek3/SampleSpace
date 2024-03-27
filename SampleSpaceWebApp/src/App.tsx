import Header from "./components/header/Header.tsx";
import {Route, Routes} from "react-router-dom";
import MainPage from "./pages/main-page/MainPage.tsx";
import SearchPage from "./pages/search-page/SearchPage.tsx";
import RequireAuth from "./hoc/RequireAuth.tsx";
import AuthProvider from "./hoc/AuthProvider.tsx";
import ProfilePage from "./pages/profile-page/ProfilePage.tsx";
import NotFoundPage from "./pages/not-found/NotFoundPage.tsx";

function App() {    
    return (
        <AuthProvider>
            <Header/>

            <Routes>
                <Route path="/" element={<MainPage/>}/>
                <Route path="/search" element={<SearchPage/>}/>
                <Route path="/gg" element={
                    <RequireAuth>
                        <div className="centered"><h1>Ну гг че</h1></div>
                    </RequireAuth>}/>
                <Route path="/:nickname" element={
                    <RequireAuth>
                        <ProfilePage/>
                    </RequireAuth>
                }/>
                <Route path="*" element={
                    <NotFoundPage/>
                }/>
            </Routes>
        </AuthProvider>
    )
}

export default App
