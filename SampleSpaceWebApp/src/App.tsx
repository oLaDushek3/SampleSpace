import {Route, Routes} from "react-router-dom";
import MainPage from "./pages/main-page/MainPage.tsx";
import SearchPage from "./pages/search-page/SearchPage.tsx";
import AuthProvider from "./hoc/AuthProvider.tsx";
import ProfilePage from "./pages/profile-page/ProfilePage.tsx";
import NotFoundPage from "./pages/not-found/NotFoundPage.tsx";
import SamplePlayerProvider from "./hoc/SampleProvider.tsx";
import SamplePage from "./pages/sample-page/SamplePage.tsx";
import Root from "./components/root/Root.tsx";

function App() {
    return (
        <AuthProvider>
            <SamplePlayerProvider>
                <Routes>
                    <Route path="/" element={<Root/>}>
                        <Route index element={<MainPage/>}/>
                        <Route path="search" element={<SearchPage/>}/>
                        <Route path="sample/:sampleGuid" element={<SamplePage/>}/>
                        <Route path="gg" element={<div className="centered"><h1>Ну гг че</h1></div>}/>
                        <Route path=":nickname" element={<ProfilePage/>}/>
                        <Route path="*" element={<NotFoundPage/>}/>
                    </Route>
                </Routes>
            </SamplePlayerProvider>
        </AuthProvider>
    )
}

export default App
