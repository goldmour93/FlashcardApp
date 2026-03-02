using System;
using System.Linq;
using System.Windows;
using FlashcardApp.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FlashcardApp.WpfUI
{
    public partial class TopicSelectionWindow : Window
    {
        private User _user = new() { Deck = [] };
        private readonly IServiceProvider _serviceProvider;

        public TopicSelectionWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        public void InitializeWithUser(User user)
        {
            _user = user;
            
            // Extract distinct topics from the user's deck
            var topics = _user.Deck
                .Select(c => c.Topic)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            if (topics.Count == 0)
            {
                topics.Add("General");
            }

            TopicComboBox.ItemsSource = topics;
            TopicComboBox.SelectedIndex = 0;
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedTopic = TopicComboBox.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedTopic))
            {
                MessageBox.Show("Please select a topic.");
                return;
            }

            StartButton.IsEnabled = false;

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            await mainWindow.InitializeWithUserAndTopicAsync(_user, selectedTopic);
            mainWindow.Show();

            this.Close();
        }
    }
}