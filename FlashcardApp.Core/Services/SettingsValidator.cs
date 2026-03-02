

namespace FlashcardApp.Core.Services
{
    /// <summary>
    /// Service responsible for validating application settings and configurations.
    /// </summary>
    public static class SettingsValidator
    {
        /// <summary>
        /// Validates the desired retention rate for the FSRS algorithm.
        /// </summary>
        /// <param name="desiredRetention">The target retention rate (e.g., 0.90 for 90%).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the desired retention is outside the valid bounds of 0.70 to 0.99.</exception>
        public static void ValidateDesiredRetention(double desiredRetention)
        {
            // FSRS requires a target retention rate strictly between 0.70 (70%) and 0.99 (99%).
            if (desiredRetention < 0.70 || desiredRetention > 0.99)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(desiredRetention), 
                    "Desired retention rate must be strictly between 0.70 and 0.99.");
            }
        }
    }
}