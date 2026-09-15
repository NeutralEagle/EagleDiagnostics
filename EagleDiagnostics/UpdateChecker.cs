namespace EagleDiagnostics
{
    public static class UpdateChecker
    {
        private const string LatestReleaseUrl =
            "https://github.com/NeutralEagle/EagleDiagnostics/releases/latest";

        public static async Task<string?> GetLatestVersionAsync()
        {
            try
            {
                using var handler = new HttpClientHandler
                {
                    AllowAutoRedirect = true
                };

                using var client = new HttpClient(handler)
                {
                    Timeout = TimeSpan.FromSeconds(3)
                };

                client.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "EagleDiagnostics-UpdateChecker");

                using HttpResponseMessage response = await client.GetAsync(
                    LatestReleaseUrl,
                    HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();

                Uri? finalUri = response.RequestMessage?.RequestUri;

                if (finalUri == null)
                    return null;

                // Example final URL:
                // https://github.com/NeutralEagle/EagleDiagnostics/releases/tag/v0.26.6.3.0

                const string marker = "/releases/tag/";

                string path = finalUri.AbsolutePath;

                int index = path.IndexOf(
                    marker,
                    StringComparison.OrdinalIgnoreCase);

                if (index < 0)
                    return null;

                string tag = path[(index + marker.Length)..];

                return tag.TrimStart('v', 'V');
            }
            catch
            {
                // Offline, timeout, GitHub unavailable, etc.
                return null;
            }
        }

        public static bool IsNewerVersion(
            string latestVersion,
            string currentVersion)
        {
            int[] latest = ParseVersion(latestVersion);
            int[] current = ParseVersion(currentVersion);

            int count = Math.Max(latest.Length, current.Length);

            for (int i = 0; i < count; i++)
            {
                int latestPart = i < latest.Length ? latest[i] : 0;
                int currentPart = i < current.Length ? current[i] : 0;

                if (latestPart > currentPart)
                    return true;

                if (latestPart < currentPart)
                    return false;
            }

            return false;
        }

        private static int[] ParseVersion(string version)
        {
            return version
                .Trim()
                .TrimStart('v', 'V')
                .Split('.')
                .Select(int.Parse)
                .ToArray();
        }
    }
}