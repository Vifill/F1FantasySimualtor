using PuppeteerSharp;

internal class Program
{
    private static async Task Main(string[] args)
    {
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

        await using var page = await browser.NewPageAsync();
        await page.GoToAsync("https://account.formula1.com/#/en/login");
        await page.EvaluateExpressionHandleAsync("document.fonts.ready"); // Wait for fonts to be loaded. Omitting this might result in no text rendered in pdf.
        await page.PdfAsync("test.pdf");
        await page.ClickAsync("")
    }
}