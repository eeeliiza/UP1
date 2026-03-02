using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.Json;

namespace UP
{
    public partial class MainWindow : Window
    {
        private List<Student> students = new List<Student>();
        private const string FilePath = "students.json";

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            UpdateGrid();
        }

       
        public class Student
        {
            public string FullName { get; set; }
            public int Age { get; set; }
            public string GroupName { get; set; }
        }

        
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(AgeTextBox.Text) ||
                string.IsNullOrWhiteSpace(GroupTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (!int.TryParse(AgeTextBox.Text, out int age) || age <= 0)
            {
                MessageBox.Show("Введите корректный возраст!");
                return;
            }

            Student newStudent = new Student
            {
                FullName = NameTextBox.Text,
                Age = age,
                GroupName = GroupTextBox.Text
            };

            students.Add(newStudent);
            UpdateGrid();

            
            NameTextBox.Clear();
            AgeTextBox.Clear();
            GroupTextBox.Clear();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsGrid.SelectedItem is Student student)
            {
                students.Remove(student);
                UpdateGrid();
            }
            else
            {
                MessageBox.Show("Выберите студента для удаления!");
            }
        }

       
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
                MessageBox.Show("Данные сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

       
        private void LoadData()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    students = JsonSerializer.Deserialize<List<Student>>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

       
        private void UpdateGrid()
        {
            StudentsGrid.ItemsSource = null;
            StudentsGrid.ItemsSource = students;
        }
    }
}


