using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab2
{
    public partial class MainPage : ContentPage
    {
        private string _xmlFilePath = string.Empty;
        private string _xslFilePath = string.Empty;
        private SearchFilter _currentFilter;
        private XmlAnalyzerContext _analyzerContext;
        private XmlAttributeExtractor _attributeExtractor;
        private HtmlTransformer _htmlTransformer;

        public MainPage()
        {
            InitializeComponent();

            _currentFilter = new SearchFilter();
            _analyzerContext = new XmlAnalyzerContext(new LinqParserStrategy());
            _attributeExtractor = new XmlAttributeExtractor();
            _htmlTransformer = new HtmlTransformer();
        }

        private async void OnSelectXmlFileClicked(object sender, EventArgs e)
        {
            try
            {
                var xmlFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".xml" } },
                    { DevicePlatform.MacCatalyst, new[] { "xml" } }
                });

                var pickOptions = new PickOptions
                {
                    PickerTitle = "Оберіть XML-файл архіву факультету",
                    FileTypes = xmlFileType
                };

                FileResult? result = await FilePicker.Default.PickAsync(pickOptions);
                if (result != null)
                {
                    _xmlFilePath = result.FullPath;
                    FilePathLabel.Text = _xmlFilePath;
                    FilePathLabel.FontAttributes = FontAttributes.None;

                    string? directory = Path.GetDirectoryName(_xmlFilePath);
                    if (!string.IsNullOrEmpty(directory))
                    {
                        string candidateXsl = Path.Combine(directory, "style.xsl");
                        if (File.Exists(candidateXsl))
                        {
                            _xslFilePath = candidateXsl;
                        }
                    }

                    LoadFilterAttributes(_xmlFilePath);
                    OutputEditor.Text = "XML-файл успішно завантажено. Списки фільтрів оновлено.";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка відкриття файлу", ex.Message, "OK");
            }
        }

        private void LoadFilterAttributes(string xmlPath)
        {
            Dictionary<string, List<string>> attributes = _attributeExtractor.ExtractUniqueValues(xmlPath);

            if (attributes.ContainsKey("Author"))
            {
                AuthorPicker.ItemsSource = attributes["Author"];
                AuthorPicker.SelectedIndex = 0;
            }

            if (attributes.ContainsKey("Faculty"))
            {
                FacultyPicker.ItemsSource = attributes["Faculty"];
                FacultyPicker.SelectedIndex = 0;
            }

            if (attributes.ContainsKey("Department"))
            {
                DepartmentPicker.ItemsSource = attributes["Department"];
                DepartmentPicker.SelectedIndex = 0;
            }

            if (attributes.ContainsKey("MaterialType"))
            {
                MaterialTypePicker.ItemsSource = attributes["MaterialType"];
                MaterialTypePicker.SelectedIndex = 0;
            }
        }

        private void OnStrategyChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!e.Value || _analyzerContext == null)
                return;

            if (sender is RadioButton radio && radio.Value != null)
            {
                string selected = radio.Value.ToString() ?? "LINQ";
                switch (selected)
                {
                    case "SAX":
                        _analyzerContext.SetStrategy(new SaxParserStrategy());
                        break;
                    case "DOM":
                        _analyzerContext.SetStrategy(new DomParserStrategy());
                        break;
                    case "LINQ":
                        _analyzerContext.SetStrategy(new LinqParserStrategy());
                        break;
                }
            }
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_xmlFilePath) || !File.Exists(_xmlFilePath))
            {
                await DisplayAlert("Увага", "Спочатку оберіть XML-файл архіву!", "OK");
                return;
            }

            try
            {
                _currentFilter.Author = AuthorPicker.SelectedItem?.ToString() ?? string.Empty;
                _currentFilter.Faculty = FacultyPicker.SelectedItem?.ToString() ?? string.Empty;
                _currentFilter.Department = DepartmentPicker.SelectedItem?.ToString() ?? string.Empty;
                _currentFilter.MaterialType = MaterialTypePicker.SelectedItem?.ToString() ?? string.Empty;
                _currentFilter.Keyword = KeywordEntry.Text ?? string.Empty;

                List<ArchiveMaterial> results = _analyzerContext.ExecuteSearch(_xmlFilePath, _currentFilter);
                DisplayResults(results);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка під час пошуку", ex.Message, "OK");
            }
        }

        private void DisplayResults(List<ArchiveMaterial> results)
        {
            ResultsCountLabel.Text = $"Результати пошуку (знайдено записів: {results.Count}):";

            if (results.Count == 0)
            {
                OutputEditor.Text = "За вказаними критеріями матеріалів не знайдено.";
                return;
            }

            var sb = new StringBuilder();
            foreach (ArchiveMaterial material in results)
            {
                sb.AppendLine(material.FormatForDisplay());
            }

            OutputEditor.Text = sb.ToString();
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            _currentFilter.Clear();

            if (AuthorPicker.ItemsSource != null && AuthorPicker.ItemsSource.Count > 0)
                AuthorPicker.SelectedIndex = 0;

            if (FacultyPicker.ItemsSource != null && FacultyPicker.ItemsSource.Count > 0)
                FacultyPicker.SelectedIndex = 0;

            if (DepartmentPicker.ItemsSource != null && DepartmentPicker.ItemsSource.Count > 0)
                DepartmentPicker.SelectedIndex = 0;

            if (MaterialTypePicker.ItemsSource != null && MaterialTypePicker.ItemsSource.Count > 0)
                MaterialTypePicker.SelectedIndex = 0;

            KeywordEntry.Text = string.Empty;
            ResultsCountLabel.Text = "Результати пошуку:";
            OutputEditor.Text = string.Empty;
        }

        private async void OnTransformToHtmlClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_xmlFilePath) || !File.Exists(_xmlFilePath))
            {
                await DisplayAlert("Увага", "Спочатку оберіть XML-файл архіву!", "OK");
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(_xslFilePath) || !File.Exists(_xslFilePath))
                {
                    var xslFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".xsl", ".xslt" } }
                    });

                    FileResult? xslResult = await FilePicker.Default.PickAsync(new PickOptions
                    {
                        PickerTitle = "Оберіть файл таблиці стилів (style.xsl)",
                        FileTypes = xslFileType
                    });

                    if (xslResult == null)
                        return;

                    _xslFilePath = xslResult.FullPath;
                }

                string? htmlOutputPath = null;

#if WINDOWS
                // Відкриваємо системне вікно збереження файлу Windows ("Зберегти як...")
                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
                savePicker.FileTypeChoices.Add("HTML Вебсторінка", new List<string>() { ".html" });
                savePicker.SuggestedFileName = "ArchiveReport";

                var window = Application.Current?.Windows[0]?.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
                if (window != null)
                {
                    IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);
                }

                Windows.Storage.StorageFile? file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    htmlOutputPath = file.Path;
                }
#else
                string defaultDir = Path.GetDirectoryName(_xmlFilePath) ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                htmlOutputPath = Path.Combine(defaultDir, "ArchiveReport.html");
#endif

                if (string.IsNullOrWhiteSpace(htmlOutputPath))
                    return;

                _htmlTransformer.TransformToHtml(_xmlFilePath, _xslFilePath, htmlOutputPath);

                await DisplayAlert("Успіх", $"HTML-звіт успішно збережено за шляхом:\n{htmlOutputPath}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка трансформації", ex.Message, "OK");
            }
        }

        private async void OnExitClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(
                "Підтвердження виходу",
                "Чи дійсно ви хочете вийти з програми?",
                "Так",
                "Ні");

            if (confirm)
            {
                Application.Current?.Quit();
            }
        }
    }
}