using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace TAXILauncher;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();

    }

    /// <summary>
    /// OnCounterClicked is the main method used in the Launcher - it instructs the launcher to send the user input from the entry box and picker as arguments to the program, 
    /// that can be interpreted by the program to personalise the user experience
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnCounterClicked(object sender, EventArgs e)
	{
            ProcessStartInfo info = new ProcessStartInfo();
        ///File name for program launch
            info.FileName = @"C:\Users\Lauren\OneDrive\Documents\GitHub\Foundry-Phase1-Lauren\TAXI\bin\Release\net7.0\TAXI.exe";
        ///Setting user inputs from UserEntry and DifficultyPicker as arguments to be fed into the program when launched
            info.Arguments = UserEntry.Text + " " + DifficultyPicker.SelectedItem;
            Process.Start(info);

        SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

