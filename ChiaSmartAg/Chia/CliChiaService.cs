using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace ChiaSmartAg.Chia
{
    /// <summary>
    /// Cross platform command-line service to interact with Chia farms.
    /// </summary>
    public class CliChiaService : IChiaService
    {
        private readonly string chiaPath;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Creates a new instance of <see cref="CliChiaService"/>.
        /// </summary>
        /// <param name="configuration">The current configuration.</param>
        public CliChiaService(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var applicationPath = configuration.GetValue<string>("ExePath");

            if (applicationPath != null)
            {
                if (File.Exists(applicationPath))
                {
                    chiaPath = applicationPath;
                }
                else
                {
                    throw new InvalidOperationException("Invalid configuration. Could not find Chia EXE path: " + applicationPath);
                }
            }
            else
            {
                throw new InvalidOperationException("Chia application path must be set.");
            }
        }

        /// <summary>
        /// Gets the current <see cref="GetFarmSummary"/>.
        /// </summary>
        /// <returns>The <see cref="FarmSummary"/> for this <see cref="IChiaService"/>.</returns>
        public FarmSummary GetFarmSummary()
        {
            var farmSummary = new FarmSummary();
            string summaryCommandResults = RunCommand("farm summary");
            string[] lines = summaryCommandResults.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> parameters = lines.Select(l => l.Split(':')).Where(l => l.Length == 2).ToDictionary(k => k[0].Trim().ToLower(), v => v[1].Trim().ToLower());

            foreach (var parameter in parameters)
            {
                switch (parameter.Key)
                {
                    case "farming status":

                        break;

                    case "total chia farmed":
                        if (float.TryParse(parameter.Value, out float totalChiaFarmed))
                        {
                            farmSummary.TotalChiaFarmed = totalChiaFarmed;
                        }
                        break;

                    case "user transaction fees":
                        if (float.TryParse(parameter.Value, out float userTransactionFees))
                        {
                            farmSummary.UserTransactionFees = userTransactionFees;
                        }
                        break;

                    case "block rewards":
                        if (float.TryParse(parameter.Value, out float blockRewards))
                        {
                            farmSummary.BlockRewards = blockRewards;
                        }
                        break;

                    case "last height farmed":
                        if (int.TryParse(parameter.Value, out int lastHeightFarmed))
                        {
                            farmSummary.LastHeightFarmed = lastHeightFarmed;
                        }
                        break;

                    case "plot count":
                        if (int.TryParse(parameter.Value, out int plotCount))
                        {
                            farmSummary.PlotCount = plotCount;
                        }
                        break;

                    case "total size of plots":
                        if (Utils.TryParseStringToGiBs(parameter.Value, out float totalSizeOfPlots))
                        {
                            farmSummary.TotalSizeOfPlotsGiB = totalSizeOfPlots;
                        }
                        break;

                    case "estimated network space":
                        if (Utils.TryParseStringToGiBs(parameter.Value, out float estimatedNetworkSpace))
                        {
                            farmSummary.EstimatedNetworkSpaceGiB = estimatedNetworkSpace;
                        }
                        break;

                    case "expected time to win":
                        farmSummary.ExpectedTimeToWin = parameter.Value;
                        break;
                }
            }

            return farmSummary;
        }

        // Run the Chia command line with the given arguments.
        private string RunCommand(string arguments)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = chiaPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }
    }
}
