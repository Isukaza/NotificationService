namespace NotificationService.DAL.Models;

public enum TokenType
{
    RegistrationConfirmation,
    EmailChangeOld,
    EmailChangeNew,
    PasswordReset,
    PasswordChange,
    UsernameChange,
    RoleChange,
    Unknown
}