using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SetlistPlaylistCreator.Wpf.Authorization
{
    /// <summary>
    /// Interaction logic for AuthorizationUserControl.xaml
    /// </summary>
    public partial class AuthorizationUserControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationUserControl"/> class.
        /// </summary>
        public AuthorizationUserControl()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)
            {
                UseShellExecute = true
            });
            e.Handled = true;
        }
    }
}
