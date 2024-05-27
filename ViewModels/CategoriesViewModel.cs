using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Supermarket.Models.BusinessLayerLogic;
using WPF_Supermarket.Models;
using Wpf_Supermarket.MVVM;
using System.Windows;

namespace WPF_Supermarket.ViewModels
{
    public class CategoriesViewModel : ViewModelBase
    {
        private readonly CategoriesBLL _categoriesBLL;
        private ObservableCollection<Category> _categories;
        private Category _selectedCategory;
        private Category _currentCategory;
        private bool _isCategorySelected;

        public RelayCommand AddCategoryCommand { get; private set; }
        public RelayCommand EditCategoryCommand { get; private set; }
        public RelayCommand DeleteCategoryCommand { get; private set; }

        public CategoriesViewModel()
        {
            _categoriesBLL = new CategoriesBLL();
            _categories = new ObservableCollection<Category>();
            _currentCategory = new Category();
            LoadCategories();

            AddCategoryCommand = new RelayCommand(_ => AddCategory(), _ => !IsCategorySelected);
            EditCategoryCommand = new RelayCommand(_ => EditCategory(), _ => IsCategorySelected);
            DeleteCategoryCommand = new RelayCommand(_ => DeleteCategory(), _ => IsCategorySelected);
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();

                if (_selectedCategory != null)
                {
                    CurrentCategory = new Category
                    {
                        Id = _selectedCategory.Id,
                        Name = _selectedCategory.Name
                    };
                    IsCategorySelected = true;
                }
                else
                {
                    CurrentCategory = new Category();
                    IsCategorySelected = false;
                }

                UpdateCommandStates();
            }
        }

        public Category CurrentCategory
        {
            get => _currentCategory;
            set
            {
                _currentCategory = value;
                OnPropertyChanged();
            }
        }

        public bool IsCategorySelected
        {
            get => _isCategorySelected;
            set
            {
                _isCategorySelected = value;
                OnPropertyChanged();
                UpdateCommandStates();
            }
        }

        private void LoadCategories()
        {
            var categories = _categoriesBLL.GetAllCategories();
            Categories.Clear();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

        private void AddCategory()
        {
            if(ValidateCategory(CurrentCategory))
            {
                _categoriesBLL.AddCategory(CurrentCategory);
                LoadCategories();
                CurrentCategory = new Category();
            }
        }

        private void EditCategory()
        {
            if (SelectedCategory != null)
            {
                _categoriesBLL.UpdateCategory(CurrentCategory);
                LoadCategories();
                CurrentCategory = new Category();
                IsCategorySelected = false;
            }
        }

        private void DeleteCategory()
        {
            if (SelectedCategory != null && ValidateCategory(CurrentCategory))
            {
                _categoriesBLL.DeleteCategory(SelectedCategory.Id);
                LoadCategories();
                CurrentCategory = new Category();
                IsCategorySelected = false;
            }
        }

        private bool ValidateCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                MessageBox.Show("Category name must be filled out.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void UpdateCommandStates()
        {
            AddCategoryCommand.RaiseCanExecuteChanged();
            EditCategoryCommand.RaiseCanExecuteChanged();
            DeleteCategoryCommand.RaiseCanExecuteChanged();
        }
    }
}
