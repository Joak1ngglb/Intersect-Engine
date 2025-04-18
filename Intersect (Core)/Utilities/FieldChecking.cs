﻿using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Intersect.Utilities;


public static partial class FieldChecking
{

    //Field Checking
    public const string PATTERN_EMAIL_ADDRESS =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public const string PATTERN_PASSWORD = @"^[-_=\+`~!@#\$%\^&\*()\[\]{}\\|;\:'"",<\.>/\?a-zA-Z0-9]{4,64}$";

    public const string PATTERN_USERNAME = @"^[a-zA-Z0-9]{2,24}$";

    public const string PATTERN_GUILDNAME = @"^[a-zA-Z0-9 ]{3,20}$";

    public static bool IsWellformedEmailAddress([NotNullWhen(true)] string? email, string emailRegex)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            var customPattern = emailRegex;
            if (string.IsNullOrEmpty(customPattern))
            {
                customPattern = PATTERN_EMAIL_ADDRESS;
            }

            return Regex.IsMatch(email, customPattern);
        }
        catch (ArgumentException)
        {
            return Regex.IsMatch(email, PATTERN_EMAIL_ADDRESS);
        }
    }

    public static bool IsValidUsername([NotNullWhen(true)] string? username, string usernameRegex)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return false;
        }

        try
        {
            var customPattern = usernameRegex;
            if (string.IsNullOrEmpty(customPattern))
            {
                customPattern = PATTERN_USERNAME;
            }

            return Regex.IsMatch(username.Trim(), customPattern);
        }
        catch (ArgumentException)
        {
            return Regex.IsMatch(username.Trim(), PATTERN_USERNAME);
        }
    }

    public static bool IsValidPassword([NotNullWhen(true)] string? password, string passwordRegex)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        try
        {
            var customPattern = passwordRegex;
            if (string.IsNullOrEmpty(customPattern))
            {
                customPattern = PATTERN_PASSWORD;
            }

            return Regex.IsMatch(password.Trim(), customPattern);
        }
        catch (ArgumentException)
        {
            return Regex.IsMatch(password.Trim(), PATTERN_PASSWORD);
        }
    }

    public static bool IsValidGuildName(string guildName, string guildNameRegex)
    {
        if (guildName == null)
        {
            return false;
        }

        try
        {
            var customPattern = guildNameRegex;
            if (string.IsNullOrEmpty(customPattern))
            {
                customPattern = PATTERN_GUILDNAME;
            }

            return Regex.IsMatch(guildName.Trim(), customPattern);
        }
        catch (ArgumentException)
        {
            return Regex.IsMatch(guildName.Trim(), PATTERN_GUILDNAME);
        }
    }
}
