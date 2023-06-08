// ErrorMessageDisplay.cs

using System.Windows.Forms;

namespace HomeBase
{
    public class ErrorMessageDisplay
    {
        public static void ShowError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
