namespace Mp3MusicZone.Common.Constants
{
    using System;

    public static class Permissions
    {
        // Admin
        public const string GetUsers = "8504f823-16f2-450d-8075-99f1f207129d";
        public const string PromoteUserToRole = "13fed190-b1bb-48ff-ad81-310a653c4ce7";
        public const string DemoteUserFromRole = "f2c2d018-b954-4353-8007-b1b1d40aab31";

        // Uploader
        public const string GetUnapprovedSongs = "b3fef7df-7f10-4db3-b56a-c6789c688fc6";
        public const string ApproveSong = "f821861b-4e65-4368-b012-7379cf396a7c";
        public const string RejectSong = "e537c433-4142-4600-9a0e-3501ed7b3912";
        public const string GetUnapprovedSongForPlaying =
            "fa178dff-88b2-41b5-9b7b-230987ceb509";

        public const string DeleteSong = "c1d64461-9101-4d34-be73-646bf7b04d8c";
        public const string EditSong = "99bebebd-7a72-4565-b7bd-a74e4ba5b7c1";
        public const string UploadSong = "32ee5347-6bce-4c3e-baf7-15c1d4118b4d";
    }
}
