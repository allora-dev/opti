using Opti.Bench;

namespace Opti.App;

/// <summary>
/// Точка входа. «Opti.exe --bench …» запускает встроенный синтетический тест без окна приложения —
/// так весь Opti остаётся одним портативным файлом.
/// </summary>
public static class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
        if (BenchProgram.IsBenchInvocation(args)) return BenchProgram.Run(args);
        // Один экземпляр: два процесса, меняющие систему одновременно, могут перепутать исходные значения для отката.
        using var instance = Opti.Workflow.SingleInstance.TryAcquire(TimeSpan.FromSeconds(3));
        if (instance is null)
        {
            System.Windows.MessageBox.Show("Opti уже запущен (окно или командная строка). Закройте его и попробуйте снова.", "Opti",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            return 3;
        }
        var app = new App();
        app.InitializeComponent();
        // Тема Fluent по системной (светлая/тёмная, акцентный цвет, тёмный заголовок окна).
        // Задаётся после загрузки ресурсов приложения, чтобы словарь темы не был перезаписан.
        app.ThemeMode = System.Windows.ThemeMode.System;
        return app.Run();
    }
}
