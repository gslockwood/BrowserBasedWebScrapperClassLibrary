
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrowserBasedWebScrapperClassLibrary
{
    public class BrowserBasedWebScraper
    {
        public static Task<string> LoadUrl(string url)
        {
            var tcs = new TaskCompletionSource<string>();
            Thread thread = new Thread(() =>
            {
                try
                {
                    Func<string> f = () =>
                    {
                        using (WebBrowser browser = new WebBrowser())
                        {
                            browser.ScriptErrorsSuppressed = true;
                            browser.Navigate(url);
                            //int maxCount = 1500;
                            int counter = 0;
                            //while (browser.ReadyState != System.Windows.Forms.WebBrowserReadyState.Complete && counter < maxCount)
                            while (browser.ReadyState != WebBrowserReadyState.Complete)
                            {
                                System.Windows.Forms.Application.DoEvents();
                                counter++;
                            }
                            return browser.DocumentText;
                        }
                    };
                    tcs.SetResult(f());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
            return tcs.Task;
        }
    }

}