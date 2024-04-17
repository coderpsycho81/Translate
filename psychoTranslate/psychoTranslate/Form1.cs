using System;
using System.Windows.Forms;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;


namespace psychoTranslate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private ChromeDriver translateDriver;
        private Thread translateThread;
        private bool isActive = false;

        #region Chrome Creat
        private void chromeCreat()
        {
            // Options
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--headless");

            // Servics
            ChromeDriverService service = ChromeDriverService.CreateDefaultService(AppDomain.CurrentDomain.BaseDirectory);
            service.HideCommandPromptWindow = true;

            // Manager
            translateDriver = new ChromeDriver(service, options);
            this.Invoke((MethodInvoker)delegate
            {
                translateDriver.Navigate().GoToUrl("https://www.deepl.com/tr/translator#tr/en/" + richTextBox1.Text);
            });
            Thread.Sleep(500);

            try
            {
                IWebElement element = translateDriver.FindElement(By.XPath("//*[@id='textareasContainer']/div[3]/section/div[1]/d-textarea/div/p/span"));
                string text = element.Text;
                this.Invoke((MethodInvoker)delegate
                {
                    richTextBox2.Text = text;
                });
                
            }
            catch { 
            }

            isActive = true;
        }
        #endregion

        #region Başlat
        private void StartTranslateThread()
        {
            translateThread = new Thread(chromeCreat);
            translateThread.Start();
        }

        private void CheckIsActive()
        {
            while (true)
            {
                if (isActive)
                {

                    this.Invoke((MethodInvoker)delegate
                    {
                        this.Cursor = Cursors.Default;
                    });

                    break;
                }
                Thread.Sleep(1000);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        #endregion

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            StartTranslateThread();

            Thread checkIsActiveThread = new Thread(CheckIsActive);
            checkIsActiveThread.Start();
        }
    }
}
