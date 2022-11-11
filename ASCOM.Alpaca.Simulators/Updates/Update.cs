using System;
using System.Collections.Generic;

using LetsMake;

namespace ASCOM.Alpaca.Simulators
{
    internal class Update
    {
        internal static IReadOnlyList<Octokit.Release> Releases = null;

        internal async static void CheckForUpdates()
        {
            try
            {
                Releases = await LetsMake.GitHubReleases.GetReleases("ASCOMInitiative", "ASCOM.Alpaca.Simulators");
            }
            catch(Exception ex)
            {
                Logging.LogError(ex.Message);
            }
        }

        internal static bool UpdateAvailable
        {
            get
            {
                try
                {
                    if (Releases != null)
                    {
                        if (Releases.Count > 0)
                        {
                            if (SemanticVersion.TryParse(ServerSettings.ServerVersion, out SemanticVersion currentversion))
                            {
                                var Release = Releases?.Latest();
    
                                if (Release != null)
                                {
                                    if (SemanticVersion.TryParse(Release.TagName, out SemanticVersion latestrelease))
                                    {
                                        if(latestrelease > currentversion)
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Logging.LogError($"Failed to check for new version, {ex.Message}");
                }
                return false;
            }
        }
    }
}
