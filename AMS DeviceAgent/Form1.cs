using Microsoft.Extensions.Configuration;
using System.Drawing.Drawing2D;

namespace AMS_DeviceAgent
{
    public partial class Form1 : Form
    {
        private LocalAgentServer? _localServer;
        private IConfiguration? _configuration;
        private Button btnRequestActivation = null!;
        private Label lblTitle = null!;
        private Label lblSubtitle = null!;
        private Label lblStatus = null!;
        private Label lblMachineName = null!;
        private TextBox txtMachineKey = null!;
        private Button btnCheck = null!;
        private Button btnCopy = null!;

        public Form1()
        {
            InitializeComponent();
            BuildUi();

            this.Load += Form1_Load;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _localServer = new LocalAgentServer(_configuration);
            _localServer.Start();

            await CheckLicense();
        }

        private void BuildUi()
        {
            Text = "AMS Device Agent";
            Width = 720;
            Height = 430;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(18, 22, 35);
            Font = new Font("Segoe UI", 10);

            var panel = new Panel
            {
                Left = 30,
                Top = 30,
                Width = 640,
                Height = 330,
                BackColor = Color.FromArgb(28, 34, 52)
            };

            Controls.Add(panel);

            lblTitle = new Label
            {
                Text = "AMS Device Agent",
                Left = 25,
                Top = 22,
                Width = 500,
                Height = 32,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18, FontStyle.Bold)
            };

            lblSubtitle = new Label
            {
                Text = "SafeStock Licence i device agent.",
                Left = 25,
                Top = 58,
                Width = 560,
                Height = 25,
                ForeColor = Color.FromArgb(170, 178, 196)
            };

            lblStatus = new Label
            {
                Text = "Provjera licence...",
                Left = 25,
                Top = 105,
                Width = 560,
                Height = 34,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13, FontStyle.Bold)
            };

            lblMachineName = new Label
            {
                Text = $"Računar: {Environment.MachineName}",
                Left = 25,
                Top = 155,
                Width = 560,
                Height = 24,
                ForeColor = Color.FromArgb(210, 215, 230)
            };

            var lblKey = new Label
            {
                Text = "Machine key:",
                Left = 25,
                Top = 190,
                Width = 200,
                Height = 24,
                ForeColor = Color.FromArgb(170, 178, 196)
            };

            txtMachineKey = new TextBox
            {
                Left = 25,
                Top = 218,
                Width = 585,
                Height = 32,
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(20, 24, 36),
                ForeColor = Color.White
            };

            btnCheck = new Button
            {
                Text = "Provjeri licencu",
                Left = 25,
                Top = 275,
                Width = 160,
                Height = 38,
                BackColor = Color.FromArgb(75, 112, 245),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCheck.FlatAppearance.BorderSize = 0;
            btnCheck.Click += async (_, _) => await CheckLicense();

            btnCopy = new Button
            {
                Text = "Kopiraj MachineKey",
                Left = 200,
                Top = 275,
                Width = 180,
                Height = 38,
                BackColor = Color.FromArgb(42, 49, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCopy.FlatAppearance.BorderSize = 0;
            btnCopy.Click += (_, _) =>
            {
                if (!string.IsNullOrWhiteSpace(txtMachineKey.Text))
                    Clipboard.SetText(txtMachineKey.Text);
            };
            btnRequestActivation = new Button
            {
                Text = "Zatraži aktivaciju",
                Left = 395,
                Top = 275,
                Width = 180,
                Height = 38,
                BackColor = Color.FromArgb(58, 160, 110),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnRequestActivation.FlatAppearance.BorderSize = 0;
            btnRequestActivation.Click += async (_, _) => await RequestActivation();

            panel.Controls.Add(btnRequestActivation);
            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblSubtitle);
            panel.Controls.Add(lblStatus);
            panel.Controls.Add(lblMachineName);
            panel.Controls.Add(lblKey);
            panel.Controls.Add(txtMachineKey);
            panel.Controls.Add(btnCheck);
            panel.Controls.Add(btnCopy);
        }
        private async Task RequestActivation()
        {
            if (_configuration == null)
                return;

            btnRequestActivation.Enabled = false;

            var client = new DeviceLicenseClient(_configuration);

            var result = await client.RequestActivationAsync(
                requestedBy: Environment.UserName,
                note: "Activation requested from AMS DeviceAgent");

            txtMachineKey.Text = result.MachineKey;

            if (result.Success)
            {
                MessageBox.Show(
                    "Zahtjev za aktivaciju je poslan administratoru.",
                    "AMS Device Agent",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    $"Zahtjev nije poslan. Status: {result.Status}",
                    "AMS Device Agent",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            btnRequestActivation.Enabled = true;
        }
        private async Task CheckLicense()
        {
            if (_configuration == null)
                return;

            btnCheck.Enabled = false;
            lblStatus.Text = "Provjera licence...";
            lblStatus.ForeColor = Color.White;

            var client = new DeviceLicenseClient(_configuration);
            var result = await client.CheckLicenseAsync();

            txtMachineKey.Text = result.MachineKey;

            if (result.Licensed)
            {
                lblStatus.Text = $"Licenca aktivna ({result.Status})";
                lblStatus.ForeColor = Color.FromArgb(58, 210, 120);
            }
            else
            {
                lblStatus.Text = $"Licenca nije aktivna ({result.Status})";
                lblStatus.ForeColor = Color.FromArgb(255, 105, 105);
            }

            btnCheck.Enabled = true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _localServer?.Stop();
            base.OnFormClosing(e);
        }
    }
}