using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using Warehouse.BLL;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Warehouse.Models;
using System.Windows;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Warehouse.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly Service _service = new();
    public ObservableCollection<Product> Products { get; } = [];
    public ObservableCollection<string> ProductTypes { get; } = [];
    public ObservableCollection<string> Suppliers { get; } = [];

    [Reactive] private Product? _selectedProduct;

    [Reactive] private string? _searchProduct;

    [Reactive] private int? _id;
    [Reactive] private string? _name;
    [Reactive] private string? _type;
    [Reactive] private string? _supplier;
    [Reactive] private decimal? _price;

    private readonly IObservable<bool> _canExecuteDelete;
    private readonly IObservable<bool> _canExecuteSave;
    private readonly IObservable<bool> _canExecuteClear;

    public MainWindowViewModel()
    {
        this.WhenAnyValue(vm => vm.SelectedProduct)
            .WhereNotNull()
            .Subscribe(p =>
            {
                Id = p.Id;
                Name = p.Name;
                Type = p.ProductTypeName;
                Supplier = p.SupplierName;
                Price = p.CostPrice;
            });

        this.WhenAnyValue(vm => vm.SearchProduct)
            .WhereNotNull()
            .Subscribe(t =>
            {
                var products = _service.GetByName(t);

                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            });

        _canExecuteDelete = this.WhenAnyValue(
            wm => wm.SelectedProduct,
            vm => vm.Id,
            (p1, p2) => p1 is not null ||
                        p2 is not null);

        _canExecuteSave = this.WhenAnyValue(
            vm => vm.Name,
            vm => vm.Price,
            vm => vm.Supplier,
            vm => vm.Type,
            (n, p, s, t) =>
            {
                return !string.IsNullOrWhiteSpace(n) &&
                        p is not null &&
                        !string.IsNullOrWhiteSpace(s) &&
                        !string.IsNullOrWhiteSpace(t);
            });

        _canExecuteClear = this.WhenAnyValue(
            vm => vm.Name,
            vm => vm.Price,
            (n, p) => !string.IsNullOrWhiteSpace(n) ||
                      p is not null);
    }

    [ReactiveCommand]
    private void Load()
    {
        var products = _service.GetAll();

        Products.Clear();
        foreach (var product in products)
        {
            Products.Add(product);
        }

        ProductTypes.Clear();
        var types = _service.GetAllTypes();
        foreach (var type in types)
        {
            ProductTypes.Add(type);
        }

        Suppliers.Clear();
        var suppliers = _service.GetAllSuppliers();
        foreach (var supplier in suppliers)
        {
            Suppliers.Add(supplier);
        }
    }

    [ReactiveCommand(CanExecute = nameof(_canExecuteClear))]
    private void Clear()
    {
        SelectedProduct = null;

        Id = null;
        Name = null;
        Price = null;
        Type = null;
        Supplier = null;
    }

    [ReactiveCommand(CanExecute = nameof(_canExecuteDelete))]
    private void Delete()
    {
        _service.Delete(SelectedProduct!);

        Clear();
        Load();
    }

    [ReactiveCommand(CanExecute = nameof(_canExecuteSave))]
    private async void Save()
    {
        if (SelectedProduct is null)
        {
            var resultCheckType = await CheckType(Type!);
            var resultCheckSupplier = await CheckSupplier(Supplier!);
            if (resultCheckType && resultCheckSupplier)
            {
                _service.Add(new Product(Name!, Type!, Supplier!, Price!.Value));
            }
            else
            {
                MessageBoxManager.GetMessageBoxStandard("Warning", "Product do not added!");
            }
        }
        else
        {
            _service.Update(new Product(Id!.Value, Name!, Type!, Supplier!, Price!.Value));
        }

        Clear();
        Load();
    }

    private async Task<ButtonResult> ShowConfirmation(string text)
    {
        var box = MessageBoxManager.GetMessageBoxStandard("Confirm", text, ButtonEnum.YesNo);
        return await box.ShowAsync();
    }

    private async Task<bool> CheckType(string type)
    {
        if (ProductTypes.Contains(type)) return true;
        var result = await ShowConfirmation($"Type does not exist. Do you want to add \"{type}\"?");
        if (result == ButtonResult.Yes)
        {
            _service.AddType(type);
            return true;
        }
        return false;
    }

    private async Task<bool>CheckSupplier(string supplier)
    {
        if (Suppliers.Contains(supplier)) return true;
        var result = await ShowConfirmation($"Supplier does not exist. Do you want to add \"{supplier}\"?");
        if (result == ButtonResult.Yes)
        {
            _service.AddSupplier(supplier);
            return true;
        }
        return false;
    }
}
