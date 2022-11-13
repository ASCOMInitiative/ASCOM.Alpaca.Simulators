using LetsMake;
using Octokit;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ASCOM.Alpaca.Simulators
{
    internal class GithubUpdateChecker
    {
        internal SemanticVersion CurrentVersion
        {
            get;
            private set;
        }

        internal string RemoteRepositoryName
        {
            get;
            private set;
        }

        internal string RemoteRepositoryOwner
        {
            get;
            private set;
        }

        internal IReadOnlyList<Octokit.Release> Releases
        {
            get;
            private set;
        } = null;

        internal GithubUpdateChecker(SemanticVersion currentRunningVersion, string remoteRepositoryOwner, string remoteRepositoryName)
        {
            CurrentVersion = currentRunningVersion;
            RemoteRepositoryName = remoteRepositoryName;
            RemoteRepositoryOwner = remoteRepositoryOwner;
        }

        internal async void CheckForUpdates()
        {
            Releases = await GitHubReleases.GetReleases(RemoteRepositoryOwner, RemoteRepositoryName);
        }

        internal bool UpdateAvailable
        {
            get
            {
                return HasNewerRelease || HasNewerPrerelease;
            }
        }

        internal Octokit.Release LatestRelease
        {
            get
            {
                return Releases?.LatestRelease();
            }
        }

        internal Octokit.Release LatestPrerelease
        {
            get
            {
                return Releases?.LatestPrerelease();
            }
        }

        internal bool HasNewerRelease
        {
            get
            {
                return (LatestRelease?.ReleaseSemVersionFromTag() ?? new SemanticVersion(0)) > CurrentVersion;
            }
        }

        internal bool HasNewerPrerelease
        {
            get
            {
                if((LatestPrerelease?.ReleaseSemVersionFromTag() ?? new SemanticVersion(0)) > (LatestRelease?.ReleaseSemVersionFromTag() ?? new SemanticVersion(0)))
                {
                    return (LatestPrerelease?.ReleaseSemVersionFromTag() ?? new SemanticVersion(0)) > CurrentVersion;
                }
                return false;
            }
        }
    }
}