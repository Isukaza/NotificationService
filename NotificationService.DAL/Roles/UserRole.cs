namespace NotificationService.DAL.Roles;

public enum UserRole
{
    /// <summary>
    /// SuperAdmin: Full control over all aspects of the system.
    /// <para>Permissions:</para>
    /// <list type="bullet">
    /// <item>Manage tracks (add/delete/edit).</item>
    /// <item>Manage albums (create/delete/edit).</item>
    /// <item>Assign/remove user roles (including Admin, Manager, Publisher).</item>
    /// <item>Change the subscription status of any user.</item>
    /// <item>Delete any user except other SuperAdmins (deletion of a SuperAdmin is only possible through the database).</item>
    /// <item>Manage subscriptions for all users.</item>
    /// <item>Perform Admin and Publisher tasks (full rights over their actions).</item>
    /// </list>
    /// </summary>
    SuperAdmin = 4,

    /// <summary>
    /// Admin: Responsible for operational tasks and user support.
    /// <para>Permissions:</para>
    /// <list type="bullet">
    /// <item>Add/delete tracks.</item>
    /// <item>Edit existing albums.</item>
    /// <item>Assign/demote Managers.</item>
    /// <item>Assist users with requests (within their permissions).</item>
    /// <item>If the Admin has a subscription, they can enable/disable it for access to premium content.</item>
    /// </list>
    /// </summary>
    Admin = 3
}