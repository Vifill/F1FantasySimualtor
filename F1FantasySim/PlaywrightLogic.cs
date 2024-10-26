using Microsoft.Playwright;

namespace F1FantasySim
{
    public class PlaywrightLogic
    {
        public async Task RunAsync()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://playwright.dev");
            await page.ScreenshotAsync(new PageScreenshotOptions { Path = "screenshot.png" });





        }




    }
}
