﻿namespace Mp3MusicZone.Common.Constants
{
    using System;

    public static class Permissions
    {
        public const string DeleteSong = "e5ef4e11-669b-4e50-8237-165ca2f7c28c";
        public const string EditSong = "ad9dd308-03d5-4b4e-9b99-f82bc86614ed";
        public const string UploadSong = "fdba5dbe-62d1-425e-8527-9ae324a8e29f";

        // Uploader
        public const string GetUnapprovedSongs = "34346c1f-5974-4c50-8dd4-a108625d8d4e";
        public const string ApproveSong = "ca516131-965c-43b7-8341-c2c7b0ff0ba2";
        public const string RejectSong = "e3c5cfda-c058-45ac-9653-ba363c5bebf7";

        // Admin
        public const string GetUsers = "7cc1d750-6226-4aa7-b0bb-0796f03951df";
        public const string PromoteUserToRole = "22fa81b1-6dd1-4524-81ab-78e22e211ba9";
        public const string DemoteUserFromRole = "a44d5791-7fda-4a2f-8749-347435890281";
    }
}
