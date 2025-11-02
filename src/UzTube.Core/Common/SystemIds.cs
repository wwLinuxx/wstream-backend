namespace UzTube.Core.Common;

/// <summary>
///     Holds constant GUIDs for system-level entities (users, roles, etc.).
/// </summary>
public static class SystemIds
{
    public static class User
    {
        /// <summary>
        ///     The root (super) user ID.
        /// </summary>
        public static readonly Guid Root = Guid.Parse("11111111-1111-1111-1111-111111111111");
    }

    public static class Role
    {
        /// <summary>
        ///     Root user ID.
        /// </summary>
        public static readonly Guid Root = Guid.Parse("11111111-1111-1111-1111-111111111111");

        /// <summary>
        ///     Administrator role ID.
        /// </summary>
        public static readonly Guid Admin = Guid.Parse("22222222-2222-2222-2222-222222222222");

        /// <summary>
        ///     Moderator role ID.
        /// </summary>
        public static readonly Guid Moderator = Guid.Parse("33333333-3333-3333-3333-333333333333");

        /// <summary>
        ///     Regular user role ID.
        /// </summary>
        public static readonly Guid User = Guid.Parse("44444444-4444-4444-4444-444444444444");
    }

    public static class Salt
    {
        /// <summary>
        ///     The root (super) salt ID.
        /// </summary>
        public const string Root = "ROOTSALT-AAAA-BBBB-CCCC-DDDDDDDDDDDD";
    }

    public static class Country
    {
        public static readonly Guid Uzbekistan = Guid.Parse("11111111-1111-1111-1111-111111111111");
    }
}