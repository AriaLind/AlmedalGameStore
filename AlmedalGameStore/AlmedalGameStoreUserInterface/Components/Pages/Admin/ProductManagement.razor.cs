using AlmedalGameStoreShared.Dtos.Products;
using AlmedalGameStoreUserInterface.Services;
using Blazorise;
using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AlmedalGameStoreUserInterface.Components.Pages.Admin;

public partial class ProductManagement
{
    [CascadingParameter]
    private Task<AuthenticationState>? authenticationState { get; set; }
    [Inject] private ProductService ProductService { get; set; }

    public List<ProductDto> Products { get; set; } = new();

    public List<ProductDto> AllProductDtos { get; set; } = new();

    public string Filter { get; set; } = "All";

    private string _searchQuery = string.Empty;

    public Decimal MaxPrice { get; set; }

    public decimal MinPrice { get; set; }

    public decimal SliderValue
    {
        get; set;
    }


    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            SearchProducts(value);
        }
    }


    protected override async Task OnInitializedAsync()
    {
        var result = await ProductService.GetAllAsync();
        if (result != null)
        {
            Products = result.ToList();
            AllProductDtos = result.ToList();
        }
        await MinAndMaxGamePrice();
    }

    private async Task DeleteProduct(Guid id)
    {
        await ProductService.DeleteAsync(id);

        var result = await ProductService.GetAllAsync();
        if (result != null)
        {
            Products = result.ToList();
            AllProductDtos = result.ToList();
        }

        await CloseDeleteProductModal();
    }

    private async Task FilterProducts(string filter)
    {
        Filter = filter;

        if (Filter == "In stock")
        {
            Products = AllProductDtos.Where(p => p.Stock >= 1 && p.Price >= SliderValue).ToList();
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                await SearchProducts(SearchQuery);
            }
        }

        if (Filter == "Out of stock")
        {
            Products = AllProductDtos.Where(p => p.Stock == 0 && p.Price >= SliderValue).ToList();
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                await SearchProducts(SearchQuery);
            }
        }

        if (Filter == "All")
        {
            Products = AllProductDtos.Where(p => p.Price >= SliderValue).ToList();
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                await SearchProducts(SearchQuery);
            }
        }
    }

    private async Task SearchProducts(string SearchQuery)
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            await FilterProducts(Filter);
        }
        else
        {
            Products = Products.Where(p => p.Name.ToLower().Contains(SearchQuery.ToLower())).ToList();
        }
    }

    private async Task MinAndMaxGamePrice()
    {
        if (Products.Count == 0)
        {
            return; 
        }
        MaxPrice = Products.Max(p => p.Price);
        MinPrice = Products.Min(p => p.Price);
    }
    
    #region AddProductModal

    public ProductDto CreateProductDto { get; set; } = new();

    private Modal modalRef;

    private bool cancelClose;

    private Task ShowModal()
    {
        return modalRef.Show();
    }

    private Task CloseModal()
    {
        cancelClose = false;

        return modalRef.Hide();
    }

    private async Task TryCloseModal()
    {
        cancelClose = true;

        try
        {
            await CreateNewProduct();
            cancelClose = false;
        }
        catch
        {
            // ignore
        }

        modalRef.Hide();
    }

    private Task OnModalClosing(ModalClosingEventArgs e)
    {
        // just set Cancel to prevent modal from closing
        e.Cancel = cancelClose
                   || e.CloseReason != CloseReason.UserClosing;

        return Task.CompletedTask;
    }

    public async Task CreateNewProduct()
    {
        await ProductService.AddAsync(CreateProductDto);

        var result = await ProductService.GetAllAsync();
        if (result != null)
        {
            Products = result.ToList();
            AllProductDtos = result.ToList();
            CreateProductDto = new();
        }
    }

    #endregion

    public string ProductCategory { get; set; } = string.Empty;

    public async Task AddProductCategory(ProductDto productDto, string category)
    {
        if (productDto.Categories == null)
        {
            productDto.Categories = new List<string>();
        }

        if (category.IsNullOrEmpty())
        {
            return;
        }

        if (productDto.Categories.Contains(category))
        {
            return;
        }

        productDto.Categories.Add(category);

        StateHasChanged();
    }

    public async Task RemoveProductCategory(ProductDto productDto, string category)
    {
        if (productDto.Categories == null)
        {
            return;
        }

        if (productDto.Categories.Count == 1)
        {
            productDto.Categories = new List<string>();
            StateHasChanged();
            return;
        }

        if (!productDto.Categories.Contains(category))
        {
            return;
        }

        productDto.Categories.Remove(category);

        StateHasChanged();
    }

    #region EditProductModal

    private Modal editProductModalRef;

    private bool editProductModalCancelClose;

    private ProductDto editingProduct = new();

    private Task ShowEditProductModal(ProductDto productDto)
    {
        editingProduct = productDto;
        return editProductModalRef.Show();
    }

    private Task CloseEditProductModal()
    {
        editProductModalCancelClose = false;

        return editProductModalRef.Hide();
    }

    private async Task TryCloseEditProductModal(ProductDto productDto)
    {
        editProductModalCancelClose = true;

        try
        {
            await EditProduct(productDto);
            editProductModalCancelClose = false;
        }
        catch
        {
            // ignore
        }

        editProductModalRef.Hide();
    }

    private Task OnEditProductModalClosing(ModalClosingEventArgs e)
    {
        // just set Cancel to prevent modal from closing
        e.Cancel = cancelClose
                   || e.CloseReason != CloseReason.UserClosing;

        return Task.CompletedTask;
    }

    public async Task EditProduct(ProductDto productDto)
    {
        await ProductService.UpdateAsync(productDto);

        var result = await ProductService.GetAllAsync();
        if (result != null)
        {
            Products = result.ToList();
            AllProductDtos = result.ToList();
        }
    }

    #endregion

    #region ConfirmDeleteModal

    bool deleteProductModal = false;

    private ProductDto deleteProduct = new ();

    Task OpenDeleteProductModal(ProductDto productDto)
    {
        deleteProductModal = true;

        deleteProduct = productDto;

        return Task.CompletedTask;
    }

    Task CloseDeleteProductModal()
    {
        deleteProductModal = false;

        return Task.CompletedTask;
    }

    #endregion
}
