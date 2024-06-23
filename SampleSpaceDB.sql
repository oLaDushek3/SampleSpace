CREATE
DATABASE sample_space

CREATE TABLE public.playlist_samples
(
    playlist_sample_guid uuid NOT NULL,
    playlist_guid        uuid NOT NULL,
    sample_guid          uuid NOT NULL
);
CREATE TABLE public.playlists
(
    playlist_guid   uuid                  NOT NULL,
    user_guid       uuid                  NOT NULL,
    name            character varying(75) NOT NULL,
    can_be_modified boolean DEFAULT true  NOT NULL
);

CREATE TABLE public.sample_comments
(
    sample_comment_guid uuid                   NOT NULL,
    sample_guid         uuid                   NOT NULL,
    user_guid           uuid                   NOT NULL,
    date                date DEFAULT now()     NOT NULL,
    comment             character varying(300) NOT NULL
);

CREATE TABLE public.samples
(
    sample_guid       uuid                  NOT NULL,
    sample_link       text                  NOT NULL,
    cover_link        text                  NOT NULL,
    name              character varying(75) NOT NULL,
    artist            character varying(75) NOT NULL,
    number_of_listens integer DEFAULT 0     NOT NULL,
    user_guid         uuid                  NOT NULL,
    duration          numeric DEFAULT 0     NOT NULL,
    vkontakte_link    text    DEFAULT ''::text,
    spotify_link      text    DEFAULT ''::text,
    soundcloud_link   text    DEFAULT ''::text,
    date              date    DEFAULT now() NOT NULL
);

CREATE TABLE public.users
(
    user_guid     uuid                  NOT NULL,
    nickname      character varying(75) NOT NULL,
    email         text                  NOT NULL,
    password_hash text                  NOT NULL,
    avatar_path   text    DEFAULT ''::text,
    is_admin      boolean DEFAULT false NOT NULL
);
-

ALTER TABLE ONLY public.playlist_samples
    ADD CONSTRAINT playlist_samples_pk PRIMARY KEY (playlist_sample_guid);

ALTER TABLE ONLY public.playlists
    ADD CONSTRAINT playlists_pk PRIMARY KEY (playlist_guid);

ALTER TABLE ONLY public.sample_comments
    ADD CONSTRAINT sample_comments_pk PRIMARY KEY (sample_comment_guid);

ALTER TABLE ONLY public.samples
    ADD CONSTRAINT sample_pk PRIMARY KEY (sample_guid);

ALTER TABLE ONLY public.users
    ADD CONSTRAINT user_pk PRIMARY KEY (user_guid);

CREATE TRIGGER users_insert_trigger
    AFTER INSERT
    ON public.users
    FOR EACH ROW EXECUTE FUNCTION public.users_insert_trigger_fnc();

ALTER TABLE ONLY public.playlist_samples
    ADD CONSTRAINT playlist_samples_playlists_playlist_guid_fk FOREIGN KEY (playlist_guid) REFERENCES public.playlists(playlist_guid) ON DELETE CASCADE;

ALTER TABLE ONLY public.playlist_samples
    ADD CONSTRAINT playlist_samples_samples_sample_guid_fk FOREIGN KEY (sample_guid) REFERENCES public.samples(sample_guid) ON DELETE CASCADE;

ALTER TABLE ONLY public.playlists
    ADD CONSTRAINT playlists_users_user_guid_fk FOREIGN KEY (user_guid) REFERENCES public.users(user_guid) ON DELETE CASCADE;

ALTER TABLE ONLY public.sample_comments
    ADD CONSTRAINT sample_comments_samples_sample_guid_fk FOREIGN KEY (sample_guid) REFERENCES public.samples(sample_guid) ON DELETE CASCADE;

ALTER TABLE ONLY public.sample_comments
    ADD CONSTRAINT sample_comments_users_user_guid_fk FOREIGN KEY (user_guid) REFERENCES public.users(user_guid) ON DELETE CASCADE;

ALTER TABLE ONLY public.samples
    ADD CONSTRAINT sample_user_user_guid_fk FOREIGN KEY (user_guid) REFERENCES public.users(user_guid) ON
DELETE
CASCADE;

CREATE FUNCTION public.users_insert_trigger_fnc() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
insert into playlists (playlist_guid, user_guid, name, can_be_modified)
values (gen_random_uuid(), new.user_guid, 'Сохраненные', false);
RETURN NEW;
END;
$$;

ALTER FUNCTION public.users_insert_trigger_fnc() OWNER TO postgres;