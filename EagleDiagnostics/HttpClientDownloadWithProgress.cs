namespace EagleDiagnostics;

public sealed class HttpClientDownloadWithProgress(string downloadUrl, string destinationFilePath) : IDisposable
{
    public delegate void ProgressChangedHandler(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage);

    public event ProgressChangedHandler? ProgressChanged;

    public async Task StartDownload()
    {
        using HttpClient client = new() { Timeout = TimeSpan.FromDays(1) };
        using HttpResponseMessage response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        long? totalBytes = response.Content.Headers.ContentLength;
        await using Stream source = await response.Content.ReadAsStreamAsync();
        await using FileStream destination = new(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

        long totalBytesRead = 0;
        long readCount = 0;
        byte[] buffer = new byte[8192];
        int bytesRead;
        while ((bytesRead = await source.ReadAsync(buffer)) > 0)
        {
            await destination.WriteAsync(buffer.AsMemory(0, bytesRead));
            totalBytesRead += bytesRead;
            readCount++;

            if (readCount % 100 == 0)
                RaiseProgressChanged(totalBytes, totalBytesRead);
        }

        RaiseProgressChanged(totalBytes, totalBytesRead);
    }

    private void RaiseProgressChanged(long? totalBytes, long downloadedBytes)
    {
        double? percentage = totalBytes.HasValue && totalBytes.Value > 0
            ? Math.Round((double)downloadedBytes / totalBytes.Value * 100, 2)
            : null;
        ProgressChanged?.Invoke(totalBytes, downloadedBytes, percentage);
    }

    public void Dispose() => GC.SuppressFinalize(this);
}
