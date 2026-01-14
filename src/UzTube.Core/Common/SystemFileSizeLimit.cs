namespace UzTube.Core.Common;

public static class SystemFileSizeLimit
{
    public static class Upload
    {
        /// <summary>
        ///     The avatar limit size.
        /// </summary>
        public static readonly long Avatar = 10L * 1024 * 1024; // 10 MB

        /// <summary>
        ///     The photo limit size.
        /// </summary>
        public static readonly long Photo = 10L * 1024 * 1024; // 10 MB

        /// <summary>
        ///     The video limit size.
        /// </summary>
        public static readonly long Video = 10L * 1024 * 1024 * 1024; // 10 GB

        /// <summary>
        ///     The preview limit size.
        /// </summary>
        public static readonly long Preview = 10L * 1024 * 1024; // 10 MB
    }
}
