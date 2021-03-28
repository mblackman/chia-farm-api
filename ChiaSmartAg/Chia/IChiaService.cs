namespace ChiaSmartAg.Chia
{
    /// <summary>
    /// A service for getting information about a current Chia instance.
    /// </summary>
    public interface IChiaService
    {
        /// <summary>
        /// Gets the current <see cref="GetFarmSummary"/>.
        /// </summary>
        /// <returns>The <see cref="FarmSummary"/> for this <see cref="IChiaService"/>.</returns>
        FarmSummary GetFarmSummary();
    }
}
