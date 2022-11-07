using System;
using System.Collections.Generic;

using LetsMake;
using Semver;

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
                            if (SemVersion.TryParse(ServerSettings.ServerVersion, SemVersionStyles.AllowV, out SemVersion currentversion))
                            {
                                var Release = Releases?.Latest();
    
                                if (Release != null)
                                {
                                    if (SemVersion.TryParse(Release.TagName, SemVersionStyles.AllowV, out SemVersion latestrelease))
                                    {
                                        if(latestrelease.CompareSortOrderTo(currentversion) == 1)
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
