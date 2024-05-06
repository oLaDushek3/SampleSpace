import {useEffect, useState} from "react";
import SampleCommentApi from "../../dal/api/sample-comment/SampleCommentApi.ts";
import ISamplePlayer from "../../models/ISamplePlayer.ts";
import Button, {ButtonVisualType} from "../../components/button/Button.tsx";
import {IoPause, IoPlay} from "react-icons/io5";
import useSamplePlayer from "../../hook/useSamplePlayer.ts";
import {Link, useParams} from "react-router-dom";
import SampleApi from "../../dal/api/sample/SampleApi.ts";
import {SlSocialVkontakte, SlSocialSpotify, SlSocialSoundcloud} from "react-icons/sl";
import Icon from "../../components/icon/Icon.tsx";
import Comment from "../../components/comment/Comment.tsx";
import UserAvatar from "../../components/user-avatar/UserAvatar.tsx";
import CommentInput from "../../components/comment-input/CommentInput.tsx";
import useAuth from "../../hook/useAuth.ts";
import samplePageClasses from "./SamplePage.module.css";
import NotFoundPage from "../not-found/NotFoundPage.tsx";
import LoadingSpinner from "../../components/loading-spinner/LoadingSpinner.tsx";
import ISampleComment from "../../dal/entities/ISampleComment.ts";

export default function SamplePage() {
    const {user} = useAuth();
    const {sampleGuid} = useParams<{ sampleGuid: string }>();
    const [samplePlayer, setSamplePlayer] = useState<ISamplePlayer | null>()
    const [sampleComments, setSampleComments] = useState<ISampleComment[]>()
    const {
        handlePlayingSamplePlayer,
        handlePlayPause,
        handleSeek,
        currentTime,
        isPlaying
    } = useSamplePlayer()

    async function fetchSample() {
        if (sampleGuid) {
            const response = await SampleApi.getSample(sampleGuid!);
            setSamplePlayer(response !== null ? {sample: response, isActive: false} : null);
        }
    }

    useEffect(() => {
        void fetchSample();
    }, [sampleGuid]);

    async function fetchSampleComments() {
        const response = await SampleCommentApi.getSampleComments(samplePlayer!.sample.sampleGuid);
        setSampleComments(response);
    }

    useEffect(() => {
        if (samplePlayer)
            void fetchSampleComments();
    }, [samplePlayer]);

    function formatDuration(durationSeconds: number) {
        const minutes = Math.floor(durationSeconds / 60);
        const seconds = Math.floor(durationSeconds % 60);
        const formattedSeconds = seconds.toString().padStart(2, "0");
        return `${minutes}:${formattedSeconds}`;
    }

    const handlePlayback = () => {
        if (!samplePlayer!.isActive)
            handlePlayingSamplePlayer(samplePlayer!);
        else
            handlePlayPause();
    }

    const handleAddComment = async (comment: string) => {
        await SampleCommentApi.createNewComment(samplePlayer!.sample.sampleGuid, user!.userGuid, comment);
        await fetchSampleComments();
    }

    if (samplePlayer === undefined)
        return <LoadingSpinner/>

    if (samplePlayer === null)
        return <NotFoundPage/>

    return (
        <div className={samplePageClasses.sampleModal + " verticalPanel"}>

            <div className={samplePageClasses.samplePanel + " horizontalPanel"}>
                <img className={samplePageClasses.cover} src={samplePlayer?.sample.coverLink} alt="Cover image"/>

                <div className={samplePageClasses.mainSpace + " verticalPanel"}>
                    <div>
                        <p style={{fontSize: "20px", fontWeight: "bold"}} 
                           className={"singleLineText"}>{samplePlayer.sample.name}</p>
                        <p className={"singleLineText"}>{samplePlayer?.sample.artist}</p>
                    </div>

                    <div className={samplePageClasses.track + " horizontalPanel"}>
                        <Button visualType={ButtonVisualType.withIcon}
                                isPrimary={true}
                                onClick={handlePlayback}>
                            {samplePlayer?.isActive && isPlaying ? <IoPause/> : <IoPlay/>}
                        </Button>

                        <input type="range"
                               min="0"
                               max={samplePlayer?.sample.duration}
                               step="0.001"
                               value={samplePlayer?.isActive ? currentTime : 0}
                               onChange={samplePlayer?.isActive ? handleSeek : () => {
                               }}/>

                        {/*Исправить*/}
                        {samplePlayer &&
                            <p>{samplePlayer?.isActive
                                ? formatDuration(currentTime)
                                : formatDuration(samplePlayer.sample.duration)}</p>}
                    </div>

                    <div className={"horizontalPanel"}>
                        <Link to={samplePlayer?.sample.vkontakteLink} target="_blank">
                            <Icon>
                                <SlSocialVkontakte/>
                            </Icon>
                        </Link>

                        <Link to={samplePlayer?.sample.spotifyLink} target="_blank">
                            <Icon>
                                <SlSocialSpotify/>
                            </Icon>
                        </Link>

                        <Link to={samplePlayer?.sample.soundcloudLink} target="_blank">
                            <Icon>
                                <SlSocialSoundcloud/>
                            </Icon>
                        </Link>
                    </div>

                    <div className={samplePageClasses.userPanel + " horizontalPanel"}>
                        <Link to={`/${samplePlayer?.sample.user.nickname}`}>
                            <UserAvatar src={samplePlayer?.sample.user.avatarPath} height={42}/>
                        </Link>
                        <Link to={`/${samplePlayer?.sample.user.nickname}`}>
                            <p>{samplePlayer?.sample.user.nickname}</p>
                        </Link>
                    </div>
                </div>
            </div>

            {sampleComments ?
                <div className={samplePageClasses.commentsPanel + " verticalPanel"}>
                    <h3 style={{fontSize: "18px", fontWeight: "bold"}}>Комментарии</h3>
                    {sampleComments.length != 0 ?
                        sampleComments.map(comment => <Comment comment={comment} updateCallBack={fetchSampleComments}
                                                               key={comment.sampleCommentGuid.toString()}/>)
                        : <p className={"secondaryFont"} style={{textAlign: "center"}}>Комментирии отсутствуют. Будьте
                            первым.</p>
                    }
                    {}
                </div>
                : <LoadingSpinner/>
            }

            {user &&
                <CommentInput submitCallBack={handleAddComment}/>
            }
        </div>
    )
}