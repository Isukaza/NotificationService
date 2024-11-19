using System.Net.Mail;

namespace Helpers;

public static class DataHelper
{
    private const int MaxLength = 255;

    #region GET

    /// <summary>
    /// Retrieves the configuration file name based on the current environment mode.
    /// </summary>
    /// <param name="isDevelopment">A boolean flag indicating whether the environment is development. 
    /// If <c>true</c>, it returns the configuration file for development, otherwise, it returns the default configuration file for other environments.</param>
    /// <returns>A string representing the configuration file name. 
    /// For development environment, it returns "appsettings.Development.json", and for other environments, it returns "appsettings.json".</returns>
    /// <example>
    /// Example usage:
    /// <code>
    /// bool isDevelopment = true;
    /// string configFile = DataHelper.GetConfigurationFileForMode(isDevelopment);
    /// Console.WriteLine(configFile); // Outputs "appsettings.Development.json"
    /// </code>
    /// </example>
    public static string GetConfigurationFileForMode(bool isDevelopment) =>
        isDevelopment
            ? "appsettings.Development.json"
            : "appsettings.json";

    #endregion

    #region Validators
    
    /// <summary>
    /// Validates and retrieves a required email configuration value.
    /// </summary>
    /// <param name="emailFromConfiguration">The configuration value to validate, expected to be a valid email address.</param>
    /// <param name="settingName">A friendly name for the setting (used in exception messages).</param>
    /// <returns>The validated email address from the configuration.</returns>
    /// <exception cref="ArgumentException">Thrown if the value is missing or invalid (not a valid email address).</exception>
    public static string GetRequiredEmail(string? emailFromConfiguration, string settingName)
    {
        if (string.IsNullOrWhiteSpace(emailFromConfiguration))
            throw new ArgumentException($"{settingName} is missing");
        
        try
        {
            _ = new MailAddress(emailFromConfiguration);
        }
        catch (FormatException)
        {
            throw new ArgumentException($"{settingName} is missing");
        }

        return emailFromConfiguration;
    }

    /// <summary>
    /// Validates and retrieves a required string setting, with optional length validation.
    /// </summary>
    /// <param name="valueFromConfiguration">The configuration value to validate.</param>
    /// <param name="settingName">A friendly name for the setting (used in exception messages).</param>
    /// <param name="minLength">Optional minimum length for the string.</param>
    /// <returns>The validated configuration value.</returns>
    /// <exception cref="ArgumentException">Thrown if the value is missing, invalid, or violates length constraints.</exception>
    public static string GetRequiredString(string? valueFromConfiguration, string settingName, int? minLength = null)
    {
        if (string.IsNullOrWhiteSpace(valueFromConfiguration))
            throw new ArgumentException($"{settingName} is missing");

        if (minLength.HasValue && valueFromConfiguration.Length < minLength.Value)
            throw new ArgumentException($"{settingName} is too short. Minimum length is {minLength.Value} characters.");

        if (valueFromConfiguration.Length > MaxLength)
            throw new ArgumentException($"{settingName} is too long. Maximum length is {MaxLength} characters.");

        return valueFromConfiguration;
    }

    /// <summary>
    /// Validates and retrieves a required integer setting within a specified range.
    /// </summary>
    /// <param name="valueFromConfiguration">The string value to convert to an integer.</param>
    /// <param name="settingName">A friendly name for the setting (used in exception messages).</param>
    /// <param name="min">The minimum acceptable value for the setting.</param>
    /// <param name="max">The maximum acceptable value for the setting.</param>
    /// <returns>The validated integer value.</returns>
    /// <exception cref="ArgumentException">Thrown if the value is missing, invalid, or out of range.</exception>
    public static int GetRequiredInt(string? valueFromConfiguration, string settingName, int min, int max)
    {
        if (string.IsNullOrWhiteSpace(valueFromConfiguration))
            throw new ArgumentException($"{settingName} is missing");

        if (!int.TryParse(valueFromConfiguration, out var result))
            throw new ArgumentException($"{settingName} is invalid. It must be a valid integer.");

        if (result < min)
            throw new ArgumentException($"{settingName} is too low. The minimum value is {min}.");

        if (result > max)
            throw new ArgumentException($"{settingName} is too high. The maximum value is {max}.");

        return result;
    }

    /// <summary>
    /// Validates and retrieves a required unsigned short (ushort) setting within a specified range.
    /// </summary>
    /// <param name="valueFromConfiguration">The string value to convert to a ushort.</param>
    /// <param name="settingName">A friendly name for the setting (used in exception messages).</param>
    /// <param name="min">The minimum acceptable value for the setting (must be between 0 and 65535).</param>
    /// <param name="max">The maximum acceptable value for the setting (must be between 0 and 65535).</param>
    /// <returns>The validated ushort value.</returns>
    /// <exception cref="ArgumentException">Thrown if the value is missing, invalid, or out of range.</exception>
    public static ushort GetRequiredUShort(string? valueFromConfiguration, string settingName, ushort min, ushort max)
    {
        if (string.IsNullOrWhiteSpace(valueFromConfiguration))
            throw new ArgumentException($"{settingName} is missing");

        if (!ushort.TryParse(valueFromConfiguration, out var result))
            throw new ArgumentException($"{settingName} is invalid. It must be a valid unsigned short (ushort).");

        if (result < min)
            throw new ArgumentException($"{settingName} is too low. The minimum value is {min}.");

        if (result > max)
            throw new ArgumentException($"{settingName} is too high. The maximum value is {max}.");

        return result;
    }

    #endregion
}