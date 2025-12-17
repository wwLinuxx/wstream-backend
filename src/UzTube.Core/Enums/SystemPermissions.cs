namespace UzTube.Core.Enums;

public enum SystemPermissions
{
    #region User
    UserCreate = 1,

    UserView,

    UserAllView,

    UserUpdate,

    UserDelete,
    #endregion

    #region Role
    RoleCreate,

    RoleView,

    RoleUpdate,

    RoleDelete,
    #endregion

    #region Follow
    FollewCreate,

    FollowView,

    FollowUpdate,

    FollowDelete,
    #endregion

    #region Post
    PostCreate,

    PostView,

    PostUpdate,

    PostDelete,
    #endregion

    #region Comment
    CommentCreate,

    CommentView,

    CommentUpdate,

    CommentDelete,
    #endregion

    #region Like
    LikeCreate,

    LikeView,

    LikeUpdate,

    LikeDelete,
    #endregion

    #region Category
    CategoryCreate,

    CategoryView,

    CategoryUpdate,

    CategoryDelete,
    #endregion

    #region Country
    CountryCreate,

    CountryView,

    CountryUpdate,

    CountryDelete
    #endregion
}