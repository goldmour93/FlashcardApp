using System.Windows;
using System.Windows.Controls;
using FlashcardApp.Core.Facades;
using FlashcardApp.Core.Models;
using FlashcardApp.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FlashcardApp.WpfUI
{
    public partial class MainWindow : Window
    {
        private const int MinimumQuestionsPerTopic = 5;
        private readonly FlashcardEngineFacade _facade;
        private readonly UserService _userService;
        private readonly IServiceProvider _serviceProvider;
        private User _currentUser = new()
        {
            Deck = [],
            TopicXp = []
        };
        private string _selectedTopic = string.Empty;
        private Queue<Flashcard> _sessionCards = new();
        private Flashcard _currentCard = new();
        private StudySessionResult _sessionResult = new();

        public MainWindow(FlashcardEngineFacade facade, UserService userService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _facade = facade;
            _userService = userService;
            _serviceProvider = serviceProvider;
        }

        public async Task InitializeWithUserAsync(User user)
        {
            _currentUser = user;
            _selectedTopic = string.Empty;

            _currentUser.Deck ??= [];

            // Start Session
            int dailyLimit = _currentUser.DailyLimit;
            var cardsForSession = StudySessionService.GetCardsForSession(_currentUser.Deck, dailyLimit);
            _sessionCards = new Queue<Flashcard>(cardsForSession);
            _sessionResult = new StudySessionResult();

            UpdateStatsUI();
            await ShowNextCardAsync();
        }

        public async Task InitializeWithUserAndTopicAsync(User user, string topic)
        {
            _currentUser = user;
            await StartTopicSessionAsync(topic);
        }

        private async Task StartTopicSessionAsync(string topic)
        {
            _selectedTopic = topic;

            // Filter deck by selected topic
            var topicDeck = _currentUser.Deck.Where(c => c.Topic == _selectedTopic).ToList();

            _sessionCards = BuildSessionCards(topicDeck);
            _sessionResult = new StudySessionResult();

            UpdateStatsUI();
            await ShowNextCardAsync();
        }

        private Queue<Flashcard> BuildSessionCards(List<Flashcard> topicDeck)
        {
            int effectiveLimit = Math.Max(_currentUser.DailyLimit, MinimumQuestionsPerTopic);
            var cardsForSession = StudySessionService.GetCardsForSession(topicDeck, effectiveLimit).ToList();

            if (cardsForSession.Count == 0 && topicDeck.Count > 0)
            {
                cardsForSession = [.. topicDeck];
            }

            int index = 0;
            while (cardsForSession.Count < MinimumQuestionsPerTopic && topicDeck.Count > 0)
            {
                cardsForSession.Add(topicDeck[index % topicDeck.Count]);
                index++;
            }

            return new Queue<Flashcard>(cardsForSession);
        }

        private async Task ShowNextCardAsync()
        {
            if (_sessionCards.Count > 0)
            {
                _currentCard = _sessionCards.Dequeue();
                FrontText.Text = _currentCard.Front;
                BackText.Text = _currentCard.Back;

                // Reset UI state
                BackText.Visibility = Visibility.Hidden;
                ShowAnswerButton.Visibility = Visibility.Visible;
                RatingPanel.Visibility = Visibility.Hidden;
                return;
            }

            // Session Complete
            FrontText.Text = "Session Complete!";
            BackText.Text = $"You reviewed {_sessionResult.CardsReviewed} cards and gained {_sessionResult.XpGained} XP.";
            BackText.Visibility = Visibility.Visible;
            ShowAnswerButton.Visibility = Visibility.Hidden;
            RatingPanel.Visibility = Visibility.Hidden;

            if (!string.IsNullOrWhiteSpace(_selectedTopic))
            {
                var choice = MessageBox.Show(
                    "Session complete. Continue with the same topic?\nYes = continue, No = switch topic.",
                    "Continue or Switch",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (choice == MessageBoxResult.Yes)
                {
                    await StartTopicSessionAsync(_selectedTopic);
                }
                else
                {
                    var topicWindow = _serviceProvider.GetRequiredService<TopicSelectionWindow>();
                    topicWindow.InitializeWithUser(_currentUser);
                    topicWindow.Show();
                    Close();
                }
            }
        }

        private void ShowAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            BackText.Visibility = Visibility.Visible;
            ShowAnswerButton.Visibility = Visibility.Hidden;
            RatingPanel.Visibility = Visibility.Visible;
        }

        private async void RatingButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Tag.ToString(), out int rating))
            {
                // Review the card
                int xpGained = _facade.ReviewCard(_currentCard, rating);

                // Update user XP
                await _userService.AddXpToUserAsync(_currentUser, _currentCard.Topic, xpGained);

                // Update stats
                _sessionResult.CardsReviewed++;
                _sessionResult.XpGained += xpGained;
                UpdateStatsUI();

                // Move to next card
                await ShowNextCardAsync();
            }
        }

        private void UpdateStatsUI()
        {
            CardsReviewedText.Text = $"Cards Reviewed: {_sessionResult.CardsReviewed}";
            
            int currentTopicXp = 0;
            if (_currentUser.TopicXp != null && _currentUser.TopicXp.ContainsKey(_selectedTopic))
            {
                currentTopicXp = _currentUser.TopicXp[_selectedTopic];
            }

            XpGainedText.Text = $"Current Topic: {_selectedTopic} | Topic XP: {currentTopicXp} | Total XP: {_currentUser.TotalXp}";
        }
    }
}