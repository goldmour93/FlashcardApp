﻿using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FlashcardApp.Core.Interfaces;
using FlashcardApp.Core.Repositories;
using FlashcardApp.Core.Services;
using FlashcardApp.Core.Facades;
using FSRS.Core.Interfaces;
using FSRS.Core.Services;

namespace FlashcardApp.WpfUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                var dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FlashcardApp");
                var filePath = Path.Combine(dataDir, "users.json");
                services.AddSingleton<IUserRepository>(new JsonFileUserRepository(filePath));

                // Services
                services.AddSingleton<UserService>();
                services.AddSingleton<IScheduler, Scheduler>();

                // Facades
                services.AddSingleton<FlashcardEngineFacade>();

                // Windows
                services.AddTransient<LoginWindow>();
                services.AddTransient<TopicSelectionWindow>();
                services.AddTransient<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var loginWindow = AppHost.Services.GetRequiredService<LoginWindow>();
        loginWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}
