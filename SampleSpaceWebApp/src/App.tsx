import Header from "./components/header/Header.tsx";
import {Route, Routes} from "react-router-dom";
import MainPage from "./pages/main-page/MainPage.tsx";
import SearchPage from "./pages/search-page/SearchPage.tsx";
import RequireAuth from "./hoc/RequireAuth.tsx";
import AuthProvider from "./hoc/AuthProvider.tsx";

function App() {    
    return (
        <AuthProvider>
            <Header/>

            <Routes>
                <Route path="/" element={<MainPage/>}/>
                <Route path="/search" element={<SearchPage/>}/>
                <Route path="/gg" element={
                    <RequireAuth>
                        <div className={"gg"}><h1>Ну гг че</h1></div>
                    </RequireAuth>}/>
            </Routes>
        </AuthProvider>
    )
}

export default App
