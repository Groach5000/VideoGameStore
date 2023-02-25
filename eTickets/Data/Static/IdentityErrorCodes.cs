using System.Security.Policy;
using System.Text.RegularExpressions;

namespace VideoGameStore.Data.Static
{

    public static class IdentityErrorCodes
        {
            public const string DefaultError = "DefaultError";
            public const string ConcurrencyFailure = "ConcurrencyFailure";
            public const string PasswordMismatch = "PasswordMismatch";
            public const string InvalidToken = "InvalidToken";
            public const string LoginAlreadyAssociated = "LoginAlreadyAssociated";
            public const string InvalidUserName = "InvalidUserName";
            public const string InvalidEmail = "InvalidEmail";
            public const string DuplicateUserName = "DuplicateUserName";
            public const string DuplicateEmail = "DuplicateEmail";
            public const string InvalidRoleName = "InvalidRoleName";
            public const string DuplicateRoleName = "DuplicateRoleName";
            public const string UserAlreadyHasPassword = "UserAlreadyHasPassword";
            public const string UserLockoutNotEnabled = "UserLockoutNotEnabled";
            public const string UserAlreadyInRole = "UserAlreadyInRole";
            public const string UserNotInRole = "UserNotInRole";
            public const string PasswordTooShort = "PasswordTooShort";
            public const string PasswordRequiresNonAlphanumeric = "PasswordRequiresNonAlphanumeric";
            public const string PasswordRequiresDigit = "PasswordRequiresDigit";
            public const string PasswordRequiresLower = "PasswordRequiresLower";
            public const string PasswordRequiresUpper = "PasswordRequiresUpper";

            public static string[] All = {
            DefaultError,
            ConcurrencyFailure,
            PasswordMismatch,
            InvalidToken,
            LoginAlreadyAssociated,
            InvalidUserName,
            InvalidEmail,
            DuplicateUserName,
            DuplicateEmail,
            InvalidRoleName,
            DuplicateRoleName,
            UserAlreadyHasPassword,
            UserLockoutNotEnabled,
            UserAlreadyInRole,
            UserNotInRole,
            PasswordTooShort,
            PasswordRequiresNonAlphanumeric,
            PasswordRequiresDigit,
            PasswordRequiresLower,
            PasswordRequiresUpper
        };
    }

}
