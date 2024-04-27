using AlmedalGameStoreShared.Dtos.Products;
using AlmedalGameStoreUserInterface.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlmedalGameStoreUserInterface.Components.Pages.Admin;

namespace AlmedalGameStoreUserInterface.Components.Pages.Shop
{
    public partial class Products : ComponentBase
    {
        [Inject] private ProductService productService { get; set; }
        [Inject] private ActiveShoppingCartService activeShoppingCartService { get; set; }
        public List<ProductDto> allProducts { get; set; } = new();  
        public List<ProductDto> filteredProducts { get; set; } = new();
        public List<ProductDto> filteredProductsByGenre { get; set; } = new();

        public string Filter { get; set; } = "All";
        public List<ProductDto> AllProductDtos { get; set; } = new();
        private string _searchQuery = string.Empty;
        public int SliderValue
        {
            get; set;
        }

        public int MaxAge { get; set; }

        public int MinAge { get; set; }

        public bool AddedToCartMessageVisible { get; set; } = false;
        public bool AddedToCartModalMessageVisible { get; set; } = false;
        private bool showModal = false;
        private ProductDto chosenProduct = new();
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
            var products = await productService.GetAllAsync();
            allProducts = products.ToList();
            filteredProducts = allProducts;
            filteredProductsByGenre = allProducts;
        }

        private async Task SearchProducts(string SearchQuery)
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                await FilterProducts(Filter);
            }
            else
            {
                filteredProducts = allProducts.Where(p => p.Name.ToLower().Contains(SearchQuery.ToLower())).ToList();
            }
        }

        private async Task FilterProducts(string filter)
        {
            Filter = filter;

            if (Filter == "All")
            {
                filteredProducts= allProducts.Where(p => p.Price >= SliderValue).ToList();
                if (!string.IsNullOrWhiteSpace(SearchQuery))
                {
                    await SearchProducts(SearchQuery);
                }
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }

        public async Task AddToCart(ProductDto product)
        {
            AddedToCartMessageVisible = true;
            await activeShoppingCartService.AddItemToCart(product);
            await ToggleAddedToCartModal();
        }

        public async Task AddToCartModal(ProductDto product)
        {
            AddedToCartModalMessageVisible = true;
            await activeShoppingCartService.AddItemToCart(product);
            await ToggleAddedToCartModal();
        }

        private async Task ShowModal(ProductDto prod)
        {
            showModal = true;
            chosenProduct = prod;
        }

        private async Task CloseModal()
        {
            showModal = false;
        }

        bool addedToCartModal = false;

        Task ToggleAddedToCartModal()
        {
            addedToCartModal = !addedToCartModal;
            return Task.CompletedTask;
        }

        string[] sortingOptions;
        private string[] filterAgeOptions;
        private string[] filterGenreOptions;

        string selectedOption;
        string selectedAgeOption;
        string selectedGenreOption;

        bool ascending = true;

        protected override void OnInitialized()
        {
            sortingOptions = new string[] { "Pris: Lägst till högst", "Pris: Högst till lägst", "Namn: A till Z", "Namn: Z till A" };

            filterAgeOptions = new string[] { "Alla", "3+", "7+", "12+", "16+", "18+" };
            filterGenreOptions = new string[] {"Alla", "RPG", "Action", "Adventure", "Stealth", "Sports", "Simulation", "Battle Royale", "Sandbox", "Casual", "Party", "FPS", "Tactical" };

            selectedOption = sortingOptions[0];
            selectedAgeOption = filterAgeOptions[0];
            selectedGenreOption = filterGenreOptions[0];
        }


        void FilterAgeProducts(string age)
        {
            selectedAgeOption = age;
            switch (selectedAgeOption)
            {
                case "Alla":
                    filteredProducts = allProducts;
                    break;
                case "3+":
                    filteredProducts = allProducts.Where(product => product.AgeRequirement <= 3).ToList();
                    break;
                case "7+":
                    filteredProducts = allProducts.Where(product => product.AgeRequirement <= 7).ToList();
                    break;
                case "12+":
                    filteredProducts = allProducts.Where(product => product.AgeRequirement <= 12).ToList();
                    break;
                case "16+":
                    filteredProducts = allProducts.Where(product => product.AgeRequirement <= 16).ToList();
                    break;
                case "18+":
                    filteredProducts = allProducts.Where(product => product.AgeRequirement <= 18).ToList();
                    break;
            }
        }

        void SortProducts(string option)
        {
            selectedOption = option;
            switch (selectedOption)
            {
                case "Pris: Lägst till högst":
                    filteredProducts = ascending ? allProducts.OrderBy(product => product.Price).ToList() : allProducts.OrderByDescending(product => product.Price).ToList();
                    break;
                case "Pris: Högst till lägst":
                    filteredProducts = ascending ? allProducts.OrderByDescending(product => product.Price).ToList() : allProducts.OrderBy(product => product.Price).ToList();
                    break;
                case "Namn: A till Z":
                    filteredProducts = ascending ? allProducts.OrderBy(product => product.Name).ToList() : allProducts.OrderByDescending(product => product.Name).ToList();
                    break;
                case "Namn: Z till A":
                    filteredProducts = ascending ? allProducts.OrderByDescending(product => product.Name).ToList() : allProducts.OrderBy(product => product.Name).ToList();
                    break;
            }
        }


        void FilterGenreProducts(string genre)
        {
            selectedGenreOption = genre;

            switch (selectedGenreOption)
            {
                case "Alla":
                    filteredProducts = allProducts;
                    break;
                default:
                    filteredProducts = allProducts.Where(product => product.Categories.Contains(selectedGenreOption)).ToList();
                    break;
            }
        }


    }
}
