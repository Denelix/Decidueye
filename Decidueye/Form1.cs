using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using WindowsInput;
using WindowsInput.Native;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Decidueye
{
    public partial class Form1 : Form
    {
        const int VK_SHIFT = 0x10;
        public static bool isRunning = false;
        private Button startStopButton;
        private Label statusLabel;
        private Button exitButton;
        private static readonly InputSimulator inputSimulator = new InputSimulator();

        // Import the necessary Win32 functions
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern void SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        // Define the necessary Win32 constants
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        public Form1()
        {
            InitializeComponent();
            this.TopMost = true;
            // Create and add Start/Stop button
            startStopButton = new Button();
            startStopButton.Size = new Size(200, 80);
            startStopButton.Location = new Point((this.ClientSize.Width - startStopButton.Size.Width) / 2, (this.ClientSize.Height - startStopButton.Size.Height) / 2);
            startStopButton.BackColor = Color.Red;
            startStopButton.Click += StartStopButton_Click;
            this.Controls.Add(startStopButton);

            // Create and add status label
            statusLabel = new Label();
            statusLabel.AutoSize = false;
            statusLabel.Size = new Size(200, 30);
            statusLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            statusLabel.ForeColor = Color.Black;
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            statusLabel.Location = new Point((this.ClientSize.Width - statusLabel.Width) / 2, startStopButton.Location.Y - 60);
            this.Controls.Add(statusLabel);

            // Set form properties
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Set form border style to fixed single
            this.MaximizeBox = false; // Remove maximize button
            this.MinimizeBox = false; // Remove minimize button
            this.ControlBox = true; // Show control box (title bar buttons)
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize status label text
            UpdateStatusLabel();
        }

        private void StartStopButton_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                // Stop operation
                isRunning = false;
                startStopButton.BackColor = Color.Red;
                UpdateStatusLabel();
                // TODO: Add code to stop operation
            }
            else
            {
                // Start operation
                isRunning = true;
                startStopButton.BackColor = Color.Blue;
                UpdateStatusLabel();
                // TODO: Add code to start operation
            }
        }

        private void StartStopButton_Click()
        {
            if (isRunning)
            {
                // Stop operation
                isRunning = false;
                startStopButton.BackColor = Color.Red;
                UpdateStatusLabel();
                // TODO: Add code to stop operation
            }
            else
            {
                // Start operation
                isRunning = true;
                startStopButton.BackColor = Color.Blue;
                UpdateStatusLabel();
                // TODO: Add code to start operation
            }
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            // Close the form
            this.Close();
        }

        private void UpdateStatusLabel()
        {
            statusLabel.Text = isRunning ? "ON" : "OFF";
            statusLabel.Location = new Point((this.ClientSize.Width - statusLabel.Width) / 2, startStopButton.Location.Y - 60);
        }
        // Override the CreateParams property to remove the minimize and maximize buttons and set the form style to fixed single
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~0x00020000; // WS_MINIMIZEBOX
                cp.Style &= ~0x00010000; // WS_MAXIMIZEBOX
                cp.Style &= ~0x00040000; // WS_SIZEBOX
                return cp;
            }
        }

        // Override the WndProc method to enable the form to be dragged by clicking and dragging anywhere on it
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCLBUTTONDOWN:
                    if (m.WParam.ToInt32() == HT_CAPTION)
                    {
                        ReleaseCapture();
                        SendMessage(this.Handle, 0x112, 0xf012, 0);
                    }
                    break;
            }

            base.WndProc(ref m);
        }
        private bool held;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Methods.IsKeyDown(0x21) && held) //if pageup is not down then: set held to false and toggle the stopstartbutton visual.
            {
                held = false;
                StartStopButton_Click();
            }
            else if (Methods.IsKeyDown(0x21) && !held) //if pageup is held down and held is false then: set held to true 
            {
                held = true;
                isRunning = !isRunning;
                UpdateStatusLabel();
                StartStopButton_Click();
            }
            else if (isRunning && Methods.IsKeyDown(0x41))
            {

                // Call the method you want to measure
                Gliding.autoShift();
            }
            else
            {
                //checks if Shift is already held down and if it is it will let it go.
                if (Variables.alreadyDown)
                {
                    inputSimulator.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
                    Variables.alreadyDown = false;
                }
            }
        }
    }
    //Idea: count clicks after right clicking on an enemy.
}