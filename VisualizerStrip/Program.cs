using System;
using System.Windows.Forms;
using VisualizerStrip;
using VisualizerStrip.Properties;
using VisualizerStrip.Visualization;

namespace WinformsVisualization
{
    static class Program
    {
        private static TrayAppContext context;

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);

            context = new TrayAppContext();
            Application.Run(context);
        }

        public static TrayAppContext GetContext()
        {
            return context;
        }
    }

    public class TrayAppContext : ApplicationContext
    {
        private static SpectrumService service;
        private NotifyIcon trayIcon;

        public TrayAppContext()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.Default,
                ContextMenuStrip = new ContextMenuStrip(){
                    Items = {
                        new ToolStripMenuItem("Exit", null, Exit)
                    }
                },
                Visible = true
            };
            trayIcon.Click += OnClick;

            InitializeService();
        }

        private void OnClick(object sender, EventArgs e)
        {
            Form form = new Form();
            form.Show();
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;
            if (service != null)
            {
                service.Stop();
            }

            Application.Exit();
            Environment.Exit(0);
        }

        void InitializeService()
        {
            if (service == null)
            {
                service = new SpectrumService();
            }

            service.AddCallback(OnCallback);
            service.Start();
        }

        public SpectrumService GetSpectrumService()
        {
            return service;
        }

        private void OnCallback(double intensity)
        {
            WSClient.Instance.SendMessage(intensity.ToString());
        }
    }
}
