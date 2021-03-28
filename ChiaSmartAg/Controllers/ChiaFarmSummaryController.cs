using System;

using ChiaSmartAg.Chia;

using Microsoft.AspNetCore.Mvc;

namespace ChiaSmartAg.Controllers
{
    /// <summary>
    /// Controller for getting farm summary information.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ChiaFarmSummaryController : ControllerBase
    {
        private readonly IChiaService chiaFarm;

        /// <summary>
        /// Creates a new instance of <see cref="ChiaFarmSummaryController"/>.
        /// </summary>
        /// <param name="chiaFarm">The <see cref="IChiaService"/> to get farm information.</param>
        public ChiaFarmSummaryController(IChiaService chiaFarm)
        {
            this.chiaFarm = chiaFarm ?? throw new ArgumentNullException(nameof(chiaFarm));
        }

        /// <summary>
        /// Gets the current summary of the Chia farm.
        /// </summary>
        /// <returns>The current summary of the farm.</returns>
        [HttpGet]
        public FarmSummary Get() => chiaFarm.GetFarmSummary();
    }
}
