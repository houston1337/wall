using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GithubWall
{
    public partial class MainWindow : Window
    {
        private bool isMouseDown = false;
        private static Color notAvailableDay = Color.FromRgb(150, 150, 150);
        private static Color colorBG = (Color)ColorConverter.ConvertFromString("#FF0D1117");

        private static Color color0 = (Color)ColorConverter.ConvertFromString("#FF161B22");
        private static Color color1 = (Color)ColorConverter.ConvertFromString("#FF0E4429");
        private static Color color2 = (Color)ColorConverter.ConvertFromString("#FF006D32");
        private static Color color3 = (Color)ColorConverter.ConvertFromString("#FF26A641");
        private static Color color4 = (Color)ColorConverter.ConvertFromString("#FF39D353");

        private static SolidColorBrush brushColorNotAvailableDay = new SolidColorBrush(notAvailableDay);
        private static SolidColorBrush brushColorBG = new SolidColorBrush(colorBG);
        private static SolidColorBrush brushColor0 = new SolidColorBrush(color0);
        private static SolidColorBrush brushColor1 = new SolidColorBrush(color1);
        private static SolidColorBrush brushColor2 = new SolidColorBrush(color2);
        private static SolidColorBrush brushColor3 = new SolidColorBrush(color3);
        private static SolidColorBrush brushColor4 = new SolidColorBrush(color4);

        Dictionary<SolidColorBrush, int?> brushValues = new Dictionary<SolidColorBrush, int?>()
        {
            {brushColor0, 0},
            {brushColor1, 1},
            {brushColor2, 2},
            {brushColor3, 3},
            {brushColor4, 4},
        };

        private static DateTime currentDate = DateTime.Now;
        private static DateTime yearStartDayDate = new DateTime(currentDate.Year, 1, 1);
        int currDayOfYear = currentDate.DayOfYear;


        private SolidColorBrush currentColor = brushColor4;
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("yearStartDayDate.DayOfWeek = " + (int)yearStartDayDate.DayOfWeek);
            //заполняем столбец 1 недели днями предыдущего года 
            //будут безымянными и = цвету фона
            for (int i = 0; i < (int)yearStartDayDate.DayOfWeek; i++)
            {

                Rectangle newRec = new Rectangle();
                newRec.Width = 10;
                newRec.Height = 10;
                newRec.Fill = brushColorBG;
                newRec.RadiusX = 2;
                newRec.RadiusY = 2;
                newRec.MouseDown += MainWindow_MouseDown;
                newRec.MouseUp += MainWindow_MouseUp;
                newRec.IsEnabled = false;
                myGrid.Children.Add(newRec);
            }
            int day = 1;
            //заполняем столбцы прошедших дней 
            //будут серого цвета и недоступны
            for (int i = (int)yearStartDayDate.DayOfWeek; i < currDayOfYear; i++, day++)
            {
                Rectangle newRec = new Rectangle();
                newRec.Name = "_" + day.ToString("D3");
                newRec.Width = 10;
                newRec.Height = 10;
                newRec.Fill = brushColorNotAvailableDay;
                newRec.RadiusX = 2;
                newRec.RadiusY = 2;
                newRec.MouseDown += MainWindow_MouseDown;
                newRec.MouseUp += MainWindow_MouseUp;
                newRec.IsEnabled = false;
                myGrid.Children.Add(newRec);
            }
            //заполняем все оставшиеся 
            //доступные для рисования дни
            for (int i = currDayOfYear; i <= (DateTime.IsLeapYear((int)currentDate.Year) ? 366 : 365); i++, day++)
            {
                Rectangle newRec = new Rectangle();
                newRec.Name = "_" + day.ToString("D3");
                newRec.Width = 10;
                newRec.Height = 10;
                newRec.Fill = brushColor0;
                newRec.RadiusX = 2;
                newRec.RadiusY = 2;
                newRec.MouseMove += Rectangle_MouseMove;
                newRec.MouseDown += MainWindow_MouseDown;
                newRec.MouseUp += MainWindow_MouseUp;
                myGrid.Children.Add(newRec);
            }
        }


        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isMouseDown = true;
            }
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Rectangle rec = (Rectangle)sender;
                rec.Fill = currentColor;
            }
        }

        private void MainWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isMouseDown = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            currentColor = brushColor0;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            currentColor = brushColor1;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            currentColor = brushColor2;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            currentColor = brushColor3;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            currentColor = brushColor4;
        }

        private void SaveToFile(object sender, RoutedEventArgs e)
        {

            try
            {   
                //обновляем словарь со значениями комитов
                brushValues[brushColor0] = myUpDownControl0.Value;
                brushValues[brushColor1] = myUpDownControl1.Value;
                brushValues[brushColor2] = myUpDownControl2.Value;
                brushValues[brushColor3] = myUpDownControl3.Value;
                brushValues[brushColor4] = myUpDownControl4.Value;

                string path = Directory.GetCurrentDirectory() + "\\my_git.txt";
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    writer.NewLine = "\n";
                    //записываем в файл значения количества комитов для каждого цвета
                    writer.WriteLine($@"color0={myUpDownControl0.Value}");
                    writer.WriteLine($@"color1={myUpDownControl1.Value}");
                    writer.WriteLine($@"color2={myUpDownControl2.Value}");
                    writer.WriteLine($@"color3={myUpDownControl3.Value}");
                    writer.WriteLine($@"color4={myUpDownControl4.Value}");
                    //цикл для каждого квадрата 
                    foreach (UIElement child in myGrid.Children)
                    {
                        if (child is Rectangle && child.IsEnabled)
                        {
                            Rectangle rec = (Rectangle)child;
                            //todo обработать отсутствующие ключи
                            //записываем в файл номер дня и колво комитов (в зависимости от цвета квадрата) 
                            writer.WriteLine(rec.Name.Remove(0, 1) + "=" + brushValues[(SolidColorBrush)rec.Fill].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The process failed: {0}", ex.ToString());
            }

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                //прочитать значение из файла
                string path = Directory.GetCurrentDirectory() + "\\my_git.txt";

                //поиск и установка значений цветов из файла
                string[] lines = File.ReadAllLines(path);
                string[] dayValues = lines[5..];

                foreach (string line in lines)
                {
                    // Разделение строки на ключ и значение
                    string[] parts = line.Split('=');

                    // Проверка соответствия ключа
                    if (parts.Length == 2 && parts[0].Trim() == "color0")
                    {
                        int foundValue = int.Parse(parts[1].Trim());
                        brushValues[brushColor0] = foundValue;
                        myUpDownControl0.Value = foundValue;
                        continue; // Выход после нахождения значения

                    }
                    if (parts.Length == 2 && parts[0].Trim() == "color1")
                    {
                        int foundValue = int.Parse(parts[1].Trim());
                        brushValues[brushColor1] = foundValue;
                        myUpDownControl1.Value = foundValue;
                        continue; // Выход после нахождения значения
                    }
                    if (parts.Length == 2 && parts[0].Trim() == "color2")
                    {
                        int foundValue = int.Parse(parts[1].Trim());
                        brushValues[brushColor2] = foundValue;
                        myUpDownControl2.Value = foundValue;
                        continue; // Выход после нахождения значения
                    }
                    if (parts.Length == 2 && parts[0].Trim() == "color3")
                    {
                        int foundValue = int.Parse(parts[1].Trim());
                        brushValues[brushColor3] = foundValue;
                        myUpDownControl3.Value = foundValue;
                        continue; // Выход после нахождения значения
                    }
                    if (parts.Length == 2 && parts[0].Trim() == "color4")
                    {
                        int foundValue = int.Parse(parts[1].Trim());
                        brushValues[brushColor4] = foundValue;
                        myUpDownControl4.Value = foundValue;
                        continue; // Выход после нахождения значения
                    }
                }

                Dictionary<string, int> valuesDictionary = new Dictionary<string, int>();
                //для каждого квадрата с именами соответветсвующеми id из файла 

                try
                {
                    // Обработка каждой строки
                    foreach (string dayValue in dayValues)
                    {
                        // Разделение строки на ключ и значение
                        // Добавление пары "ключ-значение" в словарь
                        string[] parts = dayValue.Split('=');
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim();
                            string value = parts[1].Trim();
                            valuesDictionary[key] = int.Parse(value);
                        }
                        else
                        {
                            Console.WriteLine($"Неверный формат строки: {dayValue}");
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }


                foreach (var kvp in valuesDictionary)
                {
                    string childElementName = "_"+kvp.Key;
                    int value = kvp.Value;

                    // Поиск дочернего элемента по имени
                    Rectangle? childElement = myGrid.Children?.Cast<Rectangle>()
                        .FirstOrDefault(element => element.Name == childElementName);

                    if (childElement != null)
                    {
                        childElement.Fill = brushValues.FirstOrDefault(x=>x.Value== kvp.Value).Key;
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in myGrid.Children)
            {
                if (child is Rectangle && child.IsEnabled)
                {
                    Rectangle rec = (Rectangle)child;
                    rec.Fill = brushColor0;
                }
            }
        }
    }
}
