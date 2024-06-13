import {useEffect, useRef, useState} from "react";
import ISamplePlayer from "../../models/ISamplePlayer.ts";
import Button, {ButtonVisualType} from "../../components/button/Button.tsx";
import {IoPause, IoPlay} from "react-icons/io5";
import useSamplePlayer from "../../hook/useSamplePlayer.ts";
import {Link, useParams} from "react-router-dom";
import {SlSocialSoundcloud, SlSocialSpotify, SlSocialVkontakte} from "react-icons/sl";
import {MdAdd} from "react-icons/md";
import {IoIosShareAlt} from "react-icons/io";
import Icon from "../../components/icon/Icon.tsx";
import Comment from "../../components/sample/comment/Comment.tsx";
import UserAvatar from "../../components/user-avatar/UserAvatar.tsx";
import CommentInput from "../../components/sample/comment/comment-input/CommentInput.tsx";
import useAuth from "../../hook/useAuth.ts";
import samplePageClasses from "./SamplePage.module.css";
import NotFoundPage from "../not-found/NotFoundPage.tsx";
import LoadingSpinner from "../../components/loading-spinner/LoadingSpinner.tsx";
import ISampleComment from "../../dal/entities/ISampleComment.ts";
import useClickOutside from "../../hook/useClickOutside.ts";
import PlaylistSamplePanel from "../../components/sample/playlist-sample-panel/PlaylistSamplePanel.tsx";
import useSampleApi from "../../dal/api/sample/useSampleApi.ts";
import useSampleCommentApi from "../../dal/api/sample-comment/useSampleCommentApi.ts";

export default function SamplePage() {
    const {getSample} = useSampleApi();
    const {createNewComment, getSampleComments} = useSampleCommentApi();
    const {sampleGuid} = useParams<{ sampleGuid: string }>();
    const {loginUser} = useAuth();
    const [samplePlayer, setSamplePlayer] = useState<ISamplePlayer | null>()
    const [sampleComments, setSampleComments] = useState<ISampleComment[]>()
    const {
        handlePlayingSamplePlayer,
        handlePlayPause,
        handleSeek,
        currentTime,
        isPlaying
    } = useSamplePlayer()
    const [playlistsPanelIsActive, setPlaylistsPanelIsActive] = useState<boolean>(false)
    const playlistsPanelRef = useRef(null);
    useClickOutside(playlistsPanelRef, () => {
        setPlaylistsPanelIsActive(false)
    });

    async function fetchSample() {
        if (sampleGuid) {
            const response = await getSample(sampleGuid);
            setSamplePlayer(response !== null ? {sample: response, isActive: false} : null);
        }
    }

    useEffect(() => {
        void fetchSample();
    }, [sampleGuid]);

    async function fetchSampleComments() {
        const response = await getSampleComments(samplePlayer!.sample.sampleGuid);
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
        await createNewComment(samplePlayer!.sample.sampleGuid, loginUser!.userGuid, comment);
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
                    <div className={samplePageClasses.header}>
                        <p style={{fontSize: "20px", fontWeight: "bold"}}
                           className={"singleLineText"}>{samplePlayer.sample.name}</p>
                        <p className={"singleLineText"}>{samplePlayer?.sample.artist}</p>
                    </div>

                    <div className={samplePageClasses.track + " horizontalPanel"}>
                        <Button visualType={ButtonVisualType.withIcon}
                                primary={true}
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

                        {samplePlayer &&
                            <p>{samplePlayer?.isActive
                                ? formatDuration(currentTime)
                                : formatDuration(samplePlayer.sample.duration)}</p>}
                    </div>

                    <div className={samplePageClasses.toolsPanel}>
                        <div className={"horizontalPanel"}>
                            {samplePlayer.sample.vkontakteLink &&
                                <Link to={samplePlayer.sample.vkontakteLink} target="_blank">
                                    <Icon>
                                        <SlSocialVkontakte/>
                                    </Icon>
                                </Link>}

                            {samplePlayer.sample.spotifyLink &&
                                <Link to={samplePlayer?.sample.spotifyLink} target="_blank">
                                    <Icon>
                                        <SlSocialSpotify/>
                                    </Icon>
                                </Link>}

                            {samplePlayer.sample.soundcloudLink &&
                                <Link to={samplePlayer?.sample.soundcloudLink} target="_blank">
                                    <Icon>
                                        <SlSocialSoundcloud/>
                                    </Icon>
                                </Link>}
                        </div>

                        <div className={"horizontalPanel"}>
                            <Button visualType={ButtonVisualType.icon}
                                    onClick={() => {
                                        navigator.clipboard.writeText(window.location.href);
                                    }}>
                                <Icon>
                                    <IoIosShareAlt/>
                                </Icon>
                            </Button>

                            {loginUser &&
                                <div ref={playlistsPanelRef} className={samplePageClasses.playlistPanelContainer}>
                                    <Button visualType={ButtonVisualType.icon}
                                            onClick={() => {
                                                setPlaylistsPanelIsActive(prevState => !prevState)
                                            }}>
                                        <Icon>
                                            <MdAdd/>
                                        </Icon>
                                    </Button>
                                    <PlaylistSamplePanel isActive={playlistsPanelIsActive} sampleGuid={sampleGuid!}/>
                                </div>}
                        </div>
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
                    {sampleComments.length != 0 ? sampleComments.map(comment =>
                            <Comment comment={comment} updateCallBack={fetchSampleComments}
                                     key={comment.sampleCommentGuid.toString()}/>) :
                        <p className={"secondaryFont"} style={{textAlign: "center"}}>Комментирии отсутствуют</p>
                    }
                    {}
                </div>
                : <LoadingSpinner/>
            }

            {loginUser &&
                <CommentInput submitCallBack={handleAddComment}/>
            }
        </div>
    )
}