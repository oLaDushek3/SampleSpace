import {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import UserApi from "../../dal/api/user/UserApi.ts";
import SampleApi from "../../dal/api/sample/SampleApi.ts";
import profilePageClasses from "./ProfilePage.module.css";
import SampleList from "../../components/sample-list/SampleList.tsx";
import Button from "../../components/button/Button.tsx";
import Modal from "../../components/modal/Modal.tsx";
import StatisticsModal from "../../components/statistics/StatisticsModal.tsx";
import useAuth from "../../hook/useAuth.ts";
import IUser from "../../dal/entities/IUser.ts";
import ISample from "../../dal/entities/ISample.ts";

interface ProfilePageProps {
}

export default function ProfilePage({}: ProfilePageProps) {
    const {nickname} = useParams<{ nickname: string }>();
    const [user, setUser] = useState<IUser>();
    const [userSamples, setUserSamples] = useState<Array<ISample>>()
    const [statisticsIsOpen, setStatisticsIsOpen] = useState(false)
    const {signOut} = useAuth()

    async function fetchUser() {
        const response = await UserApi.getUser(nickname!);
        setUser(response);
    }

    async function fetchUserSamples() {
        const response = await SampleApi.getUserSamples(user!.userGuid.toString());
        setUserSamples(response);
    }

    useEffect(() => {
        fetchUser();
    }, []);

    useEffect(() => {
        if(user != null)
            fetchUserSamples();
    }, [user]);

    return (
        <>
            <div className={profilePageClasses.profilePanel}>
                <div className="horizontalPanel">
                    <img className={profilePageClasses.avatar} src={user?.avatarPath} alt={"avatar"}/>
                    <div className="verticalPanel">
                        <h1>{user?.nickname}</h1>
                        <h2>{user?.email}</h2>

                        <div className="horizontalPanel">
                            <Button isPrimary={true}
                                    onClick={() => setStatisticsIsOpen(true)}>
                                Статистика
                            </Button>

                            <Button isPrimary={true}
                                    onClick={() => signOut()}>
                                Выйти
                            </Button>
                        </div>
                    </div>
                </div>

                <SampleList samples={userSamples}/>
            </div>

            <Modal open={statisticsIsOpen}>
                <StatisticsModal samples={userSamples} onClose={() => setStatisticsIsOpen(false)}/>
            </Modal>
        </>

    )
}